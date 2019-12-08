using System;

namespace LibraProgramming.BlazEdit.Core
{
    public class Selection
    {
        private readonly string text;

        public static readonly Selection Empty;

        public Selection(string text)
            : this(text, true)
        {
        }

        private Selection(string text, bool forceCheck)
        {
            if (forceCheck)
            {
                if (null == text)
                {
                    throw new ArgumentNullException(nameof(text));
                }
            }

            this.text = text;
        }

        static Selection()
        {
            Empty = new EmptySelection();
        }

        public string GetSelectionText()
        {
            return text;
        }

        private class EmptySelection : Selection
        {
            public EmptySelection()
                : base(null, false)
            {
            }
        }
    }
}