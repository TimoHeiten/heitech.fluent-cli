using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace heitech_fluent_cli
{
//
    /// <summary>
    /// Define your args ands switches
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Define<T>
        where T : new()
    {
        // switches
        private readonly List<Description> _switches = new List<Description>();

        // args
        private readonly List<Description> _args = new List<Description>();
        private readonly List<Description> _optionalArgs = new List<Description>();

        public Define<T> Argument<TA>(Expression<Func<T, TA>> expression, string longName, char? shortName = null)
        {
            AddDescription(_args, _optionalArgs.Concat(_args).ToList(), expression, longName, shortName);
            return this;
        }

        public Define<T> OptionalArgument<TA>(Expression<Func<T, TA>> expression, string longName
            , char? shortName = null)
        {
            AddDescription(_optionalArgs, _optionalArgs.Concat(_args).ToList(), expression, longName, shortName);
            return this;
        }

        public Define<T> Switch(Expression<Func<T, bool>> expression, string longName, char? shortName = null)
        {
            AddDescription(_switches, _switches, expression, longName, shortName);
            return this;
        }

        private static void AddDescription<TA>(List<Description> memberCollection
            , IReadOnlyList<Description> validateOnCollection,
            Expression<Func<T, TA>> expression, string longName, char? shortName)
        {
            var prop = (expression.Body as MemberExpression)?.Member.Name!;
            var description = new Description(shortName ?? longName[0], longName, prop, typeof(TA));

            Validate.Definition(description, validateOnCollection
                , typeof(TA) == typeof(bool) ? new[] { typeof(bool) } : null);
            memberCollection.Add(description);
        }

        public ParsedArgs<T> ParseArgs(string[] cliArgs, bool allPropertiesMustBeDefined = true
            , bool ignoreAllPropertiesSet = false)
        {
            if (allPropertiesMustBeDefined)
                Validate.AllPropertiesAreDefined<T>(_args.Concat(_optionalArgs).ToList(), _switches);

            var parseArguments = new ParseArguments<T>(_args, _optionalArgs, _switches);
            return parseArguments.ParseArgs(cliArgs, ignoreAllPropertiesSet);
        }
    }
}