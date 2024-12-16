using System;

namespace heitech_fluent_cli
{
    public static class LogArgs
    {
        public static bool Enabled { get; set; } = false;
        
        public static void Log(string message, Action<string>? logOverload = null)
        {
            if (!Enabled) 
                return;

            if (logOverload == null)
            {
                Console.WriteLine(message);
                return;
            }

            logOverload(message);
        }
    }
}