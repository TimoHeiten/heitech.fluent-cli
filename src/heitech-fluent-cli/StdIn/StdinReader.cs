using System;
using System.IO;

namespace heitech_fluent_cli.StdIn
{
    internal static class StdinReader
    {
        internal static StdInArgs? ReadStdIn()
        {
            var hasStandardIn = Console.IsInputRedirected && Console.In.Peek() != -1;
            return hasStandardIn ? new StdInArgs { IncomingStream = Console.In } : null;
        }
    }

    public sealed class StdInArgs
    {
        public TextReader IncomingStream { get; internal set; } = default!;
    }
}