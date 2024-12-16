using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using heitech_fluent_cli.DefineArgs;

namespace heitech_fluent_cli.Parse
{
    /// <summary>
    /// Encapsulates the parsing of the given arguments, according to the definition of arguments via the Description type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class ParseArguments<T>
        where T : new()
    {
        private List<Description> _args;
        private List<Description> _switches;
        private List<Description> _optionalArgs;

        internal ParseArguments(List<Description> args, List<Description> optionalArgs, List<Description> switches)
            => (_args, _optionalArgs, _switches) = (args, optionalArgs, switches);

        private static IEnumerable<(string name, string? value)> GetTuplesOfArgAndValue(string[] cliArgs)
        {
            for (int i = 0; i < cliArgs.Length; i++)
            {
                var cnt = cliArgs[i];
                if (!cnt.StartsWith("-")) // no arg but a value for the arg at i-1
                {
                    continue;
                }

                // arg value combi
                if (i + 1 < cliArgs.Length && !cliArgs[i + 1].StartsWith('-'))
                    yield return (GetCntArgNameWithoutHyphensPrefix(cnt), cliArgs[i + 1]);
                // switch
                else
                    yield return (GetCntArgNameWithoutHyphensPrefix(cnt), null);
            }

            static string GetCntArgNameWithoutHyphensPrefix(string argName)
            {
                if (argName.StartsWith("--"))
                    return argName[2..];

                return argName.StartsWith('-') ? argName[1..] : argName;
            }
        }

        internal ParsedArgs<T> ParseArgs(string[] cliArgs, bool ignoreNotAllPropertiesSet)
        {
            var argsInstance = new T();
            var propInfos = typeof(T).GetProperties();

            var tuples = GetTuplesOfArgAndValue(cliArgs).ToList();
            foreach (var cliArg in tuples)
            {
                List<Description> collectionToOperateOn = _args;
                Func<Description, object?> converter = d => ParseValue(d.PropertyType, cliArg.value!);

                // if value is null, it is a switch, so change the collection and converter
                if (cliArg.value is null)
                {
                    converter = d => true;
                    collectionToOperateOn = _switches;
                }

                // try with arg / switch (depending on the tuple)
                bool wasSet = TrySetValue(ref argsInstance, cliArg, ref collectionToOperateOn, propInfos, converter);
                // retry with optionalArgs
                wasSet |= TrySetValue(ref argsInstance, cliArg, ref _optionalArgs, propInfos,
                    d => ParseValue(d.PropertyType, cliArg.value!));

                if (wasSet)
                    continue;

                LogArgs.Log($"Argument/switch '{cliArg.name}' has not been defined");
                return new ParsedArgs<T>(default, false);
            }

            var notAllArgsAreSet = _args.Count != 0;
            if (notAllArgsAreSet && !ignoreNotAllPropertiesSet)
            {
                var msg = string.Join(Environment.NewLine
                    , _args.Select(x =>
                        $"Defined (non optional) Argument '-{x.ShortName}, --{x.LongName}' has not been assigned a value"));
                LogArgs.Log(msg);
                return new ParsedArgs<T>(argsInstance, ignoreNotAllPropertiesSet);
            }

            return new ParsedArgs<T>(argsInstance);
        }

        private static bool TrySetValue(ref T instance,
            (string name, string? value) cliArg,
            ref List<Description> argsOrSwitches,
            IEnumerable<PropertyInfo> argumentObjectsPropertyInfos,
            Func<Description, object?> converter)
        {
            var descr = argsOrSwitches.FirstOrDefault(x => x.ShortName == cliArg.name[0] || x.LongName == cliArg.name);
            if (descr is null)
                return false;

            var propertyToBeSet = argumentObjectsPropertyInfos.Single(x => x.Name == descr.PropertyName);
            var value = converter(descr);
            if (value is null)
                return false;

            propertyToBeSet.SetValue(instance, value);

            // remove the arg/switch from the list.
            argsOrSwitches.RemoveAt(argsOrSwitches.IndexOf(descr));

            return true;
        }

        private void PrintHelp()
        {
            var switches = string.Join(Environment.NewLine, _switches.Select(x => $"-{x.ShortName}, --{x.LongName}"));
            var args = string.Join(Environment.NewLine, _args.Select(x => $"-{x.ShortName}, --{x.LongName}"));
            LogArgs.Log($"Switches: {switches}");
            LogArgs.Log($"Args: {args}");
        }

        private static object? ParseValue(Type type, string argValue)
        {
            try
            {
                return type switch
                {
                    { } t when t == typeof(string) => argValue
                    , { } t when t == typeof(char) => argValue.ToCharArray()[0]
                    , { IsPrimitive: true } t => Convert.ChangeType(argValue, t)
                    , _ => throw new Exception("Type not supported")
                };
            }
            catch
            {
                return null!;
            }

        }
    }
}