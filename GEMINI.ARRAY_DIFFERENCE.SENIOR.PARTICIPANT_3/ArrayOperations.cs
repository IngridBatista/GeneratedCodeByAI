using System.Collections.Generic;
using System.Linq;

namespace GEMINI.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_3
{
    public class ArrayOperations
    {
        /// <summary>
        /// Compara dois arrays e retorna os elementos do primeiro array (a)
        /// que não estão contidos no segundo array (b), mantendo a ordem original.
        /// </summary>
        /// <param name="a">O array principal do qual os elementos serão retornados.</param>
        /// <param name="b">O array usado para verificar a existência dos elementos.</param>
        /// <returns>Um novo array contendo os elementos de 'a' não presentes em 'b'.</returns>
        public static int[] Difference(int[] a, int[] b)
        {
            // Solução 1: Usando LINQ (mais concisa e idiomática)
            return a.Except(b).ToArray();
        }

        // Solução 2: Implementação manual (mais explícita e didática)
        public static int[] Difference_Manual(int[] a, int[] b)
        {
            HashSet<int> elementsInB = new HashSet<int>(b);
            List<int> result = new List<int>();

            foreach (int element in a)
            {
                if (!elementsInB.Contains(element))
                {
                    result.Add(element);
                }
            }

            return result.ToArray();
        }
    }
}