using System.Threading;

namespace LibraProgramming.BlazEdit.Core
{
    // ReSharper disable once UnusedTypeParameter
    internal static class IdManager<TTag>
    {
        private static int lastId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int GetId(ref int id)
        {
            if (0 != id)
            {
                return id;
            }

            int candidate;

            do
            {
                candidate = Interlocked.Increment(ref lastId);

            } while (0 == candidate);

            Interlocked.CompareExchange(ref id, candidate, 0);

            return id;
        }
    }
}