using System;
using System.Linq;

namespace GEMINI.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_4
{
    public class ArrayOperations
    {
        /// <summary>
        /// Retorna um array contendo os elementos do array 'a' que não existem no array 'b'.
        /// A implementação utiliza o método LINQ 'Except' para calcular a diferença de conjuntos.
        /// </summary>
        /// <param name="a">O primeiro array de inteiros.</param>
        /// <param name="b">O segundo array de inteiros, cujos elementos serão removidos de 'a'.</param>
        /// <returns>Um novo array de inteiros com o resultado da diferença.</returns>
        public static int[] Difference(int[] a, int[] b)
        {
            // Enumerable.Except retorna os elementos na primeira coleção
            // que não aparecem na segunda coleção.
            // O método retorna um IEnumerable<int>, então .ToArray() é chamado
            // para converter o resultado para o tipo de retorno int[].
            return a.Except(b).ToArray();
        }
    }
}