using LibraProgramming.BlazEdit.Core;
using LibraProgramming.BlazEdit.Core.Extensions;
using System.Threading.Tasks;

namespace LibraProgramming.BlazEdit.Commands
{
    internal sealed class ItalicToolCommand : FormatToolCommand
    {
        public ItalicToolCommand(IEditorContext editorContext)
            : base(editorContext)
        {
        }

        public override bool CanInvoke()
        {
            return true;
        }

        public override Task InvokeAsync()
        {
            var format = new SelectionFormat("italic");
            return EditorContext.FormatSelectionAsync(format).AsTask();
        }

        protected override Task DoSelectionChangedAsync(Selection selection)
        {
            IsApplied = selection.HasNode("#italic");
            return Task.CompletedTask;
        }
    }
}