using LibraProgramming.BlazEdit.Commands;

namespace LibraProgramming.BlazEdit.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEditorContext
    {
        bool CanInvokeCommand(IToolCommand command);
    }
}