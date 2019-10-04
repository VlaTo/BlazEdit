using LibraProgramming.BlazEdit.Components;
using LibraProgramming.BlazEdit.Core;

namespace LibraProgramming.BlazEdit.Events
{
    internal sealed class ToggleButtonMessage : IMessage
    {
        public ToggleButton Button
        {
            get;
        }

        public ToggleButtonMessage(ToggleButton button)
        {
            Button = button;
        }
    }
}