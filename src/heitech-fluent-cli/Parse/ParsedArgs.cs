using System;
using System.Threading.Tasks;

namespace heitech_fluent_cli.Parse
{
    /// <summary>
    /// The parsed result of the cli arguments
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ParsedArgs<T>
        where T : new()
    {
        private readonly T _value;
        private readonly bool _isSuccess;

        internal ParsedArgs(T value, bool isSuccess = true)
        {
            _value = value;
            _isSuccess = isSuccess;
        }

        /// <summary>
        /// provide delegates to act on the success or error case of the parsing
        /// </summary>
        /// <param name="success"></param>
        /// <param name="error"></param>
        public void On(Action<T> success, Action error)
        {
            if (_isSuccess)
                success(_value);
            else
                error();
        }

        /// <summary>
        /// provide delegates to act on the success or error case of the parsing - Async
        /// </summary>
        /// <param name="success"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public Task OnAsync(Func<T, Task> success, Action error)
        {
            if (_isSuccess)
                return success(_value);

            error();
            return Task.CompletedTask;
        }
    }
}