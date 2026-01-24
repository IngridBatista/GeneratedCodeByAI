using System;
using System.Collections.Generic;
using System.Linq;

namespace GEMINI.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_2
{
    public class Program
    {
        public static void Main()
        {
            // Arrays de exemplo
            int[] A = { 1, 2, 3, 4, 5, 10, 12 };
            int[] B = { 4, 5, 6, 7, 8, 10 };

            Console.WriteLine("Array A: " + string.Join(", ", A));
            Console.WriteLine("Array B: " + string.Join(", ", B));
            Console.WriteLine("--------------------------------------------");

            // --- Solução 1: Usando LINQ ---
            int[] resultadoLinq = EncontrarDiferencaComLinq(A, B);
            Console.WriteLine("Resultado com LINQ (elementos em A que não estão em B):");
            Console.WriteLine(string.Join(", ", resultadoLinq)); // Saída esperada: 1, 2, 3, 12

            Console.WriteLine();

            // --- Solução 2: Usando Loop ---
            int[] resultadoLoop = EncontrarDiferencaComLoop(A, B);
            Console.WriteLine("Resultado com Loop (elementos em A que não estão em B):");
            Console.WriteLine(string.Join(", ", resultadoLoop)); // Saída esperada: 1, 2, 3, 12
        }

        /// <summary>
        /// Encontra os elementos que estão no array 'a' mas não no array 'b' utilizando LINQ.
        /// </summary>
        /// <param name="a">O primeiro array de inteiros.</param>
        /// <param name="b">O segundo array de inteiros.</param>
        /// <returns>Um novo array com os elementos exclusivos de 'a'.</returns>
        public static int[] EncontrarDiferencaComLinq(int[] a, int[] b)
        {
            // O método Except() retorna os elementos da primeira coleção
            // que não aparecem na segunda coleção.
            // .ToArray() converte o resultado (que é um IEnumerable<int>) de volta para um array.
            return a.Except(b).ToArray();
        }

        /// <summary>
        /// Encontra os elementos que estão no array 'a' mas não no array 'b' utilizando um loop.
        /// </summary>
        /// <param name="a">O primeiro array de inteiros.</param>
        /// <param name="b">O segundo array de inteiros.</param>
        /// <returns>Um novo array com os elementos exclusivos de 'a'.</returns>
        public static int[] EncontrarDiferencaComLoop(int[] a, int[] b)
        {
            // Para otimizar a busca, convertemos o array B para um HashSet.
            // A verificação .Contains() em um HashSet é muito mais rápida (quase O(1))
            // do que verificar em um array ou lista (O(n)).
            var setB = new HashSet<int>(b);

            // Usamos uma lista para armazenar os resultados, pois não sabemos o tamanho final.
            var resultado = new List<int>();

            // Percorremos cada elemento do array A.
            foreach (int numeroA in a)
            {
                // Se o elemento de A NÃO estiver contido no HashSet de B...
                if (!setB.Contains(numeroA))
                {
                    // ...adicionamos à nossa lista de resultados.
                    resultado.Add(numeroA);
                }
            }

            // Convertemos a lista de resultados para um array antes de retornar.
            return resultado.ToArray();
        }
    }
}