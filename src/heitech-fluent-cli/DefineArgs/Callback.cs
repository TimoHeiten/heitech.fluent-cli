using System;
using System.Linq;
using System.Threading.Tasks;
using heitech_fluent_cli.Help;

namespace heitech_fluent_cli.DefineArgs
{
    /// <summary>
    /// Wrap the callback to allow for sync and async
    /// </summary>
    internal sealed class Callback
    {
        public Action<string[]>? SyncCallback { get; set; }
        public Func<string[], Task>? AsyncCallback { get; set; }

        public HelpCommand HelpCommand { get; set; } = default!;
        
        internal void CreateSyncCallBackWithParsing<T>(CommandDefine<T> optional, Action<T> runCommand)
            where T : class, new()
        {
            SyncCallback = cliArgs =>
            {
                _ = optional.TryParse(cliArgs, out var parsed);
                parsed.On(runCommand, () => OnError(cliArgs, optional));
            };
        }

        internal void Create_A_SyncCallBackWithParsing<T>(CommandDefine<T> optional, Func<T, Task> runCommandAsync)
            where T : class, new()
        {
            AsyncCallback = cliArgs =>
            {
                _ = optional.TryParse(cliArgs, out var parsed);
                return parsed.OnAsync(runCommandAsync, () => OnError(cliArgs, optional));
            };
        }

        private void OnError<T>(string[] cliArgs, CommandDefine<T> optional)
            where T : class, new()
        {
            var args = new HelpArgs() { SimpleHelp = false, SpecificCommand = optional.CommandName };
            Console.WriteLine("Cannot parse cliArgs for " + typeof(T).Name + " with args: " +
                              string.Join(", ", cliArgs.Skip(1)));
            Console.WriteLine();
            HelpCommand.Help(args);
        }
    }
}