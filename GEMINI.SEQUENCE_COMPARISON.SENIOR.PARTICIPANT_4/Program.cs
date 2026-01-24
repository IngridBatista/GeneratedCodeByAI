using System;
using System.Collections.Generic;

namespace GEMINI.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_4
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var comparer = new CompareSequence();

            try
            {
                List<double> sequenceA = comparer.ReadDoubleSequence();
                List<Fraction> sequenceB = comparer.ReadFractionSequence();

                if (sequenceA.Count == 0)
                {
                    Console.WriteLine("\nErro: A sequência A não pode ser vazia.");
                    return;
                }

                if (sequenceB.Count == 0)
                {
                    Console.WriteLine("\nErro: A sequência B não pode ser vazia.");
                    return;
                }

                List<Fraction> resultFractions = comparer.FindMatchingFractions(sequenceA, sequenceB);

                Console.WriteLine("\nFrações da sequência B maiores que pelo menos metade dos números da sequência A:");
                if (resultFractions.Count > 0)
                {
                    foreach (var fraction in resultFractions)
                    {
                        Console.WriteLine(fraction.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("Nenhuma fração encontrada que satisfaça a condição.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nOcorreu um erro inesperado: {ex.Message}");
            }
        }
    }
}