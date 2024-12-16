using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using heitech_fluent_cli.DefineArgs;
using heitech_fluent_cli.Help;

namespace heitech_fluent_cli
{
    /// <summary>
    /// <inheritdoc cref="IRunWithDefinitions"/>
    /// </summary>
    internal sealed class DefinedArgsParser : IRunWithDefinitions
    {
        private readonly DefinedArguments _definedArguments;
        private readonly HelpCommand _help;
        private readonly Dictionary<string, Callback> _registeredCallbacks = new Dictionary<string, Callback>();

        internal DefinedArgsParser(DefinedArguments definedArguments)
        {
            _definedArguments = definedArguments;
            _help = new HelpCommand(definedArguments);
        }

        internal void Add(Callback cb, string commandName)
        {
            cb.HelpCommand = _help;
            _registeredCallbacks.Add(commandName, cb);
        }

        public bool IsStdIn { get; set; }
        public Action ReadStdin { get; set; } = default!;

        private static string CommandName(string[] cliArgs)
            => cliArgs.First();

        /// <summary>
        /// Parse the cliArgs and match to definitions if possible
        /// </summary>
        /// <param name="cliArgs"></param>
        public async Task Run(string[] cliArgs)
        {
            if (IsStdIn)
            {
                ReadStdin();
                return;
            }

            if (cliArgs.Length == 0)
            {
                PrintSimpleHelp("no arguments provided");
                return;
            }

            var cmdName = CommandName(cliArgs);
            if (cmdName == "help")
            {
                if (cliArgs.Length > 1)
                {
                    _help.Help(new HelpArgs { SimpleHelp = false, SpecificCommand = cliArgs.Skip(1).FirstOrDefault() ?? ""});
                }
                else
                {
                    PrintSimpleHelp();
                }
                return;
            }

            var isDefined = _registeredCallbacks.TryGetValue(cmdName, out var cb);
            if (!isDefined)
            {
                PrintSimpleHelp($"Command not found '{cmdName}'");
                return;
            }

            if (cb!.AsyncCallback != null)
            {
                await cb.AsyncCallback(cliArgs);
                return;
            }

            if (cb!.SyncCallback != null)
            {
                cb.SyncCallback(cliArgs);
                return;
            }

            return;

            void PrintSimpleHelp(string preMessage = "")
            {
                if (!string.IsNullOrWhiteSpace(preMessage))
                    Console.WriteLine(preMessage);

                _help.Help(new HelpArgs {SimpleHelp = true});
            }
        }
    }
}