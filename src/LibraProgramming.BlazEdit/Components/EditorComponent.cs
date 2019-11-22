using LibraProgramming.BlazEdit.Commands;
using LibraProgramming.BlazEdit.Core;
using LibraProgramming.BlazEdit.Events;
using LibraProgramming.BlazEdit.TinyRx;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace LibraProgramming.BlazEdit.Components
{
    public class EditorComponent : ComponentBase, IEditorContext, IMessageHandler<SelectionChangedMessage>, IDisposable
    {
        private readonly string generatedElementId;
        private readonly BoldToolCommand boldCommand;
        private readonly ItalicToolCommand italicCommand;
        private EditorJsInterop editorInterop;
        //private EditorContext editorContext;
        private ITimeout timeout;
        private bool hasRendered;
        //private IDisposable subscription;
        private IDisposable disposable;
        private bool initialized;

        [Inject]
        public IJSRuntime JsRuntime
        {
            get; 
            set;
        }

        [Inject]
        public IMessageDispatcher MessageDispatcher
        {
            get; 
            set;
        }

        [Inject]
        public ITimeoutManager TimeoutManager
        {
            get;
            set;
        }

        [Parameter]
        public string Text
        {
            get; 
            set;
        }

        [Parameter]
        public EventCallback<string> TextChanged
        {
            get; 
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        protected IToolCommand BoldCommand => boldCommand;

        /// <summary>
        /// 
        /// </summary>
        protected IToolCommand ItalicCommand => italicCommand;

        protected object[] Paragraphs
        {
            get;
        }

        protected string EditorElementId => generatedElementId;

        protected ElementReference Temp1;

        public EditorComponent()
        {
            generatedElementId = IdManager.Instance.Generate("editor-area");

            boldCommand = new BoldToolCommand(this);
            italicCommand = new ItalicToolCommand(this);

            Paragraphs = new []
            {
                new StyleDefinition("p", "Paragraph"), 
                new StyleDefinition("h1", "Heading 1"), 
                new StyleDefinition("h2", "Heading 2"), 
                new StyleDefinition("h3", "Heading 3")
            };
        }

        public void Dispose()
        {
            disposable.Dispose();
        }

        /*bool IEditorContext.CanInvokeCommand(IToolCommand command)
        {
            return true;
        }*/

        Task IMessageHandler<SelectionChangedMessage>.HandleAsync(SelectionChangedMessage message)
        {
            return Task.CompletedTask;
        }

        Task IEditorContext.FormatSelectionAsync()
        {
            return editorInterop.FormatSelection("strong").AsTask();
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            editorInterop = new EditorJsInterop(JsRuntime, generatedElementId);
            disposable = new CompositeDisposable(
                editorInterop.Subscribe(SelectionObserver.Create(
                    e =>
                    {
                        MessageDispatcher.Publish(new SelectionChangedMessage(Selection.Empty));
                    },
                    e =>
                    {
                        var selection = new Selection(e.Text);
                        MessageDispatcher.Publish(new SelectionChangedMessage(selection));
                    })
                ),
                MessageDispatcher.Subscribe(this),
                MessageDispatcher.Subscribe(boldCommand),
                MessageDispatcher.Subscribe(italicCommand)
            );
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if (hasRendered)
            {
                await DoAssignContent();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                hasRendered = true;
                timeout = TimeoutManager.CreateTimeout(DoAssignContent, TimeSpan.FromMilliseconds(50));
            }
        }

        /*protected async Task UpdateText(string value)
        {
            Text = value;
            await TextChanged.InvokeAsync(value);
        }*/

        /*private void UpdateCommand(IToolCommand command)
        {
            MessageAggregator.Publish(new CommandMessage(command));
        }*/

        // https://github.com/cloudcrate/BlazorStorage/blob/master/src/Storage.cs
        private async Task EnsureInitializedAsync()
        {
            if (initialized)
            {
                return ;
            }

            initialized = true;

            await editorInterop.InitializeEditorAsync();
        }

        private async Task DoAssignContent()
        {
            if (null != timeout)
            {
                timeout.Dispose();
                timeout = null;
            }

            await EnsureInitializedAsync();
            await editorInterop.SetContent(Text);
        }

        /// <summary>
        /// 
        /// </summary>
        /*private class EditorContext : IEditorContext
        {
            private readonly ElementReference editorElement;
            private readonly IMessageAggregator messageAggregator;
            private readonly IEditorJSInterop editorInterop;
            private readonly Dictionary<IToolCommand, bool> commands;
            private bool initialized;

            public IToolCommand MakeBold { get; }

            public IToolCommand MakeItalic { get; }

            public EditorContext(ElementReference editorElement, IMessageAggregator messageAggregator, IEditorJSInterop editorInterop)
            {
                this.editorElement = editorElement;
                this.messageAggregator = messageAggregator;
                this.editorInterop = editorInterop;
                
                commands = new Dictionary<IToolCommand, bool>();

                MakeBold = new ToolCommand(DoMakeBoldAsync, CanMakeBold);
                MakeItalic = new ToolCommand(DoMakeItalicAsync, CanMakeItalic);
            }

            public async Task<string> GetContent()
            {
                return await editorInterop.GetContent();
            }

            public async Task SetContentAsync(string content)
            {
                await EnsureInitializedAsync();
                await editorInterop.SetContent(content);
            }

            private Task EnsureInitializedAsync()
            {
                if (initialized)
                {
                    return Task.CompletedTask;
                }

                initialized = true;

                return editorInterop.InitializeEditorAsync().AsTask();
            }

            private Task DoMakeBoldAsync()
            {
                return editorInterop.Apply("strong").AsTask();
            }

            private async Task DoMakeItalicAsync()
            {
                Debug.WriteLine("EditorComponent.DoMakeItalic");
                await Task.CompletedTask;
            }

            private bool CanMakeBold()
            {
                return true;
            }

            private bool CanMakeItalic()
            {
                return true;
            }
        }*/
    }
}