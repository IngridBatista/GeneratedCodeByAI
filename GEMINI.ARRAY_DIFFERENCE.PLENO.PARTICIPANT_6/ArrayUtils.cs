using System;
using System.Linq; // Necessário para usar os métodos LINQ

namespace GEMINI.ARRAY_DIFFERENCE.PLENO.PARTICIPANT_6
{
    public class ArrayUtils
    {
        /// <summary>
        /// Retorna um array com os elementos de 'a' que não estão em 'b'.
        /// </summary>
        /// <param name="a">O array principal.</param>
        /// <param name="b">O array cujos elementos serão excluídos de 'a'.</param>
        /// <returns>Um novo array contendo a diferença.</returns>
        public static int[] ElementosUnicosEmA(int[] a, int[] b)
        {
            // Except() retorna todos os elementos no primeiro array (a)
            // que não aparecem no segundo array (b).
            // ToArray() converte o resultado de volta para um array.
            return a.Except(b).ToArray();
        }

        // --- Exemplo de Uso ---
        public static void Main(string[] args)
        {
            int[] arrayA = { 1, 2, 3, 4, 5, 8 };
            int[] arrayB = { 3, 5, 6, 7, 8 };

            int[] resultado = ElementosUnicosEmA(arrayA, arrayB);

            // O resultado esperado é { 1, 2, 4 }
            Console.WriteLine("Elementos de A que não estão em B: ");
            Console.WriteLine(string.Join(", ", resultado)); // Saída: 1, 2, 4
        }
    }
}