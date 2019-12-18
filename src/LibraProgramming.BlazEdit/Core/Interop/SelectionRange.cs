namespace LibraProgramming.BlazEdit.Core.Interop
{
    public class SelectionRange
    {
        public Core.SelectionNode StartNode
        {
            get;
            set;
        }

        public Core.SelectionNode EndNode
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

        public string Content
        {
            get;
            set;
        }
    }
}