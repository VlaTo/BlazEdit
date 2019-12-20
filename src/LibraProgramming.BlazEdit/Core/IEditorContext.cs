using System.Threading.Tasks;

namespace LibraProgramming.BlazEdit.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEditorContext
    {
        /// <summary>
        /// 
        /// </summary>
        Selection Selection
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        ValueTask FormatSelectionAsync(SelectionFormat format);
    }
}