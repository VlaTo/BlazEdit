using LibraProgramming.BlazEdit.Core;

namespace LibraProgramming.BlazEdit.Events
{
    public class SelectionChangedMessage : IMessage
    {
        public string Text
        {
            get;
        }

        public SelectionChangedMessage(string text)
        {
            Text = text;
        }
    }
}