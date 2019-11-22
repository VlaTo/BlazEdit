using LibraProgramming.BlazEdit.Core;

namespace LibraProgramming.BlazEdit.Events
{
    public class SelectionChangedMessage : IMessage
    {
        public Selection Selection
        {
            get;
        }

        public SelectionChangedMessage(Selection selection)
        {
            Selection = selection;
        }
    }
}