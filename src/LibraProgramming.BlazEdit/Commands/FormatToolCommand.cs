using LibraProgramming.BlazEdit.Core;
using LibraProgramming.BlazEdit.Events;
using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace LibraProgramming.BlazEdit.Commands
{
    public abstract class FormatToolCommand : IObservableToolCommand, IMessageHandler<SelectionChangedMessage>
    {
        private readonly Subject<IToolCommand> subject;
        private bool applied;

        public virtual bool IsApplied
        {
            get => applied;
            set
            {
                /*if (applied == value)
                {
                    return;
                }*/

                applied = value;

                subject.OnNext(this);
            }
        }

        protected IEditorContext EditorContext
        {
            get;
        }

        protected FormatToolCommand(IEditorContext editorContext)
        {
            subject = new Subject<IToolCommand>();
            EditorContext = editorContext;
        }

        public IDisposable Subscribe(IObserver<IToolCommand> observer) => subject.Subscribe(observer);

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

        protected abstract Task DoSelectionChangedAsync(Selection selection);
    }
}