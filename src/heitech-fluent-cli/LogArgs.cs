using System;

namespace heitech_fluent_cli
{
    /// <summary>
    /// Logger - console by default, but can be overloaded via callback
    /// </summary>
    public static class LogArgs
    {
        /// <summary>
        /// Default is false 
        /// </summary>
        public static bool Enabled { get; set; } = false;
        /// <summary>
        /// The logger overload callback - defualt is just console.writeline
        /// </summary>
        public static Action<string>? LogOverload { get; set; } = null!;
        
        internal static void Log(string message)
        {
            if (!Enabled) 
                return;

            if (LogOverload == null)
            {
                Console.WriteLine(message);
                return;
            }

            LogOverload(message);
        }
    }
}