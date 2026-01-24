using System;
using System.Linq;

namespace DEEPSEEK.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_1
{
    public class Program
    {
        public static int[] DiferencaEntreArrays(int[] array1, int[] array2)
        {
            // Usando LINQ para filtrar os elementos
            return array1.Where(x => !array2.Contains(x)).ToArray();
        }

        // Método alternativo usando HashSet para melhor performance com grandes arrays
        public static int[] DiferencaEntreArraysOtimizado(int[] array1, int[] array2)
        {
            // Converter o segundo array para HashSet para busca O(1)
            var set2 = new HashSet<int>(array2);

            // Filtrar elementos do primeiro array que não estão no HashSet
            return array1.Where(x => !set2.Contains(x)).ToArray();
        }

        // Método sem usar LINQ (para versões mais antigas do .NET)
        public static int[] DiferencaEntreArraysSemLINQ(int[] array1, int[] array2)
        {
            var resultado = new List<int>();

            foreach (var item in array1)
            {
                bool encontrado = false;

                foreach (var item2 in array2)
                {
                    if (item == item2)
                    {
                        encontrado = true;
                        break;
                    }
                }

                if (!encontrado)
                {
                    resultado.Add(item);
                }
            }

            return resultado.ToArray();
        }

        // Exemplo de uso
        public static void Main()
        {
            int[] array1 = { 1, 2, 3, 4, 5, 6 };
            int[] array2 = { 2, 4, 6 };

            // Usando o método com LINQ
            int[] diferenca = DiferencaEntreArrays(array1, array2);
            Console.WriteLine("Resultado: " + string.Join(", ", diferenca));
            // Saída: 1, 3, 5

            // Usando o método otimizado
            int[] diferencaOtimizada = DiferencaEntreArraysOtimizado(array1, array2);
            Console.WriteLine("Resultado otimizado: " + string.Join(", ", diferencaOtimizada));
            // Saída: 1, 3, 5
        }
    }
}