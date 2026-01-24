using System;
using System.Collections.Generic;
using System.Globalization;

namespace DEEPSEEK.SEQUENCE_COMPARISON.JUNIOR.PARTICIPANT_7
{
    /// <summary>
    /// Classe que representa uma fração com numerador e denominador inteiros
    /// </summary>
    public class Fraction : IComparable<Fraction>, IEquatable<Fraction>
    {
        public int Numerator { get; private set; }
        public int Denominator { get; private set; }

        /// <summary>
        /// Valor numérico da fração como double
        /// </summary>
        public double Value => (double)Numerator / Denominator;

        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0)
                throw new ArgumentException("Denominator cannot be zero.", nameof(denominator));

            Numerator = numerator;
            Denominator = denominator;
            Simplify();
        }

        /// <summary>
        /// Verifica se a fração é menor que outra fração
        /// </summary>
        public bool IsLesser(Fraction other)
        {
            if (other == null) return false;
            return CompareTo(other) < 0;
        }

        /// <summary>
        /// Verifica se a fração é maior que outra fração
        /// </summary>
        public bool IsGreater(Fraction other)
        {
            if (other == null) return false;
            return CompareTo(other) > 0;
        }

        /// <summary>
        /// Simplifica a fração usando o máximo divisor comum
        /// </summary>
        private void Simplify()
        {
            if (Numerator == 0)
            {
                Denominator = 1;
                return;
            }

            int gcd = GreatestCommonDivisor(Math.Abs(Numerator), Math.Abs(Denominator));
            Numerator /= gcd;
            Denominator /= gcd;

            // Garantir que o denominador seja positivo
            if (Denominator < 0)
            {
                Numerator = -Numerator;
                Denominator = -Denominator;
            }
        }

        /// <summary>
        /// Calcula o máximo divisor comum usando o algoritmo de Euclides
        /// </summary>
        private static int GreatestCommonDivisor(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return Math.Abs(a);
        }

        public int CompareTo(Fraction other)
        {
            if (other == null) return 1;

            // Comparação cruzada para evitar problemas de precisão
            long left = (long)Numerator * other.Denominator;
            long right = (long)other.Numerator * Denominator;

            return left.CompareTo(right);
        }

        public bool Equals(Fraction other)
        {
            if (other == null) return false;
            return CompareTo(other) == 0;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Fraction);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Numerator, Denominator);
        }

        public override string ToString()
        {
            return Denominator == 1 ? Numerator.ToString() : $"{Numerator}/{Denominator}";
        }

        /// <summary>
        /// Tenta analisar uma string no formato "numerador/denominador"
        /// </summary>
        public static bool TryParse(string input, out Fraction fraction)
        {
            fraction = null;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            var parts = input.Split('/');

            if (parts.Length != 2)
                return false;

            if (!int.TryParse(parts[0].Trim(), out int numerator))
                return false;

            if (!int.TryParse(parts[1].Trim(), out int denominator))
                return false;

            if (denominator == 0)
                return false;

            try
            {
                fraction = new Fraction(numerator, denominator);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Classe principal para comparação de sequências
    /// </summary>
    public class CompareSequence
    {
        private readonly List<double> _sequenceA;
        private readonly List<Fraction> _sequenceB;

        public CompareSequence()
        {
            _sequenceA = new List<double>();
            _sequenceB = new List<Fraction>();
        }

        /// <summary>
        /// Executa o processo completo de leitura e comparação
        /// </summary>
        public void Execute()
        {
            try
            {
                ReadSequenceA();
                ReadSequenceB();
                ProcessAndPrintResults();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }

        /// <summary>
        /// Lê a sequência A de valores double
        /// </summary>
        private void ReadSequenceA()
        {
            Console.WriteLine("Digite os números da sequência A (digite 0 para encerrar):");

            while (true)
            {
                Console.Write("Número: ");
                string input = Console.ReadLine()?.Trim();

                if (!double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
                {
                    Console.WriteLine("Valor inválido. Digite um número válido.");
                    continue;
                }

                if (Math.Abs(number) < double.Epsilon) // Verifica se é zero
                    break;

                _sequenceA.Add(number);
            }
        }

        /// <summary>
        /// Lê a sequência B de frações
        /// </summary>
        private void ReadSequenceB()
        {
            Console.WriteLine("\nDigite as frações da sequência B (formato: numerador/denominador):");
            Console.WriteLine("Digite uma fração negativa para encerrar.");

            while (true)
            {
                Console.Write("Fração: ");
                string input = Console.ReadLine()?.Trim();

                if (!Fraction.TryParse(input, out Fraction fraction))
                {
                    Console.WriteLine("Formato inválido. Use o formato: numerador/denominador");
                    continue;
                }

                // Verifica se a fração é negativa
                if (fraction.Value < 0)
                    break;

                _sequenceB.Add(fraction);
            }
        }

        /// <summary>
        /// Processa as sequências e imprime os resultados
        /// </summary>
        private void ProcessAndPrintResults()
        {
            // Validação das sequências
            if (_sequenceA.Count == 0 || _sequenceB.Count == 0)
            {
                Console.WriteLine("\nErro: Uma ou ambas as sequências estão vazias.");
                Console.WriteLine($"Sequência A tem {_sequenceA.Count} elementos.");
                Console.WriteLine($"Sequência B tem {_sequenceB.Count} elementos.");
                return;
            }

            // Calcula a mediana inferior (metade dos números)
            var sortedA = new List<double>(_sequenceA);
            sortedA.Sort();

            int halfIndex = sortedA.Count / 2;
            double threshold = sortedA[halfIndex];

            Console.WriteLine($"\nLimite (metade dos valores de A): {threshold:F4}");

            // Encontra frações que são maiores que pelo menos metade dos números de A
            var resultFractions = new List<Fraction>();

            foreach (var fraction in _sequenceB)
            {
                if (fraction.Value > threshold)
                {
                    resultFractions.Add(fraction);
                }
            }

            // Imprime os resultados
            Console.WriteLine($"\nFrações da sequência B maiores que pelo menos metade dos números de A:");

            if (resultFractions.Count == 0)
            {
                Console.WriteLine("Nenhuma fração atende ao critério.");
            }
            else
            {
                foreach (var fraction in resultFractions)
                {
                    Console.WriteLine($"{fraction} = {fraction.Value:F4}");
                }
            }

            // Informações adicionais para debug
            Console.WriteLine($"\n--- Informações das sequências ---");
            Console.WriteLine($"Sequência A ({_sequenceA.Count} elementos):");
            Console.WriteLine(string.Join(", ", _sequenceA.Select(x => x.ToString("F2"))));

            Console.WriteLine($"\nSequência B ({_sequenceB.Count} elementos):");
            foreach (var fraction in _sequenceB)
            {
                Console.WriteLine($"{fraction} = {fraction.Value:F4}");
            }
        }
    }

    /// <summary>
    /// Classe principal do programa
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Comparador de Sequências ===");
            Console.WriteLine("Este programa compara sequências de números e frações.\n");

            var comparator = new CompareSequence();
            comparator.Execute();

            Console.WriteLine("\nPressione qualquer tecla para sair...");
            Console.ReadKey();
        }
    }
}