using System.Threading.Tasks;

namespace LibraProgramming.BlazEdit.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEditorJSInterop
    {
        Task InitializeEditorAsync();

        Task<string> GetContentAsync();

        Task SetContentAsync(string content);

        Task FormatSelectionAsync(SelectionFormat htmlTag);
    }
}