using System;
using System.Threading.Tasks;
using LibraProgramming.BlazEdit.Core;

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
            return base.CanInvoke();
        }

        public override Task InvokeAsync()
        {
            ;
        }
    }
}