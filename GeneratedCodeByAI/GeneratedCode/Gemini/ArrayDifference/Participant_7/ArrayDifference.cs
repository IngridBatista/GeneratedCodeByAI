using System;
using System.Linq;
using System.Collections.Generic;

namespace GEMINI.ARRAY_DIFFERENCE.JUNIOR.PARTICIPANT_7
{
    public class ArrayDifference
    {
        /// <summary>
        /// Retorna um array de inteiros contendo todos os elementos do array 'a' 
        /// que não aparecem no array 'b'.
        /// </summary>
        /// <param name="a">O array principal do qual os elementos serão retornados.</param>
        /// <param name="b">O array cujos elementos serão removidos de 'a'.</param>
        /// <returns>Um novo array com a diferença dos elementos.</returns>
        public static int[] Difference(int[] a, int[] b)
        {
            // O método Except() retorna todos os elementos da primeira coleção (a)
            // que não aparecem na segunda coleção (b).
            // Internamente, ele é otimizado e utiliza um HashSet para garantir 
            // uma performance excelente (O(n + m)).
            // ToArray() converte o resultado (IEnumerable<int>) de volta para um array.
            return a.Except(b).ToArray();
        }

        // Exemplo de uso:
        public static void Main(string[] args)
        {
            int[] arrayA = { 1, 2, 3, 4, 5, 6 };
            int[] arrayB = { 4, 5, 6, 7, 8 };

            int[] result = Difference(arrayA, arrayB);

            // A saída esperada é: [1, 2, 3]
            Console.WriteLine("Resultado: [" + string.Join(", ", result) + "]");

            int[] arrayC = { 1, 2, 2, 3, 1 };
            int[] arrayD = { 2 };

            int[] result2 = Difference(arrayC, arrayD);

            // A saída esperada é: [1, 3]
            // Note que o Except() também remove duplicatas do resultado, 
            // pois opera com a lógica de conjuntos.
            Console.WriteLine("Resultado 2: [" + string.Join(", ", result2) + "]");
        }
    }
}