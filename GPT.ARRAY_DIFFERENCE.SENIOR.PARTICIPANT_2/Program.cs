using System;
using System.Linq;

namespace GPT.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_2
{
    class Program
    {
        static void Main()
        {
            int[] A = { 1, 2, 3, 4, 5, 6 };
            int[] B = { 4, 5, 7, 8 };

            // 1) Usando LINQ
            int[] resultadoLinq = A
                .Where(x => !B.Contains(x))
                .ToArray();

            Console.WriteLine("Resultado com LINQ: " + string.Join(", ", resultadoLinq));

            // 2) Usando loop
            int[] resultadoLoop = DiferencaComLoop(A, B);
            Console.WriteLine("Resultado com loop: " + string.Join(", ", resultadoLoop));
        }

        static int[] DiferencaComLoop(int[] A, int[] B)
        {
            // Para busca mais rápida em B, convertemos para HashSet
            var conjuntoB = B.ToHashSet();
            var listaResultado = new System.Collections.Generic.List<int>();

            foreach (var valor in A)
            {
                if (!conjuntoB.Contains(valor))
                {
                    listaResultado.Add(valor);
                }
            }

            return listaResultado.ToArray();
        }
    }
}