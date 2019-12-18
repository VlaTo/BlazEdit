using System;
using LibraProgramming.BlazEdit.Core.Interop;

namespace LibraProgramming.BlazEdit.Core
{
    public enum SelectionChangeAction
    {
        SelectionStart,
        SelectionChanged
    }

    public sealed class SelectionEventArgs : EventArgs
    {
        public SelectionChangeAction Action
        {
            get;
        }

        public SelectionRange[] Ranges
        {
            get;
        }

        public SelectionEventArgs(SelectionChangeAction action, SelectionRange[] ranges)
        {
            Action = action;
            Ranges = ranges;
        }
    }
}