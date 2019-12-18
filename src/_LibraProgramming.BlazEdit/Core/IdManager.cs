using System;
using System.Collections.Generic;

namespace LibraProgramming.BlazEdit.Core
{
    internal class IdManager
    {
        private readonly Dictionary<string, int> counters;

        public static readonly IdManager Instance;

        private IdManager()
        {
            counters = new Dictionary<string, int>();
        }

        static IdManager()
        {
            Instance = new IdManager();
        }

        public string Generate(string prefix)
        {
            if (null == prefix)
            {
                throw new ArgumentNullException(nameof(prefix));
            }

            if (false == counters.TryGetValue(prefix, out var counter))
            {
                counter = 0;
            }
            else
            {
                counter++;
            }

            counters[prefix] = counter;

            return $"__{prefix}_{counter}";
        }
    }
}