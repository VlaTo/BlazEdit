using System;
using LibraProgramming.BlazEdit.Core.Interop;

namespace LibraProgramming.BlazEdit.Core.Extensions
{
    internal static class SelectionExtensions
    {
        private static readonly StringComparer comparer = StringComparer.InvariantCultureIgnoreCase;

        public static bool HasStrong(this Selection selection)
        {
            if (null == selection)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            return null != FindNodeInternal(selection, "STRONG");
        }

        public static SelectionNode FindNode(this Selection selection, string name)
        {
            if (null == selection)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            return FindNodeInternal(selection, name);
        }

        private static SelectionNode FindNodeInternal(Selection selection, string name)
        {
            for (var index = 0; index < selection.Count; index++)
            {
                var range = selection[index];

                for (var current = range.Start; null != current; current = current.NextNode)
                {
                    if (comparer.Equals(current.Name, name))
                    {
                        return current;
                    }
                }
            }

            return null;
        }
    }
}