using LibraProgramming.BlazEdit.Core;
using LibraProgramming.BlazEdit.Events;
using System;
using System.Threading.Tasks;

namespace LibraProgramming.BlazEdit.Commands
{
    internal abstract class FormatToolCommand : IToolCommand, IMessageHandler<SelectionChangedMessage>
    {
        protected IEditorContext EditorContext
        {
            get;
        }

        protected FormatToolCommand(IEditorContext editorContext)
        {
            EditorContext = editorContext;
        }

        public virtual bool CanInvoke() => false;

        public abstract Task InvokeAsync();

        Task IMessageHandler<SelectionChangedMessage>.HandleAsync(SelectionChangedMessage message)
        {
            var selection = message.Selection;

            if (false == String.IsNullOrEmpty(selection.GetSelectionText()))
            {
                ;
            }

            return DoSelectionChangedAsync(selection);
        }

        protected virtual Task DoSelectionChangedAsync(Selection selection) => Task.CompletedTask;
    }
}