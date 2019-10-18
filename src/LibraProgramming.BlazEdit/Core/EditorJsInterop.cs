using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace LibraProgramming.BlazEdit.Core
{
    public class EditorJsInterop : IEditorJSInterop
    {
        private readonly IJSRuntime jsRuntime;
        private readonly string elementId;

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
            return jsRuntime.InvokeVoidAsync("editor", elementId, this);
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

        public ValueTask Apply(string htmlTag)
        {
            return jsRuntime.InvokeVoidAsync("editor.apply", htmlTag);
        }

        [JSInvokable]
        public void OnSelectionStart()
        {
            Debug.WriteLine($"[EditorJsInterop.OnSelectionStart]");
        }
    }
}
