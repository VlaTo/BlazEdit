using LibraProgramming.BlazEdit.Components;
using LibraProgramming.BlazEdit.Core.Interop;
using LibraProgramming.BlazEdit.TinyRx;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

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

        public async ValueTask InitializeEditorAsync()
        {
            var callback = DotNetObjectReference.Create(this);
            await jsRuntime.InvokeVoidAsync("editor", elementId, callback);
        }

        public ValueTask<string> GetContentAsync() => jsRuntime.InvokeAsync<string>("editor.getContent");

        public ValueTask SetContentAsync(string content) => jsRuntime.InvokeVoidAsync("editor.setContent", content);

        public ValueTask FormatSelectionAsync(SelectionFormat format) => jsRuntime.InvokeVoidAsync("editor.formatSelection", format);

        [JSInvokable]
        public ValueTask OnSelectionChange(SelectionChangeAction action, SelectionRange[] ranges)
        {
            var eventArgs = new SelectionEventArgs(action, ranges);
            Raise(observer => observer.OnSelectionChange(eventArgs));
            return new ValueTask(Task.CompletedTask);
        }
    }
}
