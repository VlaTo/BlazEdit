using System.Threading.Tasks;

namespace LibraProgramming.BlazEdit.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEditorJSInterop
    {
        ValueTask InitializeEditorAsync();

        ValueTask SetContentStylesAsync(string styles);

        ValueTask<string> GetContentAsync();

        ValueTask SetContentAsync(string content);

        ValueTask FormatSelectionAsync(SelectionFormat htmlTag);
    }
}