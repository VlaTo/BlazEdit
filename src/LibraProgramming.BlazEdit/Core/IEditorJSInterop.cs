using System.Threading.Tasks;

namespace LibraProgramming.BlazEdit.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEditorJSInterop
    {
        ValueTask InitializeEditorAsync();

        ValueTask<string> GetContent();

        ValueTask SetContent(string content);

        ValueTask Apply(string htmlTag);
    }
}