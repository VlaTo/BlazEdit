using System;
using System.Diagnostics;
using System.Threading.Tasks;
using LibraProgramming.BlazEdit.Core;
using LibraProgramming.BlazEdit.Events;

namespace LibraProgramming.BlazEdit.Commands
{
    internal sealed class BoldToolCommand : FormatToolCommand
    {
        public BoldToolCommand(IEditorContext editorContext)
            : base(editorContext)
        {
        }

        public override bool CanInvoke()
        {
            return true;
        }

        public override Task InvokeAsync()
        {
            var format = new SelectionFormat("strong");
            return EditorContext.FormatSelectionAsync(format);
        }

        protected override Task DoSelectionChangedAsync(Selection selection)
        {
            Debug.WriteLine($"[{nameof(BoldToolCommand)}.DoSelectionChangedAsync] Selection: {selection.GetSelectionText()}");
            return base.DoSelectionChangedAsync(selection);
        }
    }
}