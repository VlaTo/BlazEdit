namespace LibraProgramming.BlazEdit.Core.Interop
{
    public class SelectionChangeCallbackArgs
    {
        public SelectionNode startNode
        {
            get; 
            set;
        }

        public SelectionNode endNode
        {
            get; 
            set;
        }

        public string text
        {
            get; 
            set;
        }
    }
}