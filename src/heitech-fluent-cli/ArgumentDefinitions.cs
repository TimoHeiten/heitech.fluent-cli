using System;
using heitech_fluent_cli.DefineArgs;
using heitech_fluent_cli.Help;

namespace heitech_fluent_cli
{
    public static class ArgumentDefinitions
    {
        /// <summary>
        /// Define one argument
        /// </summary>
        /// <param name="defineArgs"></param>
        /// <typeparam name="TArgs"></typeparam>
        /// <returns></returns>
        public static DefinedArguments Define<TArgs>(Action<IDefine<TArgs>> defineArgs) where TArgs : new()
        {
            var def = new CommandDefine<TArgs>();
            defineArgs(def);

            var defined = new DefinedArguments(def);
            var help = new HelpCommand(defined);
            defined.HelpCommand = help;

            return defined;
        }

        /// <summary>
        /// Define two argumenst
        /// </summary>
        /// <param name="defineArgs"></param>
        /// <param name="defineArgs2"></param>
        /// <typeparam name="TArgs"></typeparam>
        /// <typeparam name="TArgs2"></typeparam>
        /// <returns></returns>
        public static DefinedArguments Define<TArgs, TArgs2>(Action<IDefine<TArgs>> defineArgs, Action<IDefine<TArgs2>> defineArgs2) 
            where TArgs : new()
            where TArgs2 : new()
        {
            var def = new CommandDefine<TArgs>();
            defineArgs(def);
            var def2 = new CommandDefine<TArgs2>();
            defineArgs2(def2);

            var defined = new DefinedArguments(def, def2);
            var help = new HelpCommand(defined);
            defined.HelpCommand = help;
            return defined;
        }

        /// <summary>
        /// Define three arguments
        /// </summary>
        /// <param name="defineArgs"></param>
        /// <param name="defineArgs2"></param>
        /// <param name="defineArgs3"></param>
        /// <typeparam name="TArgs"></typeparam>
        /// <typeparam name="TArgs2"></typeparam>
        /// <typeparam name="TArgs3"></typeparam>
        /// <returns></returns>
        public static DefinedArguments Define<TArgs, TArgs2, TArgs3>(Action<IDefine<TArgs>> defineArgs, 
            Action<IDefine<TArgs2>> defineArgs2,
            Action<IDefine<TArgs3>> defineArgs3) 
            where TArgs : new()
            where TArgs2 : new()
            where TArgs3 : new()
        {
            var def = new CommandDefine<TArgs>();
            defineArgs(def);
            var def2 = new CommandDefine<TArgs2>();
            defineArgs2(def2);
            var def3 = new CommandDefine<TArgs3>();
            defineArgs3(def3);

            var defined = new DefinedArguments(def, def2, def3);
            var help = new HelpCommand(defined);
            defined.HelpCommand = help;
            return defined;
        }

        /// <summary>
        /// Define four arguments
        /// </summary>
        /// <param name="defineArgs"></param>
        /// <param name="defineArgs2"></param>
        /// <param name="defineArgs3"></param>
        /// <param name="defineArgs4"></param>
        /// <typeparam name="TArgs"></typeparam>
        /// <typeparam name="TArgs2"></typeparam>
        /// <typeparam name="TArgs3"></typeparam>
        /// <typeparam name="TArgs4"></typeparam>
        /// <returns></returns>
        public static DefinedArguments Define<TArgs, TArgs2, TArgs3, TArgs4>(Action<IDefine<TArgs>> defineArgs, 
            Action<IDefine<TArgs2>> defineArgs2,
            Action<IDefine<TArgs3>> defineArgs3,
            Action<IDefine<TArgs4>> defineArgs4)
            where TArgs : new()
            where TArgs2 : new()
            where TArgs3 : new()
            where TArgs4 : new()
        {
            var def = new CommandDefine<TArgs>();
            defineArgs(def);
            var def2 = new CommandDefine<TArgs2>();
            defineArgs2(def2);
            var def3 = new CommandDefine<TArgs3>();
            defineArgs3(def3);
            var def4 = new CommandDefine<TArgs4>();
            defineArgs4(def4);

            var defined = new DefinedArguments(def, def2, def3, def4);
            var help = new HelpCommand(defined);
            defined.HelpCommand = help;
            return defined;
        }

        /// <summary>
        /// Define five arguments
        /// </summary>
        /// <param name="defineArgs"></param>
        /// <param name="defineArgs2"></param>
        /// <param name="defineArgs3"></param>
        /// <param name="defineArgs4"></param>
        /// <param name="defineArgs5"></param>
        /// <typeparam name="TArgs"></typeparam>
        /// <typeparam name="TArgs2"></typeparam>
        /// <typeparam name="TArgs3"></typeparam>
        /// <typeparam name="TArgs4"></typeparam>
        /// <typeparam name="TArgs5"></typeparam>
        /// <returns></returns>
        public static DefinedArguments Define<TArgs, TArgs2, TArgs3, TArgs4, TArgs5>(Action<IDefine<TArgs>> defineArgs, 
            Action<IDefine<TArgs2>> defineArgs2,
            Action<IDefine<TArgs3>> defineArgs3,
            Action<IDefine<TArgs4>> defineArgs4,
            Action<IDefine<TArgs5>> defineArgs5)
            where TArgs : new()
            where TArgs2 : new()
            where TArgs3 : new()
            where TArgs4 : new()
            where TArgs5 : new()
        {
            var def = new CommandDefine<TArgs>();
            defineArgs(def);
            var def2 = new CommandDefine<TArgs2>();
            defineArgs2(def2);
            var def3 = new CommandDefine<TArgs3>();
            defineArgs3(def3);
            var def4 = new CommandDefine<TArgs4>();
            defineArgs4(def4);
            var def5 = new CommandDefine<TArgs5>();
            defineArgs5(def5);
            
            var defined = new DefinedArguments(def, def2, def3, def4, def5);
            var help = new HelpCommand(defined);
            defined.HelpCommand = help;
            return defined;
        }
    }
}