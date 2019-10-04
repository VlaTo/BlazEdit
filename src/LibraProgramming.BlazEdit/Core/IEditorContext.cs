using LibraProgramming.BlazEdit.Commands;

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
        IToolCommand MakeBold { get; }

        /// <summary>
        /// 
        /// </summary>
        IToolCommand MakeItalic { get; }
    }
}