using System;
using System.Collections.Generic;

namespace DEEPSEEK.ARRAY_DIFFERENCE.PLENO.PARTICIPANT_6
{
    public class Program
    {
        public static int[] ElementosExclusivos(int[] a, int[] b)
        {
            // Converte o array b para um HashSet para busca eficiente O(1)
            HashSet<int> conjuntoB = new HashSet<int>(b);

            // Lista temporária para armazenar os resultados
            List<int> resultado = new List<int>();

            // Percorre o array a verificando quais elementos não estão em b
            foreach (int elemento in a)
            {
                if (!conjuntoB.Contains(elemento))
                {
                    resultado.Add(elemento);
                }
            }

            // Converte a lista para array e retorna
            return resultado.ToArray();
        }

        // Exemplo de uso
        public static void Main()
        {
            int[] a = { 1, 2, 3, 4, 5, 6 };
            int[] b = { 2, 4, 6 };

            int[] resultado = ElementosExclusivos(a, b);

            Console.WriteLine("Elementos em a que não estão em b:");
            foreach (int num in resultado)
            {
                Console.Write(num + " ");
            }
            // Saída: 1 3 5
        }
    }
}