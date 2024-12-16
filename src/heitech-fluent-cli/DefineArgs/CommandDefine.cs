using System;
using System.Linq;
using heitech_fluent_cli.Parse;

namespace heitech_fluent_cli.DefineArgs
{
    internal sealed class CommandDefine<T> : Define<T> where T : new()
    {
        public override bool TryParse(string[] cliArgs, out ParsedArgs<T> parsedArgs)
        {
            parsedArgs = default!;
            var commandName = CommandName ?? throw new DefinitionException("CommandName must be set");
            if (cliArgs.Any() is false || (cliArgs[0] != commandName))
                return false;

            if (!cliArgs.Any(x => string.Equals(x, CommandName, StringComparison.CurrentCultureIgnoreCase)))
                return false;

            try
            {
                parsedArgs = ParseArgs(cliArgs);
                return true;
            }
            catch (Exception e)
            {
                LogArgs.Log(e.Message);
                return false;
            }
        } 
    }
}