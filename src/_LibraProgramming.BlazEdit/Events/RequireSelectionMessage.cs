using System.Collections.Generic;
using LibraProgramming.BlazEdit.Core;

namespace LibraProgramming.BlazEdit.Events
{
    public class RequireSelectionMessage : IMessage
    {
        public KeyValuePair<string, object>[] Items
        {
            get;
        }

        public RequireSelectionMessage(KeyValuePair<string, object>[] items)
        {
            Items = items;
        }
    }
}