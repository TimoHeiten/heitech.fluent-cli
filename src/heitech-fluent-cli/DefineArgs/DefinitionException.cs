using System;

namespace heitech_fluent_cli.DefineArgs
{
    public sealed class DefinitionException : Exception
    {
        public DefinitionException(string msg) : base(msg)
        { }
    }
}