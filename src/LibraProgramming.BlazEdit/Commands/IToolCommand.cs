using System.Threading.Tasks;

namespace LibraProgramming.BlazEdit.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public interface IToolCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool CanInvoke();

        /// <summary>
        /// 
        /// </summary>
        Task InvokeAsync();
    }
}