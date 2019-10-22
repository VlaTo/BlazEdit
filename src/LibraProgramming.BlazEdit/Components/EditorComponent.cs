using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using LibraProgramming.BlazEdit.Commands;
using LibraProgramming.BlazEdit.Core;
using LibraProgramming.BlazEdit.Events;
using LibraProgramming.BlazEdit.TinyRx;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LibraProgramming.BlazEdit.Components
{
    public class EditorComponent : ComponentBase, IEditorContext, IMessageHandler<RequireSelectionMessage>, IDisposable
    {
        private readonly string generatedElementId;
        private readonly BoldToolCommand boldCommand;
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
        public IMessageAggregator MessageAggregator
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
        public string Text { get; set; }

        [Parameter]
        public EventCallback<string> TextChanged { get; set; }

        protected IToolCommand MakeBold => boldCommand;

        protected IToolCommand MakeItalic
        {
            get;
        }

        protected object[] Paragraphs { get; }

        protected string EditorElementId => generatedElementId;

        protected ElementReference Temp1;

        public EditorComponent()
        {
            generatedElementId = IdManager.Instance.Generate("editor-area");

            boldCommand = new BoldToolCommand(this);
            MakeItalic = new ToolCommand(DoMakeItalicAsync, () => true);

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

        Task IMessageHandler<RequireSelectionMessage>.HandleAsync(RequireSelectionMessage message)
        {
            Text = "<p>Sample text</p>";
            return TextChanged.InvokeAsync(Text);
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            editorInterop = new EditorJsInterop(JsRuntime, generatedElementId);
            disposable = new CompositeDisposable(
                editorInterop.Subscribe(SelectionObserver.Create(OnSelectionStart, OnSelectionChange)),
                MessageAggregator.Subscribe(this),
                MessageAggregator.Subscribe(boldCommand)
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

        private Task DoMakeBoldAsync()
        {
            return editorInterop.Apply("strong").AsTask();
        }

        private async Task DoMakeItalicAsync()
        {
            Debug.WriteLine("EditorComponent.DoMakeItalic");
            await Task.CompletedTask;
        }

        private void OnSelectionStart(SelectionEventArgs e)
        {
            Debug.WriteLine("EditorComponent.OnSelectionStart");
        }

        private void OnSelectionChange(SelectionEventArgs e)
        {
            Debug.WriteLine("EditorComponent.OnSelectionChange");
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