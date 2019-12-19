using System;

namespace LibraProgramming.BlazEdit.Commands
{
    public interface IObservableToolCommand : IToolCommand, IObservable<IToolCommand>
    {
    }
}