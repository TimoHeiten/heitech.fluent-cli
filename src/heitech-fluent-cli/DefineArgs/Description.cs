using System;

namespace heitech_fluent_cli.DefineArgs
{

    /// <summary>
    /// Describes a switch or an argument
    /// </summary>
    internal sealed class Description
    {
        /// <summary>
        /// Describes a switch or an argument
        /// </summary>
        /// <param name="shortName"></param>
        /// <param name="longName"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyType"></param>
        /// <param name="describe"></param>
        public Description(char shortName, string longName, string propertyName, Type propertyType, string describe = null!)
        {
            LongName = longName;
            Describe = describe;
            ShortName = shortName;
            PropertyName = propertyName;
            PropertyType = propertyType;
        }

        public Type PropertyType { get; set; }
        public string PropertyName { get; set; }
        public string LongName { get; set; }
        public char ShortName { get; set; }

        public string? Describe { get; set; }
    }
}