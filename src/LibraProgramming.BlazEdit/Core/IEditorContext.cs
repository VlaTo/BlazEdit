using System.Threading.Tasks;

namespace LibraProgramming.BlazEdit.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEditorContext
    {
        ValueTask FormatSelectionAsync(SelectionFormat format);
    }
}