using System;
using LibraProgramming.BlazEdit.Core.Interop;

namespace LibraProgramming.BlazEdit.Core
{
    public sealed class SelectionEventArgs : EventArgs
    {
        public SelectionRange[] Ranges
        {
            get;
        }

        public SelectionEventArgs(SelectionRange[] ranges)
        {
            Ranges = ranges;
        }
    }
}