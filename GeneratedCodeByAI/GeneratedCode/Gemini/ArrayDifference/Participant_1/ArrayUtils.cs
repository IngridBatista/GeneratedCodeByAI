using System;
using System.Linq;

namespace GEMINI.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_1
{
    public class ArrayUtils
    {
        /// <summary>
        /// Retorna os elementos do primeiro array que não existem no segundo.
        /// </summary>
        /// <param name="primeiroArray">O array principal do qual os elementos serão retornados.</param>
        /// <param name="segundoArray">O array contendo os elementos a serem excluídos.</param>
        /// <returns>Um novo array com os elementos exclusivos do primeiro array.</returns>
        public static int[] ElementosExclusivos(int[] primeiroArray, int[] segundoArray)
        {
            // O método Except() retorna todos os elementos do primeiroArray
            // que não estão presentes no segundoArray.
            // O ToArray() converte o resultado de volta para um array.
            return primeiroArray.Except(segundoArray).ToArray();
        }

        // --- Exemplo de Uso ---
        public static void Main(string[] args)
        {
            int[] array1 = { 1, 2, 3, 4, 5, 6 };
            int[] array2 = { 2, 4, 6, 8, 10 };

            int[] resultado = ElementosExclusivos(array1, array2);

            // O resultado esperado é { 1, 3, 5 }
            Console.WriteLine("Elementos do primeiro array que não estão no segundo:");
            Console.WriteLine(string.Join(", ", resultado)); // Saída: 1, 3, 5
        }
    }
}