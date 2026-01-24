using System;
using System.Collections.Generic;
using System.Linq;

namespace GEMINI.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_2
{
    // 1. Classe Fraction: Representa uma fração com numerador e denominador.
    public class Fraction
    {
        public int Numerator { get; }
        public int Denominator { get; }

        /// <summary>
        /// Construtor para a classe Fraction.
        /// Lança uma exceção se o denominador for zero.
        /// </summary>
        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0)
            {
                throw new ArgumentException("O denominador não pode ser zero.");
            }
            Numerator = numerator;
            Denominator = denominator;
        }

        /// <summary>
        /// Converte a fração para seu valor decimal (double).
        /// </summary>
        public double ToDecimal()
        {
            // É necessário converter um dos operandos para double para garantir a divisão de ponto flutuante.
            return (double)Numerator / Denominator;
        }

        /// <summary>
        /// Sobrescreve o método ToString para uma representação amigável da fração.
        /// </summary>
        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // 2. Leitura da Sequência A (List<double>)
                List<double> sequenceA = ReadSequenceA();

                // 3. Leitura da Sequência B (List<Fraction>)
                List<Fraction> sequenceB = ReadSequenceB();

                // 4. Validação: Verifica se alguma das sequências está vazia.
                if (sequenceA.Count == 0 || sequenceB.Count == 0)
                {
                    Console.WriteLine("\nErro: Ambas as sequências devem conter pelo menos um elemento para a comparação.");
                    return; // Finaliza a execução
                }

                // 5. Saída: Encontra e imprime as frações que atendem ao critério.
                Console.WriteLine("\n--- Resultado ---");
                Console.WriteLine("Frações em B cujo valor decimal é maior que pelo menos metade dos números em A:");

                // Calcula o número mínimo de elementos em A que uma fração de B deve superar.
                // Usamos 'Count / 2.0' para garantir a divisão de ponto flutuante, lidando corretamente com listas de tamanho ímpar.
                double halfCountOfA = sequenceA.Count / 2.0;

                bool foundAny = false;

                foreach (var fraction in sequenceB)
                {
                    // Usando LINQ para contar quantos números em A são menores que o valor decimal da fração.
                    int countGreater = sequenceA.Count(numberA => fraction.ToDecimal() > numberA);

                    // Verifica se a contagem atende ao critério (maior ou igual à metade).
                    if (countGreater >= halfCountOfA)
                    {
                        Console.WriteLine($"- {fraction} (valor decimal: {fraction.ToDecimal():F4})");
                        foundAny = true;
                    }
                }

                if (!foundAny)
                {
                    Console.WriteLine("Nenhuma fração em B atendeu ao critério especificado.");
                }
            }
            catch (Exception ex)
            {
                // Captura exceções gerais (como a da classe Fraction) e exibe uma mensagem de erro.
                Console.WriteLine($"\nOcorreu um erro inesperado: {ex.Message}");
            }
        }

        /// <summary>
        /// Lê uma sequência de números double do console até que o usuário digite '0'.
        /// </summary>
        private static List<double> ReadSequenceA()
        {
            Console.WriteLine("--- Inserir Sequência A (números decimais) ---");
            Console.WriteLine("Digite um número por linha. Digite '0' para finalizar.");

            var sequence = new List<double>();
            while (true)
            {
                Console.Write("A > ");
                string input = Console.ReadLine();

                if (double.TryParse(input, out double number))
                {
                    if (number == 0)
                    {
                        break; // Condição de parada
                    }
                    sequence.Add(number);
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Por favor, insira um número válido.");
                }
            }
            return sequence;
        }

        /// <summary>
        /// Lê uma sequência de frações do console até que uma fração negativa seja inserida.
        /// </summary>
        private static List<Fraction> ReadSequenceB()
        {
            Console.WriteLine("\n--- Inserir Sequência B (frações no formato N/D) ---");
            Console.WriteLine("Digite uma fração por linha (ex: 3/4). Uma fração negativa (ex: -1/2) para finalizar.");

            var sequence = new List<Fraction>();
            while (true)
            {
                Console.Write("B > ");
                string input = Console.ReadLine();
                string[] parts = input.Split('/');

                if (parts.Length != 2 || !int.TryParse(parts[0], out int num) || !int.TryParse(parts[1], out int den))
                {
                    Console.WriteLine("Formato inválido. Use o formato Numerador/Denominador (ex: 3/4).");
                    continue;
                }

                if (den == 0)
                {
                    Console.WriteLine("Erro: O denominador não pode ser zero. Tente novamente.");
                    continue;
                }

                var fraction = new Fraction(num, den);

                // Condição de parada: fração com valor decimal negativo.
                if (fraction.ToDecimal() < 0)
                {
                    break;
                }

                sequence.Add(fraction);
            }
            return sequence;
        }
    }
}