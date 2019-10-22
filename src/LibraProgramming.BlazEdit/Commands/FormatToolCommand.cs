using System;
using System.Threading.Tasks;
using LibraProgramming.BlazEdit.Core;
using LibraProgramming.BlazEdit.Events;

namespace LibraProgramming.BlazEdit.Commands
{
    internal abstract class FormatToolCommand : IToolCommand, IMessageHandler<SelectionChangedMessage>
    {
        protected bool CanBeApplied
        {
            get; 
            private set;
        }

        protected IEditorContext EditorContext
        {
            get;
        }

        protected FormatToolCommand(IEditorContext editorContext)
        {
            EditorContext = editorContext;
        }

        public virtual bool CanInvoke()
        {
            return CanBeApplied && EditorContext.CanInvokeCommand(this);
        }

        public abstract Task InvokeAsync();

        Task IMessageHandler<SelectionChangedMessage>.HandleAsync(SelectionChangedMessage message)
        {
            var notEmpty = false == String.IsNullOrEmpty(message.Text);

            if (notEmpty != CanBeApplied)
            {
                CanBeApplied = notEmpty;
                return DoSelectionChanged();
            }

            return Task.CompletedTask;
        }

        protected virtual Task DoSelectionChanged()
        {
            return Task.CompletedTask;
        }
    }
}