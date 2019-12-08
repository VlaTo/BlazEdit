namespace LibraProgramming.BlazEdit.Core
{
    public sealed class SelectionFormat
    {
        public string elementName
        {
            get;
        }

        public SelectionFormat(string elementName)
        {
            this.elementName = elementName;
        }
    }
}