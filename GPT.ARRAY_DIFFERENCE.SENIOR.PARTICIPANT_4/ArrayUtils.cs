using System;
using System.Linq;

namespace GPT.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_4
{
    public static class ArrayUtils
    {
        public static int[] Difference(int[] a, int[] b)
        {
            // Pré-calcula o conjunto de elementos de b para busca O(1) em média
            var setB = b.ToHashSet();

            // Filtra apenas os elementos de a que não estão em b
            return a.Where(x => !setB.Contains(x)).ToArray();
        }
    }
}