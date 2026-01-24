using System;
using System.Linq;
using System.Collections.Generic;

namespace GEMINI.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_5
{
    public class ArrayUtils
    {
        /// <summary>
        /// Retorna um novo array contendo os elementos do array 'a' que não existem no array 'b'.
        /// </summary>
        /// <param name="a">O array de origem do qual os elementos serão retornados.</param>
        /// <param name="b">O array cujos elementos serão removidos do resultado.</param>
        /// <returns>Um array de inteiros com a diferença entre 'a' e 'b'.</returns>
        public static int[] Difference(int[] a, int[] b)
        {
            // O método Except() é altamente otimizado para esta operação.
            // Ele retorna todos os elementos na primeira coleção (a)
            // que não aparecem na segunda coleção (b).
            // ToArray() materializa o resultado em um novo array.
            return a.Except(b).ToArray();
        }
    }
}