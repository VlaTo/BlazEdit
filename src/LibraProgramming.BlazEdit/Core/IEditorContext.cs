using System.Threading.Tasks;
using LibraProgramming.BlazEdit.Commands;

namespace LibraProgramming.BlazEdit.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEditorContext
    {
        //bool CanInvokeCommand(IToolCommand command);

        Task FormatSelectionAsync();
    }
}