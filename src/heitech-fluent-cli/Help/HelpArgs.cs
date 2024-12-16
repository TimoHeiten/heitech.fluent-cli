using System;
using System.Linq;
using heitech_fluent_cli.DefineArgs;

namespace heitech_fluent_cli.Help
{
    public sealed class HelpArgs
    {
        public bool SimpleHelp { get; set; }
        public string SpecificCommand { get; set; } = default!;
    }

    public sealed class HelpCommand
    {
        public IDefine HelpMe()
        {
            var df = new CommandDefine<HelpArgs>()
                .Name("help", "Prints the help for all commands or a specific description for a given command")
                .Switch(x => x.SimpleHelp, "simple", 's', "Prints a simple help")
                .Argument(h => h.SpecificCommand, "command", 'c', "Prints the help for a specific command");

            return df;
        }

        private readonly DefinedArguments _definedArguments;
        public HelpCommand(DefinedArguments definedArguments)
            => _definedArguments = definedArguments;

        public void Help(HelpArgs args)
        {
            if (args.SimpleHelp)
            {
                SimpleHelp();
                return;
            }

            var specificCommand = _definedArguments.Definitions.FirstOrDefault(x => x.CommandName == args.SpecificCommand);
            if (specificCommand is null)
            {
                Console.WriteLine($"Command '{args.SpecificCommand}' not found");
                return;
            }

            Console.WriteLine(specificCommand.HelpText());
        }

        private void SimpleHelp()
        {
            Console.WriteLine();
            var cmds = _definedArguments.Definitions.Select(x => $"{x.CommandName}\t- {x.Describe ?? "-"}");
            Console.WriteLine("Available Commands:");
            Console.WriteLine(string.Join(Environment.NewLine, cmds));
            Console.WriteLine("Use --help <command> | -h <command> for more information");
            Console.WriteLine();
        }
    }
}