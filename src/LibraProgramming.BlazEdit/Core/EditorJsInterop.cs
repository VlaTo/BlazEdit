using LibraProgramming.BlazEdit.Core.Interop;
using Microsoft.JSInterop;
using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace LibraProgramming.BlazEdit.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class EditorJsInterop : IObservable<Selection>, IEditorJSInterop
    {
        private readonly IJSRuntime jsRuntime;
        private readonly string elementId;
        private readonly Subject<Selection> subject;

        public EditorJsInterop(IJSRuntime jsRuntime, string elementId)
        {
            if (null == jsRuntime)
            {
                throw new ArgumentNullException(nameof(jsRuntime));
            }

            this.jsRuntime = jsRuntime;
            this.elementId = elementId;

            subject = new Subject<Selection>();
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
            subject.OnNext(new Selection(ranges));
            return new ValueTask(Task.CompletedTask);
        }

        public IDisposable Subscribe(IObserver<Selection> observer) => subject.Subscribe(observer);
    }
}
