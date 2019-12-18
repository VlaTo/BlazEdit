namespace LibraProgramming.BlazEdit
{
    public sealed class StyleDefinition
    {
        public string Title
        {
            get;
        }

        public string StyleKey
        {
            get;
        }

        public StyleDefinition(string styleKey, string title = null)
        {
            StyleKey = styleKey;
            Title = title ?? styleKey;
        }
    }
}