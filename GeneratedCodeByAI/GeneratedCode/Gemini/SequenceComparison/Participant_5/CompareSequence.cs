using System;
using System.Collections.Generic;
using System.Linq;

namespace GEMINI.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_5
{
    // CompareSequence.cs

    /// <summary>
    /// Orquestra a leitura de sequências de números e frações,
    /// e compara-as de acordo com regras específicas.
    /// </summary>
    public class CompareSequence
    {
        /// <summary>
        /// Executa o fluxo principal do programa.
        /// </summary>
        public void Run()
        {
            try
            {
                var sequenceA = ReadDoubleSequence();
                var sequenceB = ReadFractionSequence();

                if (!ValidateSequences(sequenceA, sequenceB))
                {
                    return;
                }

                var resultFractions = FindMatchingFractions(sequenceA, sequenceB);

                PrintResult(resultFractions);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nOcorreu um erro inesperado: {ex.Message}");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Lê uma sequência de doubles do console até que o usuário digite '0'.
        /// </summary>
        private List<double> ReadDoubleSequence()
        {
            Console.WriteLine("--- Sequência A (números decimais) ---");
            Console.WriteLine("Digite os números um por um. Digite '0' para finalizar.");

            var sequence = new List<double>();
            while (true)
            {
                Console.Write("> ");
                string? input = Console.ReadLine();

                if (!double.TryParse(input, out double number))
                {
                    Console.WriteLine("Entrada inválida. Por favor, digite um número.");
                    continue;
                }

                if (number == 0)
                {
                    break;
                }

                sequence.Add(number);
            }
            return sequence;
        }

        /// <summary>
        /// Lê uma sequência de frações do console até que uma fração negativa seja inserida.
        /// </summary>
        private List<Fraction> ReadFractionSequence()
        {
            Console.WriteLine("\n--- Sequência B (frações no formato 'numerador/denominador') ---");
            Console.WriteLine("Digite as frações uma por uma. Uma fração negativa finaliza a leitura.");

            var sequence = new List<Fraction>();
            while (true)
            {
                Console.Write("> ");
                string? input = Console.ReadLine();

                if (!Fraction.TryParse(input, out Fraction? fraction))
                {
                    Console.WriteLine("Formato de fração inválido. Use 'numerador/denominador'.");
                    continue;
                }

                if (fraction!.ToDouble() < 0)
                {
                    break;
                }

                sequence.Add(fraction);
            }
            return sequence;
        }

        /// <summary>
        /// Valida se as sequências não estão vazias.
        /// </summary>
        private bool ValidateSequences(List<double> sequenceA, List<Fraction> sequenceB)
        {
            if (!sequenceA.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nErro: A sequência A está vazia. O programa será encerrado.");
                Console.ResetColor();
                return false;
            }

            if (!sequenceB.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nErro: A sequência B está vazia. O programa será encerrado.");
                Console.ResetColor();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Encontra as frações na sequência B que são maiores que pelo menos metade dos elementos da sequência A.
        /// </summary>
        private List<Fraction> FindMatchingFractions(List<double> sequenceA, List<Fraction> sequenceB)
        {
            var halfCountOfA = sequenceA.Count / 2.0;
            var matchingFractions = new List<Fraction>();

            foreach (var fraction in sequenceB)
            {
                int countGreater = sequenceA.Count(number => fraction.IsGreater(number));

                if (countGreater >= halfCountOfA)
                {
                    matchingFractions.Add(fraction);
                }
            }

            return matchingFractions;
        }

        /// <summary>
        /// Imprime o resultado final no console.
        /// </summary>
        private void PrintResult(List<Fraction> resultFractions)
        {
            Console.WriteLine("\n--- Resultado ---");
            Console.WriteLine("Frações da sequência B maiores que pelo menos metade dos números da sequência A:");

            if (!resultFractions.Any())
            {
                Console.WriteLine("Nenhuma fração encontrada que satisfaça a condição.");
            }
            else
            {
                foreach (var fraction in resultFractions)
                {
                    Console.WriteLine($"- {fraction}");
                }
            }
        }
    }
}