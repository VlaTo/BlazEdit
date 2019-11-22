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
        private IEditorJSInterop editor;
        private ITimeout timeout;
        private bool hasRendered;
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

        Task IMessageHandler<SelectionChangedMessage>.HandleAsync(SelectionChangedMessage message)
        {
            return Task.CompletedTask;
        }

        Task IEditorContext.FormatSelectionAsync(SelectionFormat format) => editor.FormatSelectionAsync(format);

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var editorInterop = new EditorJsInterop(JsRuntime, generatedElementId);

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

            editor = editorInterop;
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

        // https://github.com/cloudcrate/BlazorStorage/blob/master/src/Storage.cs
        private async Task EnsureInitializedAsync()
        {
            if (initialized)
            {
                return ;
            }

            initialized = true;

            await editor.InitializeEditorAsync();
        }

        private async Task DoAssignContent()
        {
            if (null != timeout)
            {
                timeout.Dispose();
                timeout = null;
            }

            await EnsureInitializedAsync();
            await editor.SetContentAsync(Text);
        }
    }
}