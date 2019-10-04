using LibraProgramming.BlazEdit.Commands;
using LibraProgramming.BlazEdit.Core;

namespace LibraProgramming.BlazEdit.Events
{
    public sealed class CommandMessage : IMessage
    {
        public IToolCommand Command
        {
            get;
        }

        public CommandMessage(IToolCommand command)
        {
            Command = command;
        }
    }
}