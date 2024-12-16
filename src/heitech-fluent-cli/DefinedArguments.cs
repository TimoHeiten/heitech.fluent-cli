using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using heitech_fluent_cli.DefineArgs;
using heitech_fluent_cli.StdIn;

namespace heitech_fluent_cli
{
    /// <summary>
    /// <inheritdoc cref="IDefinedArguments"/>
    /// </summary>
    public sealed class DefinedArguments : IDefinedArguments
    {
        private bool _readFromStdIn;
        private readonly List<IDefine> _definitions = new List<IDefine>();

        internal IEnumerable<IDefine> Definitions => _definitions;
        private readonly DefinedArgsParser _argsParser;

        internal DefinedArguments(params IDefine[] definitions)
        {
            _definitions.AddRange(definitions);
            _argsParser = new DefinedArgsParser(this);
        }

        public DefinedArguments Is<T>(Action<T> runCommand) where T : class,new()
            => Add(null, runCommand);

        public DefinedArguments Is<T>(Func<T, Task> runCommandAsync) where T : class, new()
            => Add(runCommandAsync, null!);

        public DefinedArguments IsStandardIn(Action<StdInArgs> runCommand)
        {
            var stdIn = StdinReader.ReadStdIn();
            if (stdIn is null) 
                return this;

            _argsParser.ReadStdin = () =>
            {
                runCommand(stdIn);
            };

            _readFromStdIn = true;
            return this;
        }

        private DefinedArguments Add<T>(Func<T, Task>? runCommandAsync = null!, Action<T> runCommand = null!) 
            where T : class, new()
        {
            if (_readFromStdIn)
            {
                _argsParser.IsStdIn = true;
                return this;
            }

            var define = _definitions.FirstOrDefault(x => x.IsType(typeof(T)));
            if (!(define is CommandDefine<T> commandDefine))
            {
                throw new DefinitionException($"missing a definition for {typeof(T).Name}");
            }

            var cb = new Callback();
            if (runCommandAsync != null)
                cb.Create_A_SyncCallBackWithParsing(commandDefine, runCommandAsync);
            else 
                cb.CreateSyncCallBackWithParsing(commandDefine, runCommand);

            _argsParser.Add(cb, define.CommandName);

            return this;
        }

        /// <summary>
        /// Combine definitions and callbacks to a runnable instance
        /// </summary>
        /// <returns></returns>
        public IRunWithDefinitions Build()
            => _argsParser;
    }
}