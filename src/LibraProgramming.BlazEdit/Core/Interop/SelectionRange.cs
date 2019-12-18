namespace LibraProgramming.BlazEdit.Core.Interop
{
    public class SelectionNode
    {
        public string Name
        {
            get;
            set;
        }

        public SelectionNode NextNode
        {
            get;
            set;
        }
    }

    public class SelectionRange
    {
        public SelectionNode Start
        {
            get;
            set;
        }

        public SelectionNode End
        {
            get;
            set;
        }

        public int StartOffset
        {
            get;
            set;
        }

        public int EndOffset
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }
    }
}