using LibraProgramming.BlazEdit.Components;
using LibraProgramming.BlazEdit.TinyRx;
using Microsoft.JSInterop;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using LibraProgramming.BlazEdit.Core.Interop;

namespace LibraProgramming.BlazEdit.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class EditorJsInterop : ObservableBase<ISelectionObserver>, IEditorJSInterop
    {
        private readonly IJSRuntime jsRuntime;
        private readonly ElementReference element;

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
            this.element = element;
        }

        public Task InitializeEditorAsync()
        {
            Console.WriteLine("EditorJsInterop.InitializeEditorAsync");
            var callback = DotNetObjectReference.Create(this);
            return jsRuntime.InvokeVoidAsync("editor", elementId, callback).AsTask();
        }

        public Task<string> GetContentAsync() => jsRuntime.InvokeAsync<string>("editor.getContent").AsTask();

        public Task SetContentAsync(string content) => jsRuntime.InvokeVoidAsync("editor.setContent", content).AsTask();

        public Task FormatSelectionAsync(SelectionFormat format)
        {

            return jsRuntime.InvokeVoidAsync("editor.formatSelection", format).AsTask();
        }

        [JSInvokable]
        public void OnSelectionStart(SelectionRange[] ranges)
        {
            var eventArgs = new SelectionEventArgs(ranges);
            Raise(observer => observer.OnSelectionStart(eventArgs));
        }

        [JSInvokable]
        public void OnSelectionChange(SelectionRange[] ranges)
        {
            var eventArgs = new SelectionEventArgs(Array.Empty<SelectionRange>());

            for (int index = 0; index < ranges.Length; index++)
            {
                Debug.WriteLine($"Range index: {index}");

                var range = ranges[index];
                var node = range.StartNode;

                while (null != node)
                {
                    Debug.WriteLine($"Node: {node.Name}");
                    node = node.Next;
                }
            }

            Raise(observer => observer.OnSelectionChange(eventArgs));
        }
    }
}
