using System;
using System.Threading.Tasks;

namespace LibraProgramming.BlazEdit.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITimeout : IDisposable
    {

    }

    /// <summary>
    /// Manages 
    /// </summary>
    public interface ITimeoutManager
    {
        /// <summary>
        /// Creates new timeout with <paramref name="callback" /> and <paramref name="timeout" /> specified.
        /// </summary>
        /// <param name="callback">The <see cref="Action" /> action to invoke.</param>
        /// <param name="timeout">The <see cref="TimeSpan" /> to delay.</param>
        /// <returns>The instance of the <see cref="ITimeout" /> created.</returns>
        ITimeout CreateTimeout(Action callback, TimeSpan timeout);

        /// <summary>
        /// Creates new timeout with <paramref name="callback" /> and <paramref name="timeout" /> specified.
        /// </summary>
        /// <param name="callback">The <see cref="System.Func{System.Threading.Tasks.Task}" /> action to invoke.</param>
        /// <param name="timeout">The <see cref="TimeSpan" /> to delay.</param>
        /// <returns>The instance of the <see cref="ITimeout" /> created.</returns>
        ITimeout CreateTimeout(Func<Task> callback, TimeSpan timeout);
    }
}