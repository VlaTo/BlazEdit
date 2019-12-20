using LibraProgramming.BlazEdit.Commands;
using LibraProgramming.BlazEdit.Core;
using LibraProgramming.BlazEdit.Events;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace LibraProgramming.BlazEdit.Components
{
    public class EditorComponent : ComponentBase, IEditorContext, IDisposable
    {
        private readonly string generatedElementId;
        private FormatCommand boldCommand;
        private FormatCommand italicCommand;
        private IEditorJSInterop editor;
        private ITimeout timeout;
        private bool hasRendered;
        private readonly CompositeDisposable subscriptions;
        private bool initialized;
        private Selection selection;
        private int id;

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

        public Selection Selection
        {
            get => selection;
            private set
            {
                selection = value;
                MessageDispatcher.Publish(new SelectionChangedMessage());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected FormatCommand BoldCommand
        {
            get
            {
                if (null == boldCommand)
                {
                    boldCommand = new FormatCommand(this, "STRONG");
                    subscriptions.Add(MessageDispatcher.Subscribe(boldCommand));
                }

                return boldCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected FormatCommand ItalicCommand
        {
            get
            {
                if (null == italicCommand)
                {
                    italicCommand = new FormatCommand(this, "EM");
                    subscriptions.Add(MessageDispatcher.Subscribe(italicCommand));
                }

                return italicCommand;
            }
        }

        protected object[] Paragraphs
        {
            get;
        }

        protected string EditorElementId => generatedElementId;

        protected EditorComponent()
        {
            generatedElementId = $"__editor_area_{IdManager<EditorComponent>.GetId(ref id)}";
            subscriptions = new CompositeDisposable(4);
            selection = Selection.Empty;

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
            subscriptions.Dispose();
        }

        ValueTask IEditorContext.FormatSelectionAsync(SelectionFormat format) => editor.FormatSelectionAsync(format);

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var editorInterop = new EditorJsInterop(JsRuntime, EditorElementId);

            subscriptions.Add(editorInterop.Subscribe(value => Selection = value));

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