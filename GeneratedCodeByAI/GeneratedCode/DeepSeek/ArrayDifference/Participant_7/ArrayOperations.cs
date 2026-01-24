using System;
using System.Collections.Generic;
using System.Linq;

namespace DEEPSEEK.ARRAY_DIFFERENCE.JUNIOR.PARTICIPANT_7
{
    public static class ArrayOperations
    {
        /// <summary>
        /// Retorna um array contendo todos os elementos do array 'a' que não aparecem no array 'b'.
        /// </summary>
        /// <param name="a">Array de origem</param>
        /// <param name="b">Array para comparação</param>
        /// <returns>Array com elementos de 'a' que não estão em 'b'</returns>
        public static int[] Difference(int[] a, int[] b)
        {
            // Validação de entrada (mesmo que o problema diga que não será nulo,
            // é uma boa prática defensiva)
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));

            // Caso especial: se 'a' estiver vazio, retorna array vazio
            if (a.Length == 0)
            {
                return Array.Empty<int>();
            }

            // Caso especial: se 'b' estiver vazio, retorna cópia de 'a'
            if (b.Length == 0)
            {
                return (int[])a.Clone();
            }

            // Usando HashSet para O(1) lookups - melhor performance para arrays grandes
            var bSet = new HashSet<int>(b);

            // Usando LINQ para clareza do código (mantém a intenção explícita)
            return a.Where(element => !bSet.Contains(element)).ToArray();
        }
    }

    // Implementação alternativa com loops explícitos (para maior controle)
    public static class ArrayOperationsAlternative
    {
        public static int[] Difference(int[] a, int[] b)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));

            if (a.Length == 0) return Array.Empty<int>();
            if (b.Length == 0) return (int[])a.Clone();

            var bSet = new HashSet<int>(b);
            var result = new List<int>(a.Length); // Capacidade inicial otimizada

            foreach (var element in a)
            {
                if (!bSet.Contains(element))
                {
                    result.Add(element);
                }
            }

            return result.ToArray();
        }
    }

    // Exemplo de uso e testes
    public class Program
    {
        public static void Main()
        {
            // Teste 1: Caso básico
            int[] a1 = { 1, 2, 3, 4, 5 };
            int[] b1 = { 4, 5, 6, 7, 8 };
            var result1 = ArrayOperations.Difference(a1, b1);
            Console.WriteLine($"Teste 1: [{string.Join(", ", result1)}]"); // [1, 2, 3]

            // Teste 2: Elementos duplicados
            int[] a2 = { 1, 2, 2, 3, 3, 3 };
            int[] b2 = { 2 };
            var result2 = ArrayOperations.Difference(a2, b2);
            Console.WriteLine($"Teste 2: [{string.Join(", ", result2)}]"); // [1, 3, 3, 3]

            // Teste 3: Array 'a' vazio
            int[] a3 = Array.Empty<int>();
            int[] b3 = { 1, 2, 3 };
            var result3 = ArrayOperations.Difference(a3, b3);
            Console.WriteLine($"Teste 3: [{string.Join(", ", result3)}]"); // []

            // Teste 4: Array 'b' vazio
            int[] a4 = { 1, 2, 3 };
            int[] b4 = Array.Empty<int>();
            var result4 = ArrayOperations.Difference(a4, b4);
            Console.WriteLine($"Teste 4: [{string.Join(", ", result4)}]"); // [1, 2, 3]

            // Teste 5: Nenhum elemento em comum
            int[] a5 = { 1, 2, 3 };
            int[] b5 = { 4, 5, 6 };
            var result5 = ArrayOperations.Difference(a5, b5);
            Console.WriteLine($"Teste 5: [{string.Join(", ", result5)}]"); // [1, 2, 3]
        }
    }
}