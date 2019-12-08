using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LibraProgramming.BlazEdit.Core
{
    public class EditorJsInterop : IEditorJSInterop
    {
        private readonly IJSRuntime jsRuntime;
        private readonly ElementReference element;

        public EditorJsInterop(IJSRuntime jsRuntime, ElementReference element)
        {
            if (null == jsRuntime)
            {
                throw new ArgumentNullException(nameof(jsRuntime));
            }

            this.jsRuntime = jsRuntime;
            this.element = element;
        }

        public ValueTask InitializeEditorAsync()
        {
            Console.WriteLine("EditorJsInterop.InitializeEditorAsync");
            return jsRuntime.InvokeVoidAsync("editor", element);
        }

        public ValueTask<string> GetContent()
        {
            return jsRuntime.InvokeAsync<string>("editor.getContent");
        }

        public ValueTask SetContent(string content)
        {
            Debug.WriteLine($"[EditorJsInterop.SetContent]");
            return jsRuntime.InvokeVoidAsync("editor.setContent", content);
        }

        public ValueTask Apply(string htmlTag)
        {
            return jsRuntime.InvokeVoidAsync("editor.apply", htmlTag);
        }
    }
}
