using System;

namespace LibraProgramming.BlazEdit.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMessageDispatcher
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void Publish(IMessage message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        IDisposable Subscribe(IMessageHandler handler);
    }
}