using System;

namespace LibraProgramming.BlazEdit.TinyRx
{
    /// <summary>
    /// 
    /// </summary>
    internal class Stubs
    {
        /// <summary>
        /// 
        /// </summary>
        public static Action<Exception> Throw = exception => throw exception;

        /// <summary>
        /// 
        /// </summary>
        public static Action Nop = () => { };
    }
}