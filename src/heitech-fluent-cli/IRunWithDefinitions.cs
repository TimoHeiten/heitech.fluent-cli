using System.Threading.Tasks;

namespace heitech_fluent_cli
{
    /// <summary>
    /// AFter all definitions and callbacks are setup, call the Run Method to actually start the parsing and execution
    /// </summary>
    public interface IRunWithDefinitions
    {
        Task Run(string[] cliArgs);
    }
}