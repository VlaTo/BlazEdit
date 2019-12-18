using LibraProgramming.BlazEdit.Components;
using LibraProgramming.BlazEdit.TinyRx;
using Microsoft.JSInterop;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using LibraProgramming.BlazEdit.Core.Interop;
using Microsoft.AspNetCore.Components;

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
        public ValueTask OnSelectionStart(SelectionRange[] ranges)
        {
            var eventArgs = new SelectionEventArgs(SelectionChangeAction.SelectionStart, ranges);
            Raise(observer => observer.OnSelectionStart(eventArgs));
            return new ValueTask(Task.CompletedTask);
        }

        [JSInvokable]
        public ValueTask OnSelectionChange(SelectionRange[] ranges)
        {
            var eventArgs = new SelectionEventArgs(SelectionChangeAction.SelectionChanged, ranges);

            for (int index = 0; index < ranges.Length; index++)
            {
                Debug.WriteLine($"Range index: {index}");

                var range = ranges[index];
                var node = range.Start;

                while (null != node)
                {
                    Debug.WriteLine($"Start Node: {node.Name}");
                    node = node.NextNode;
                }

                node = range.End;

                while (null != node)
                {
                    Debug.WriteLine($"End Node: {node.Name}");
                    node = node.NextNode;
                }
            }

            Raise(observer => observer.OnSelectionChange(eventArgs));

            return new ValueTask(Task.CompletedTask);
        }
    }
}
