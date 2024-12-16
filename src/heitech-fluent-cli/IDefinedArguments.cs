using System;
using System.Threading.Tasks;
using heitech_fluent_cli.StdIn;

namespace heitech_fluent_cli
{
    /// <summary>
    /// Access DefinedArguments and register callbacks for defined arguments.
    /// </summary>
    public interface IDefinedArguments
    {
        /// <summary>
        /// Register sync callback for Argument Type T
        /// </summary>
        /// <param name="runCommand">the callback</param>
        /// <typeparam name="T">the args type</typeparam>
        /// <returns></returns>
        DefinedArguments Is<T>(Action<T> runCommand) 
            where T : class, new();

        /// <summary>
        /// register async callback for Argument Type T
        /// </summary>
        /// <param name="runCommandAsync">the async callback</param>
        /// <typeparam name="T">the args type</typeparam>
        /// <returns></returns>
        DefinedArguments Is<T>(Func<T, Task> runCommandAsync) 
            where T : class, new();

        /// <summary>
        /// Allow stdin as input for the command
        /// </summary>
        /// <param name="runCommand">Handle the stdin Stream with this callback</param>
        /// <returns></returns>
        DefinedArguments IsStandardIn(Action<StdInArgs> runCommand);
        
        /// <summary>
        /// Build the defined arguments and combine with the registered callbacks for a runnable instance
        /// </summary>
        /// <returns></returns>
        IRunWithDefinitions Build();
    }
}