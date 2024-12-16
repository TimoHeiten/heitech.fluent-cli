using System;

namespace heitech_fluent_cli.DefineArgs
{
    /// <summary>
    /// Something went wrong with the definition of the an argument
    /// </summary>
    public sealed class DefinitionException : Exception
    {
        public DefinitionException(string msg) : base(msg)
        { }
    }
}