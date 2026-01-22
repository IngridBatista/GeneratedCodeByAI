using System;
using System.Linq;

namespace CLAUDE.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_2
{
    class Program
    {
        static void Main()
        {
            int[] A = { 1, 2, 3, 4, 5, 6 };
            int[] B = { 2, 4, 6, 8, 10 };

            // Solução com LINQ - Except
            int[] resultado = A.Except(B).ToArray();

            Console.WriteLine("Elementos em A que não estão em B:");
            Console.WriteLine(string.Join(", ", resultado));
            // Output: 1, 3, 5
        }
    }
}