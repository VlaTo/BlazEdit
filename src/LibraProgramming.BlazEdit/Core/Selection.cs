using System;
using System.Collections;
using System.Collections.Generic;
using LibraProgramming.BlazEdit.Core.Interop;

namespace LibraProgramming.BlazEdit.Core
{
    public class Selection : IEnumerable<SelectionRange>
    {
        public static readonly Selection Empty;

        private readonly SelectionRange[] ranges;

        public int Count => ranges.Length;

        public bool IsEmpty
        {
            get
            {
                if (ReferenceEquals(this, Empty))
                {
                    return true;
                }

                return 0 == ranges.Length;
            }
        }

        public SelectionRange this[int index] => ranges[index];

        public Selection(SelectionRange range)
            : this(null != range ? new[] {range} : null)
        {
        }

        public Selection(SelectionRange[] ranges)
        {
            this.ranges = ranges ?? Array.Empty<SelectionRange>();
        }

        static Selection()
        {
            Empty = new EmptySelection();
        }

        public string GetSelectionText()
        {
            return "";
        }

        public IEnumerator<SelectionRange> GetEnumerator()
        {
            for (var index = 0; index < ranges.Length; index++)
            {
                yield return ranges[index];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => ranges.GetEnumerator();

        /// <summary>
        /// 
        /// </summary>
        private class EmptySelection : Selection
        {
            public EmptySelection()
                : base((SelectionRange) null)
            {
            }
        }
    }
}