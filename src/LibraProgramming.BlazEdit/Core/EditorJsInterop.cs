using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using LibraProgramming.BlazEdit.Components;
using LibraProgramming.BlazEdit.Extensions;
using LibraProgramming.BlazEdit.TinyRx;
using Microsoft.JSInterop;

namespace LibraProgramming.BlazEdit.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class EditorJsInterop : ObservableBase<ISelectionObserver>, IEditorJSInterop
    {
        private readonly IJSRuntime jsRuntime;
        private readonly string elementId;

/*
        public event EventHandler<SelectionStartEventArgs> SelectionStart
        {
            add
            {
                if (null == selectionStartHandler)
                {
                    var instance = DotNetObjectReference.Create(this);
                    Debug.WriteLine("EditorJsInterop.SelectionStart subscribe listener");
                    jsRuntime.InvokeVoidAsync("editor.addSelectionStartListener", instance).RunAndForget();
                }

                Debug.WriteLine("EditorJsInterop.SelectionStart add event handler");

                selectionStartHandler += value;
            }
            remove
            {
                selectionStartHandler -= value;

                if (null == selectionStartHandler)
                {
                    jsRuntime.InvokeVoidAsync("editor.removeSelectionStartListener").RunAndForget();
                }
            }
        }
*/

        public EditorJsInterop(IJSRuntime jsRuntime, string elementId)
        {
            if (null == jsRuntime)
            {
                throw new ArgumentNullException(nameof(jsRuntime));
            }

            this.jsRuntime = jsRuntime;
            this.elementId = elementId;
        }

        public ValueTask InitializeEditorAsync()
        {
            Console.WriteLine("EditorJsInterop.InitializeEditorAsync");
            var callback = DotNetObjectReference.Create(this);
            return jsRuntime.InvokeVoidAsync("editor", elementId, callback);
        }

        public ValueTask<string> GetContent()
        {
            return jsRuntime.InvokeAsync<string>("editor.content");
        }

        public ValueTask SetContent(string content)
        {
            Debug.WriteLine($"[EditorJsInterop.SetContent]");
            return jsRuntime.InvokeVoidAsync("editor.content", content);
        }

        public ValueTask FormatSelection(string htmlTag)
        {
            var format = new SelectionFormat("strong");
            return jsRuntime.InvokeVoidAsync("editor.formatSelection", format);
        }

        [JSInvokable]
        public void OnSelectionStart(JsonElement e)
        {
            var eventArgs = new SelectionEventArgs(String.Empty);
            Raise(observer => observer.OnSelectionStart(eventArgs));
        }

        [JSInvokable]
        public void OnSelectionChange(string text)
        {
            var eventArgs = new SelectionEventArgs(text);
            Raise(observer => observer.OnSelectionChange(eventArgs));
        }

        internal sealed class SelectionFormat
        {
            public string elementName
            {
                get;
            }

            public SelectionFormat(string elementName)
            {
                this.elementName = elementName;
            }
        }
    }
}
