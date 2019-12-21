using LibraProgramming.BlazEdit.Commands;
using LibraProgramming.BlazEdit.Core;
using LibraProgramming.BlazEdit.Events;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;
using LibraProgramming.BlazEdit.Extensions;

namespace LibraProgramming.BlazEdit.Components
{
    public class EditorComponent : ComponentBase, IEditorContext, IDisposable
    {
        private readonly string generatedElementId;
        private FormatCommand boldCommand;
        private FormatCommand italicCommand;
        private FormatCommand strikeThroughCommand;
        private IEditorJSInterop editor;
        private ITimeout timeout;
        private bool hasRendered;
        private readonly CompositeDisposable subscriptions;
        private int initialized;
        private Selection selection;
        private int id;
        private string text;

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
            get => text;
            set
            {
                if (String.Equals(text, value))
                {
                    return;
                }

                text = value;

                TextChanged.InvokeAsync(value).RunAndForget();
            }
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

        /// <summary>
        /// 
        /// </summary>
        protected FormatCommand StrikeThroughCommand
        {
            get
            {
                if (null == strikeThroughCommand)
                {
                    strikeThroughCommand = new FormatCommand(this, "STRIKE");
                    subscriptions.Add(MessageDispatcher.Subscribe(strikeThroughCommand));
                }

                return strikeThroughCommand;
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
            if (0 == Interlocked.CompareExchange(ref initialized, 1, 0))
            {
                var styles = "@font-face{" +
                             "font-family:\"avenir\"; " +
                             "src: url(\"/static/AvenirHeavy-9de46e344e47c7432887c85c9583aafe.woff\") format(\"woff\"), " +
                             "url(\"/static/AvenirHeavy-289fbfeed5013eb4bb1638deea01cc65.woff2\") format(\"woff2\"); " +
                             "font-style:normal; " +
                             "font-weight:600; " +
                             "font-display:swap; " +
                             "} " +
                             "#blazedit.editor-content-body{font-family:avenir,arial,helvetica,sans-serif!important; color:#626262;}"
                    ;
                await editor.InitializeEditorAsync();
                await editor.SetContentStylesAsync(styles);
            }
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