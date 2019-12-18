using System.Diagnostics;
using System.Threading.Tasks;
using LibraProgramming.BlazEdit.Core;

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
            return EditorContext.FormatSelectionAsync(format);
        }

        protected override Task DoSelectionChangedAsync(Selection selection)
        {
            Debug.WriteLine($"[{nameof(ItalicToolCommand)}.DoSelectionChangedAsync] Selection: {selection.GetSelectionText()}");
            return base.DoSelectionChangedAsync(selection);
        }
    }
}