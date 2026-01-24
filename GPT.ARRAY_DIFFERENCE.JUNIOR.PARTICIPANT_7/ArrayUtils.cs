using System;
using System.Collections.Generic;

namespace GPT.ARRAY_DIFFERENCE.JUNIOR.PARTICIPANT_7
{
    public static class ArrayUtils
    {
        public static int[] Difference(int[] a, int[] b)
        {
            // Usar HashSet para busca O(1) dos elementos de b
            var setB = new HashSet<int>(b);
            var result = new List<int>(a.Length);

            foreach (var item in a)
            {
                if (!setB.Contains(item))
                {
                    result.Add(item);
                }
            }

            return result.ToArray();
        }
    }
}