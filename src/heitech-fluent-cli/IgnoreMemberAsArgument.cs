using System;

namespace heitech_fluent_cli
{
    /// <summary>
    /// Marks a Member as not to be included as an argument for the Definition of Arguments
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class IgnoreMemberAsArgumentAttribute : Attribute
    { }
}