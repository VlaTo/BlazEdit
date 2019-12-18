using System.Threading.Tasks;

namespace LibraProgramming.BlazEdit.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMessageHandler
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IMessageHandler<in TMessage> : IMessageHandler
        where TMessage : IMessage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        Task HandleAsync(TMessage message);
    }
}