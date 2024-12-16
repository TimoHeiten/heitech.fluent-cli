using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using heitech_fluent_cli.DefineArgs;
using heitech_fluent_cli.StdIn;

namespace heitech_fluent_cli
{
    /// <summary>
    /// Access DefinedArguments and register callbacks for defined arguments.
    /// <para></para>
    /// Be aware of the sequential execution. So if you want to make sure stdin is always read first
    /// define stdin as the first step.
    /// </summary>
    public sealed class DefinedArguments
    {
        private int _count;
        private bool _readFromStdIn;
        private readonly List<IDefine> _definitions = new List<IDefine>();

        public DefinedArguments(params IDefine[] definitions)
        {
            _definitions.AddRange(definitions);
            _count = _definitions.Count;
        }

        /// <summary>
        /// One entry in cliArgs ought to be the command name
        /// </summary>
        /// <param name="cliArgs"></param>
        /// <param name="runCommand"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public DefinedArguments Is<T>(string[] cliArgs, Action<T> runCommand) where T : new()
            => Is(cliArgs, null, runCommand);

        public DefinedArguments Is<T>(string[] cliArgs, Func<T, Task> runCommandAsync) where T : new()
            => Is(cliArgs, runCommandAsync, null!);

        /// <summary>
        /// Use stdin As first setup to read from stdin 
        /// </summary>
        /// <param name="runCommand"></param>
        /// <returns></returns>
        public DefinedArguments IsStandardIn(Action<StdInArgs> runCommand)
        {
            var stdIn = StdinReader.ReadStdIn();
            if (stdIn is null) 
                return this;

            runCommand(stdIn);
            _readFromStdIn = true;

            return this;
        }

        private DefinedArguments Is<T>(string[] cliArgs, Func<T, Task>? runCommandAsync = null!, Action<T> runCommand = null!) where T : new()
        {
            if (_readFromStdIn)
                return this;

            if (cliArgs.Length == 0)
                PrintHelp();

            _count--;
            foreach (var def in _definitions)
            {
                if (!(def is CommandDefine<T> optional)) 
                    continue;

                var canBeParsed = optional.TryParse(cliArgs, out var parsed);
                if (!canBeParsed) 
                    continue;

                var msg = string.Join(", " , cliArgs);
                if (runCommand != null)
                    parsed.On(runCommand, () => LogArgs.Log("Error parsing for " + msg + " and "));
                else if (runCommandAsync != null)
                    parsed.OnAsync(runCommandAsync, () => LogArgs.Log("Error parsing for " + msg + " and "));
            }

            if (_count == 0 && cliArgs.Length > 0)
                PrintHelp();

            if (_count < 0)
            {
                throw new DefinitionException(
                    "At least one of the definitions you tried to use, was not properly set up. [use DefineXX to define multiple arguments]");
            }
            
            return this;

            void PrintHelp()
            {
                var msg = _definitions.Aggregate("", (current, arg) => current + arg.HelpText());
                // this needs to be printed in any case (not a LOG)
                Console.Write(msg);
            }
        }
    }
}