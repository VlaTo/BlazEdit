using System;
using LibraProgramming.BlazEdit.Core;
using System.Threading.Tasks;

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
            return EditorContext.FormatSelectionAsync(format).AsTask();
        }

        protected override Task DoSelectionChangedAsync(Selection selection)
        {
            IsApplied = selection.HasNode("strong");
            Console.WriteLine($"Has strong: {IsApplied}");
            return Task.CompletedTask;
        }
    }
}