using System;

namespace LibraProgramming.BlazEdit.TinyRx
{
    public interface IError
    {
        void OnError(Exception exception);
    }
}