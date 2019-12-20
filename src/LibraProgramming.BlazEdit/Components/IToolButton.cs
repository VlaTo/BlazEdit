using LibraProgramming.BlazEdit.Commands;

namespace LibraProgramming.BlazEdit.Components
{
    internal interface IToolButton
    {
        bool Enabled
        {
            get; 
            set;
        }

        IToolCommand Command
        {
            get;
            set;
        }

        string Tooltip
        {
            get;
            set;
        }
    }
}