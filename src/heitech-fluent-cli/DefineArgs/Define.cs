using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using heitech_fluent_cli.Parse;

namespace heitech_fluent_cli.DefineArgs
{
    /// <summary>
    /// Represents a single defined argument
    /// </summary>
    public interface IDefine
    {
        string CommandName { get; }
        string HelpText();
    }

    /// <summary>
    /// Define your args ands switches
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal abstract class Define<T> 
        : IDefine, IDefine<T>
        where T : new()
    {
        public string CommandName { get; internal set; } = default!;

        // switches
        private readonly List<Description> _switches = new List<Description>();

        // args
        private readonly List<Description> _args = new List<Description>();
        private readonly List<Description> _optionalArgs = new List<Description>();

        public IDefine<T> Name(string name)
        {
            CommandName = name;
            return this;
        }
        
        public IDefine<T> Argument<TA>(Expression<Func<T, TA>> expression, string longName, char? shortName = null, string describe = null!)
        {
            AddDescription(_args, _optionalArgs.Concat(_args).ToList(), expression, longName, shortName, describe);
            return this;
        }

        public IDefine<T> OptionalArgument<TA>(Expression<Func<T, TA>> expression, string longName
            , char? shortName = null, string describe = null!)
        {
            AddDescription(_optionalArgs, _optionalArgs.Concat(_args).ToList(), expression, longName, shortName, describe);
            return this;
        }

        public IDefine<T> Switch(Expression<Func<T, bool>> expression, string longName, char? shortName = null, string describe = null!)
        {
            AddDescription(_switches, _switches, expression, longName, shortName, describe);
            return this;
        }

        private static void AddDescription<TA>(List<Description> memberCollection
            , IReadOnlyList<Description> validateOnCollection,
            Expression<Func<T, TA>> expression, string longName, char? shortName, string describe)
        {
            var prop = (expression.Body as MemberExpression)?.Member.Name!;
            var description = new Description(shortName ?? longName[0], longName, prop, typeof(TA), describe);

            Validate.Definition(description, validateOnCollection
                , typeof(TA) == typeof(bool) ? new[] { typeof(bool) } : null);
            memberCollection.Add(description);
        }

        /// <summary>
        /// Try parse the arguments
        /// </summary>
        /// <param name="cliArgs"></param>
        /// <param name="parsedArgs"></param>
        /// <returns></returns>
        public abstract bool TryParse(string[] cliArgs, out ParsedArgs<T> parsedArgs);

        public ParsedArgs<T> ParseArgs(string[] cliArgs, bool allPropertiesMustBeDefined = true
            , bool ignoreAllPropertiesSet = false)
        {
            if (allPropertiesMustBeDefined)
                Validate.AllPropertiesAreDefined<T>(_args.Concat(_optionalArgs).ToList(), _switches);

            var parseArguments = new ParseArguments<T>(_args, _optionalArgs, _switches);
            return parseArguments.ParseArgs(cliArgs, ignoreAllPropertiesSet);
        }

        public string HelpText()
        {
            var args = _args.Select(x => $"{x.Describe ?? x.PropertyName}: -{x.ShortName} --{x.LongName}");
            var switches = _switches.Select(x => $"{x.Describe ?? x.PropertyName}: -{x.ShortName} --{x.LongName} ");
            var optionalArgs = _optionalArgs.Select(x => $"{x.Describe ?? x.PropertyName}: -{x.ShortName} --{x.LongName}");

            return $"Args:{Environment.NewLine}\t{ToTabbedLines(args)}{Environment.NewLine}" +
                   $"Switches:{Environment.NewLine}\t{ToTabbedLines(switches)}{Environment.NewLine}" +
                   $"Optional Args:{Environment.NewLine}\t{ToTabbedLines(optionalArgs)}";

            string ToTabbedLines(IEnumerable<string> lines)
                => string.Join($"{Environment.NewLine}\t", lines);
        }
    }
}