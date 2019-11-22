using System;

namespace LibraProgramming.BlazEdit.Components
{
    public sealed class SelectionEventArgs : EventArgs
    {
        public string Text
        {
            get;
        }

        public SelectionEventArgs(string text)
        {
            Text = text;
        }
    }
}