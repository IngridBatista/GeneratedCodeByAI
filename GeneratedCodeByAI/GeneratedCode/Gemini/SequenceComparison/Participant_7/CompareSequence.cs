using System;
using System.Collections.Generic;
using System.Linq;

namespace GEMINI.SEQUENCE_COMPARISON.JUNIOR.PARTICIPANT_7
{
    // CompareSequence.cs

    /// <summary>
    /// Orquestra a leitura de duas sequências, compara-as conforme as regras
    /// e exibe o resultado.
    /// </summary>
    public class CompareSequence
    {
        /// <summary>
        /// Executa o fluxo completo do programa.
        /// </summary>
        public void Run()
        {
            try
            {
                Console.WriteLine("--- Leitura da Sequência A (números decimais) ---");
                var sequenceA = ReadDoubleSequence();

                Console.WriteLine("\n--- Leitura da Sequência B (frações) ---");
                var sequenceB = ReadFractionSequence();

                if (sequenceA.Count == 0 || sequenceB.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nErro: Ambas as sequências A e B devem conter pelo menos um elemento.");
                    Console.ResetColor();
                    return;
                }

                var resultFractions = FindMatchingFractions(sequenceA, sequenceB);

                PrintResults(resultFractions);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nOcorreu um erro inesperado: {ex.Message}");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Lê uma sequência de doubles do console até que o usuário digite 0.
        /// </summary>
        /// <returns>Uma lista de doubles.</returns>
        private List<double> ReadDoubleSequence()
        {
            var sequence = new List<double>();
            Console.WriteLine("Digite os números da sequência A. Digite 0 para terminar.");

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
            Console.WriteLine($"Sequência A lida com {sequence.Count} elemento(s).");
            return sequence;
        }

        /// <summary>
        /// Lê uma sequência de frações do console até que uma fração negativa seja inserida.
        /// </summary>
        /// <returns>Uma lista de frações.</returns>
        private List<Fraction> ReadFractionSequence()
        {
            var sequence = new List<Fraction>();
            Console.WriteLine("Digite as frações da sequência B no formato 'numerador/denominador'.");
            Console.WriteLine("Digite uma fração negativa (ex: -1/2) para terminar.");

            while (true)
            {
                Console.Write("> ");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Entrada inválida.");
                    continue;
                }

                string[] parts = input.Split('/');
                if (parts.Length != 2 || !int.TryParse(parts[0], out int num) || !int.TryParse(parts[1], out int den))
                {
                    Console.WriteLine("Formato inválido. Use 'numerador/denominador'.");
                    continue;
                }

                try
                {
                    var fraction = new Fraction(num, den);

                    if (fraction.ToDouble() < 0)
                    {
                        break; // Condição de parada
                    }

                    sequence.Add(fraction);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                }
            }
            Console.WriteLine($"Sequência B lida com {sequence.Count} elemento(s).");
            return sequence;
        }

        /// <summary>
        /// Encontra as frações na sequência B que são maiores que pelo menos metade dos elementos da sequência A.
        /// </summary>
        /// <param name="sequenceA">A lista de doubles.</param>
        /// <param name="sequenceB">A lista de frações.</param>
        /// <returns>Uma lista com as frações que satisfazem a condição.</returns>
        private List<Fraction> FindMatchingFractions(List<double> sequenceA, List<Fraction> sequenceB)
        {
            // A condição é ser "maior do que pelo menos metade", ou seja, > 50% dos elementos.
            int requiredCount = sequenceA.Count / 2;

            var result = new List<Fraction>();

            foreach (var fraction in sequenceB)
            {
                // Usando LINQ para uma contagem limpa e expressiva.
                int countGreaterThan = sequenceA.Count(numberA => fraction.IsGreater(numberA));

                if (countGreaterThan > requiredCount)
                {
                    result.Add(fraction);
                }
            }

            return result;
        }

        /// <summary>
        /// Imprime o resultado final no console.
        /// </summary>
        /// <param name="fractions">A lista de frações a ser impressa.</param>
        private void PrintResults(List<Fraction> fractions)
        {
            Console.WriteLine("\n--- Resultado ---");
            if (fractions.Count == 0)
            {
                Console.WriteLine("Nenhuma fração da sequência B atendeu ao critério.");
            }
            else
            {
                Console.WriteLine("Frações da sequência B que são maiores que pelo menos metade dos números da sequência A:");
                foreach (var fraction in fractions)
                {
                    Console.WriteLine($"- {fraction}");
                }
            }
        }
    }
}