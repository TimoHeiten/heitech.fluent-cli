using System;
using System.Linq.Expressions;
using heitech_fluent_cli.Parse;

namespace heitech_fluent_cli
{
    /// <summary>
    /// Definition for a single argument
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDefine<T> where T : new()
    {
        /// <summary>
        /// The command name associated with this arguments
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IDefine<T> Name(string name);
        /// <summary>
        /// Define a new argument. (like -n "value", or --name "value")
        /// </summary>
        /// <param name="expression">the property expression</param>
        /// <param name="longName">the name</param>
        /// <param name="shortName">character - defaults to first letter of longName</param>
        /// <typeparam name="TA"></typeparam>
        /// <returns></returns>
        IDefine<T> Argument<TA>(Expression<Func<T, TA>> expression, string longName, char? shortName = null);

        /// <summary>
        /// Define an optional argument. (like -n "value", or --name "value")
        /// </summary>
        /// <param name="expression">the property expression</param>
        /// <param name="longName">the name</param>
        /// <param name="shortName">character - defaults to first letter of longName</param>
        /// <typeparam name="TA"></typeparam>
        /// <returns></returns>
        IDefine<T> OptionalArgument<TA>(Expression<Func<T, TA>> expression, string longName, char? shortName = null);
        /// <summary>
        /// Define a switch (like -e or --enabled)
        /// </summary>
        /// <param name="expression">the property expression</param>
        /// <param name="longName">the name</param>
        /// <param name="shortName">character - defaults to first letter of longName</param>
        /// <returns></returns>
        IDefine<T> Switch(Expression<Func<T, bool>> expression, string longName, char? shortName = null);

        /// <summary>
        /// Try to parse the arguments
        /// </summary>
        /// <param name="args"></param>
        /// <param name="parsedArgs"></param>
        /// <returns></returns>
        bool TryParse(string[] args, out ParsedArgs<T> parsedArgs);

        /// <summary>
        /// Parse arguments, may throw
        /// </summary>
        /// <param name="args"></param>
        /// <param name="allPropertiesMustBeDefined"></param>
        /// <param name="ignoreAllPropertiesSet"></param>
        /// <returns></returns>
        ParsedArgs<T> ParseArgs(string[] args, bool allPropertiesMustBeDefined = true, bool ignoreAllPropertiesSet = false);
    }
}