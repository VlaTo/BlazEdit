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
        bool IsApplied
        {
            get;
        }

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