using System;
using System.Threading.Tasks;

namespace LibraProgramming.BlazEdit.Commands
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class ToolCommand : ToolCommandBase
    {
        public override string Id
        {
            get; 
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="condition"></param>
        public ToolCommand(Func<Task> action, Func<bool> condition = null)
            : base(action, condition)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="condition"></param>
        public ToolCommand(Action action, Func<bool> condition = null)
            : base(() => new Task(action), condition)
        {
        }

        /// <inheritdoc cref="ToolCommandBase.CanInvoke" />
        public override bool CanInvoke() => Condition.Invoke();

        /// <inheritdoc cref="ToolCommandBase.InvokeAsync" />
        public override Task InvokeAsync() => CanInvoke() ? Action.Invoke() : Task.CompletedTask;
    }
}