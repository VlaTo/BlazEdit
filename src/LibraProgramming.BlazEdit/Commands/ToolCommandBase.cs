using System;
using System.Threading.Tasks;
using LibraProgramming.BlazEdit.Core;

namespace LibraProgramming.BlazEdit.Commands
{
    /// <summary>
    /// 
    /// </summary>
    internal abstract class ToolCommandBase : IToolCommand
    {
        /// <summary>
        /// 
        /// </summary>
        protected Func<Task> Action { get; }

        /// <summary>
        /// 
        /// </summary>
        protected Func<bool> Condition { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="condition"></param>
        protected ToolCommandBase(Func<Task> action, Func<bool> condition = null)
        {
            if (null == action)
            {
                throw new ArgumentNullException(nameof(action));
            }

            Action = action;
            Condition = condition ?? Conditions.Always;
        }

        /// <inheritdoc cref="IToolCommand.CanInvoke" />
        public abstract bool CanInvoke();

        /// <inheritdoc cref="IToolCommand.InvokeAsync" />
        public abstract Task InvokeAsync();
    }
}