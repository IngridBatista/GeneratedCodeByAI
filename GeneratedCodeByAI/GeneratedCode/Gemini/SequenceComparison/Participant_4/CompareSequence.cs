using System;
using System.Collections.Generic;
using System.Linq;

namespace GEMINI.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_4
{
    public class CompareSequence
    {
        /// <summary>
        /// Lê uma sequência de valores double da entrada padrão até que 0 seja inserido.
        /// </summary>
        /// <returns>Uma lista de doubles.</returns>
        public List<double> ReadDoubleSequence()
        {
            var sequence = new List<double>();
            Console.WriteLine("Insira a sequência A (valores double, digite 0 para terminar):");

            while (true)
            {
                Console.Write("> ");
                string? input = Console.ReadLine();

                if (!double.TryParse(input, out double value))
                {
                    Console.WriteLine("Entrada inválida. Por favor, insira um número double.");
                    continue;
                }

                if (value == 0)
                {
                    break;
                }

                sequence.Add(value);
            }
            return sequence;
        }

        /// <summary>
        /// Lê uma sequência de frações da entrada padrão até que uma fração negativa seja inserida.
        /// </summary>
        /// <returns>Uma lista de Frações.</returns>
        public List<Fraction> ReadFractionSequence()
        {
            var sequence = new List<Fraction>();
            Console.WriteLine("\nInsira a sequência B (frações no formato 'numerador/denominador', uma fração negativa para terminar):");

            while (true)
            {
                Console.Write("> ");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Entrada vazia. Tente novamente.");
                    continue;
                }

                string[] parts = input.Split('/');
                if (parts.Length != 2 || !long.TryParse(parts[0], out long num) || !long.TryParse(parts[1], out long den))
                {
                    Console.WriteLine("Formato inválido. Use 'numerador/denominador'.");
                    continue;
                }

                try
                {
                    var fraction = new Fraction(num, den);
                    if (fraction.ToDouble() < 0)
                    {
                        break;
                    }
                    sequence.Add(fraction);
                }
                catch (DivideByZeroException ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                }
            }
            return sequence;
        }

        /// <summary>
        /// Encontra todas as frações na sequência B que são maiores que pelo menos metade dos números na sequência A.
        /// </summary>
        /// <param name="sequenceA">A sequência de doubles.</param>
        /// <param name="sequenceB">A sequência de frações.</param>
        /// <returns>Uma lista de frações que satisfazem a condição.</returns>
        public List<Fraction> FindMatchingFractions(List<double> sequenceA, List<Fraction> sequenceB)
        {
            var result = new List<Fraction>();
            double halfCount = sequenceA.Count / 2.0;

            foreach (var fraction in sequenceB)
            {
                // Usando LINQ para contar quantos elementos em A são menores que a fração atual.
                // A sobrecarga do operador '<' na classe Fraction permite a comparação direta.
                int countSmaller = sequenceA.Count(aValue => aValue < fraction);

                if (countSmaller >= halfCount)
                {
                    result.Add(fraction);
                }
            }
            return result;
        }
    }
}