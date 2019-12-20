using LibraProgramming.BlazEdit.Core;
using LibraProgramming.BlazEdit.Core.Extensions;
using LibraProgramming.BlazEdit.Events;
using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace LibraProgramming.BlazEdit.Commands
{
    public class FormatCommand : IObservableToolCommand, IMessageHandler<SelectionChangedMessage>
    {
        private readonly string checkNodeName;
        private readonly Subject<IToolCommand> subject;
        private bool applied;

        public virtual bool IsApplied
        {
            get => applied;
            set
            {
                if (applied == value)
                {
                    return;
                }

                applied = value;

                subject.OnNext(this);
            }
        }

        public string WrapNodeName
        {
            get;
        }

        public string CheckNodeName => checkNodeName ?? WrapNodeName;

        protected IEditorContext EditorContext
        {
            get;
        }

        public FormatCommand(IEditorContext editorContext, string wrapNodeName, string checkNodeName = null)
        {
            this.checkNodeName = checkNodeName;

            subject = new Subject<IToolCommand>();
            
            EditorContext = editorContext;
            WrapNodeName = wrapNodeName;
        }

        public IDisposable Subscribe(IObserver<IToolCommand> observer) => subject.Subscribe(observer);

        public virtual bool CanInvoke() => true;

        public virtual Task InvokeAsync()
        {
            var format = new SelectionFormat(WrapNodeName);
            return EditorContext.FormatSelectionAsync(format).AsTask();
        }

        Task IMessageHandler<SelectionChangedMessage>.HandleAsync(SelectionChangedMessage message)
        {
            DoSelectionChanged();
            return Task.CompletedTask;
        }

        protected virtual void DoSelectionChanged()
        {
            var selection = EditorContext.Selection;
            IsApplied = null != selection.FindNode(CheckNodeName);
        }
    }
}