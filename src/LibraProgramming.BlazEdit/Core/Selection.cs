using System;
using LibraProgramming.BlazEdit.Core.Interop;

namespace LibraProgramming.BlazEdit.Core
{
    public class Selection
    {
        private static readonly StringComparer comparer;

        public static readonly Selection Empty;
        
        private readonly SelectionChangeAction action;
        private readonly SelectionRange[] ranges;

        public Selection(SelectionChangeAction action, SelectionRange range)
        {
            this.action = action;
            ranges = null != range ? new[] {range} : Array.Empty<SelectionRange>();
        }

        private Selection(SelectionChangeAction action, SelectionRange[] ranges)
        {
            this.action = action;
            this.ranges = ranges ?? Array.Empty<SelectionRange>();
        }

        static Selection()
        {
            comparer = StringComparer.InvariantCultureIgnoreCase;
            Empty = new EmptySelection();
        }

        public bool HasNode(string nodeName)
        {
            for (var index = 0; index < ranges.Length; index++)
            {
                if (RangeHasNode(ranges[index], nodeName))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool RangeHasNode(SelectionRange range, string nodeName)
        {
            for (var current = range.Start; null != current; current = current.NextNode)
            {
                if (comparer.Equals(current.Name, nodeName))
                {
                    return true;
                }
            }

            return false;
        }

        public static Selection From(SelectionEventArgs e)
        {
            return new Selection(e.Action, e.Ranges);
        }

        public string GetSelectionText()
        {
            return "";
        }

        private class EmptySelection : Selection
        {
            public EmptySelection()
                : base(SelectionChangeAction.SelectionStart, (SelectionRange) null)
            {
            }
        }
    }
}