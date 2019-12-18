using System.Threading.Tasks;
using LibraProgramming.BlazEdit.Core;
using LibraProgramming.BlazEdit.TinyRx;

namespace LibraProgramming.BlazEdit.Components
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISelectionObserver : IObserver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        ValueTask OnSelectionStart(SelectionEventArgs e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        ValueTask OnSelectionChange(SelectionEventArgs e);
    }
}