using System;
using System.Collections.Generic;
using System.Linq;

namespace CLAUDE.SEQUENCE_COMPARISON.JUNIOR.PARTICIPANT_7
{
    /// <summary>
    /// Representa uma fração matemática com numerador e denominador
    /// </summary>
    public class Fraction
    {
        public int Numerator { get; }
        public int Denominator { get; }

        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0)
                throw new ArgumentException("Denominador não pode ser zero.", nameof(denominator));

            Numerator = numerator;
            Denominator = denominator;
        }

        /// <summary>
        /// Calcula o valor decimal da fração
        /// </summary>
        public double ToDouble() => (double)Numerator / Denominator;

        /// <summary>
        /// Verifica se esta fração é menor que outra
        /// </summary>
        public bool IsLesser(Fraction other)
        {
            return ToDouble() < other.ToDouble();
        }

        /// <summary>
        /// Verifica se esta fração é maior que outra
        /// </summary>
        public bool IsGreater(Fraction other)
        {
            return ToDouble() > other.ToDouble();
        }

        /// <summary>
        /// Verifica se esta fração é menor que um valor double
        /// </summary>
        public bool IsLesser(double value)
        {
            return ToDouble() < value;
        }

        /// <summary>
        /// Verifica se esta fração é maior que um valor double
        /// </summary>
        public bool IsGreater(double value)
        {
            return ToDouble() > value;
        }

        /// <summary>
        /// Verifica se a fração é negativa
        /// </summary>
        public bool IsNegative()
        {
            return ToDouble() < 0;
        }

        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }
    }

    /// <summary>
    /// Interface para leitura de entrada
    /// </summary>
    public interface IInputReader
    {
        List<double> ReadDoubleSequence();
        List<Fraction> ReadFractionSequence();
    }

    /// <summary>
    /// Implementação de leitura via Console
    /// </summary>
    public class ConsoleInputReader : IInputReader
    {
        public List<double> ReadDoubleSequence()
        {
            var sequence = new List<double>();
            Console.WriteLine("Digite os valores da sequência A (0 para encerrar):");

            while (true)
            {
                if (!double.TryParse(Console.ReadLine(), out double value))
                {
                    Console.WriteLine("Valor inválido. Tente novamente.");
                    continue;
                }

                if (value == 0)
                    break;

                sequence.Add(value);
            }

            return sequence;
        }

        public List<Fraction> ReadFractionSequence()
        {
            var sequence = new List<Fraction>();
            Console.WriteLine("Digite as frações da sequência B no formato 'numerador/denominador':");
            Console.WriteLine("(Uma fração negativa encerra a leitura)");

            while (true)
            {
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Entrada inválida. Tente novamente.");
                    continue;
                }

                var parts = input.Split('/');

                if (parts.Length != 2 ||
                    !int.TryParse(parts[0], out int numerator) ||
                    !int.TryParse(parts[1], out int denominator))
                {
                    Console.WriteLine("Formato inválido. Use: numerador/denominador");
                    continue;
                }

                try
                {
                    var fraction = new Fraction(numerator, denominator);

                    if (fraction.IsNegative())
                        break;

                    sequence.Add(fraction);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                }
            }

            return sequence;
        }
    }

    /// <summary>
    /// Interface para análise de sequências
    /// </summary>
    public interface ISequenceAnalyzer
    {
        List<Fraction> FilterFractionsGreaterThanMedian(
            List<double> doubleSequence,
            List<Fraction> fractionSequence);
    }

    /// <summary>
    /// Analisador de sequências
    /// </summary>
    public class SequenceAnalyzer : ISequenceAnalyzer
    {
        public List<Fraction> FilterFractionsGreaterThanMedian(
            List<double> doubleSequence,
            List<Fraction> fractionSequence)
        {
            if (doubleSequence == null || doubleSequence.Count == 0)
                throw new InvalidOperationException("Sequência A está vazia.");

            if (fractionSequence == null || fractionSequence.Count == 0)
                throw new InvalidOperationException("Sequência B está vazia.");

            // Ordena a sequência A para encontrar a mediana
            var sortedA = doubleSequence.OrderBy(x => x).ToList();
            double median = CalculateMedian(sortedA);

            // Filtra frações maiores que a mediana
            return fractionSequence
                .Where(fraction => fraction.IsGreater(median))
                .ToList();
        }

        private double CalculateMedian(List<double> sortedSequence)
        {
            int count = sortedSequence.Count;
            int middleIndex = count / 2;

            if (count % 2 == 0)
            {
                // Se par, média dos dois valores centrais
                return (sortedSequence[middleIndex - 1] + sortedSequence[middleIndex]) / 2.0;
            }
            else
            {
                // Se ímpar, valor central
                return sortedSequence[middleIndex];
            }
        }
    }

    /// <summary>
    /// Interface para saída de dados
    /// </summary>
    public interface IOutputWriter
    {
        void WriteResults(List<Fraction> fractions);
        void WriteError(string message);
    }

    /// <summary>
    /// Implementação de saída via Console
    /// </summary>
    public class ConsoleOutputWriter : IOutputWriter
    {
        public void WriteResults(List<Fraction> fractions)
        {
            Console.WriteLine("\nFrações da sequência B maiores que a mediana da sequência A:");

            if (fractions.Count == 0)
            {
                Console.WriteLine("Nenhuma fração atende ao critério.");
                return;
            }

            foreach (var fraction in fractions)
            {
                Console.WriteLine($"{fraction} = {fraction.ToDouble():F4}");
            }
        }

        public void WriteError(string message)
        {
            Console.WriteLine($"ERRO: {message}");
        }
    }

    /// <summary>
    /// Classe principal que coordena a comparação de sequências
    /// </summary>
    public class CompareSequence
    {
        private readonly IInputReader _inputReader;
        private readonly ISequenceAnalyzer _analyzer;
        private readonly IOutputWriter _outputWriter;

        public CompareSequence(
            IInputReader inputReader,
            ISequenceAnalyzer analyzer,
            IOutputWriter outputWriter)
        {
            _inputReader = inputReader ?? throw new ArgumentNullException(nameof(inputReader));
            _analyzer = analyzer ?? throw new ArgumentNullException(nameof(analyzer));
            _outputWriter = outputWriter ?? throw new ArgumentNullException(nameof(outputWriter));
        }

        public void Execute()
        {
            try
            {
                // 1. Ler sequência A
                var sequenceA = _inputReader.ReadDoubleSequence();

                // 2. Ler sequência B
                var sequenceB = _inputReader.ReadFractionSequence();

                // 3. Processar e imprimir resultados
                var results = _analyzer.FilterFractionsGreaterThanMedian(sequenceA, sequenceB);
                _outputWriter.WriteResults(results);
            }
            catch (InvalidOperationException ex)
            {
                _outputWriter.WriteError(ex.Message);
            }
            catch (Exception ex)
            {
                _outputWriter.WriteError($"Erro inesperado: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Programa principal
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var inputReader = new ConsoleInputReader();
            var analyzer = new SequenceAnalyzer();
            var outputWriter = new ConsoleOutputWriter();

            var compareSequence = new CompareSequence(inputReader, analyzer, outputWriter);
            compareSequence.Execute();

            Console.WriteLine("\nPressione qualquer tecla para sair...");
            Console.ReadKey();
        }
    }
}