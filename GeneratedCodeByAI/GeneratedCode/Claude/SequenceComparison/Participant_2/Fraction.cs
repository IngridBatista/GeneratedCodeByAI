using System;
using System.Collections.Generic;
using System.Linq;

namespace CLAUDE.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_2
{
    // 1. Classe Fraction
    public class Fraction
    {
        public int Numerator { get; set; }
        public int Denominator { get; set; }

        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0)
            {
                throw new ArgumentException("Denominador não pode ser zero.");
            }

            Numerator = numerator;
            Denominator = denominator;
        }

        public double ToDecimal()
        {
            return (double)Numerator / Denominator;
        }

        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }

        public bool IsNegative()
        {
            return (Numerator < 0 && Denominator > 0) || (Numerator > 0 && Denominator < 0);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // 2. Leitura da Sequência A
                List<double> sequenciaA = LerSequenciaA();

                // 3. Leitura da Sequência B
                List<Fraction> sequenciaB = LerSequenciaB();

                // 4. Validação
                if (sequenciaA.Count == 0 || sequenciaB.Count == 0)
                {
                    throw new InvalidOperationException(
                        "Erro: Ambas as sequências devem conter pelo menos um elemento.");
                }

                // 5. Processamento e Saída
                ProcessarESaida(sequenciaA, sequenciaB);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }

        static List<double> LerSequenciaA()
        {
            Console.WriteLine("=== SEQUÊNCIA A (números decimais) ===");
            Console.WriteLine("Digite números decimais (0 para finalizar):");

            List<double> sequencia = new List<double>();

            while (true)
            {
                Console.Write("Número: ");
                string input = Console.ReadLine();

                if (double.TryParse(input, out double numero))
                {
                    if (numero == 0)
                    {
                        break; // Para e descarta o 0
                    }
                    sequencia.Add(numero);
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Digite um número decimal válido.");
                }
            }

            Console.WriteLine($"Sequência A: [{string.Join(", ", sequencia)}]");
            return sequencia;
        }

        static List<Fraction> LerSequenciaB()
        {
            Console.WriteLine("\n=== SEQUÊNCIA B (frações) ===");
            Console.WriteLine("Digite frações no formato 'numerador/denominador'");
            Console.WriteLine("(fração negativa para finalizar):");

            List<Fraction> sequencia = new List<Fraction>();

            while (true)
            {
                Console.Write("Fração: ");
                string input = Console.ReadLine();

                try
                {
                    string[] partes = input.Split('/');

                    if (partes.Length != 2)
                    {
                        Console.WriteLine("Formato inválido. Use: numerador/denominador");
                        continue;
                    }

                    if (int.TryParse(partes[0], out int numerador) &&
                        int.TryParse(partes[1], out int denominador))
                    {
                        Fraction fracao = new Fraction(numerador, denominador);

                        if (fracao.IsNegative())
                        {
                            break; // Para e descarta a fração negativa
                        }

                        sequencia.Add(fracao);
                    }
                    else
                    {
                        Console.WriteLine("Valores inválidos. Digite números inteiros.");
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                }
            }

            Console.WriteLine($"Sequência B: [{string.Join(", ", sequencia)}]");
            return sequencia;
        }

        static void ProcessarESaida(List<double> sequenciaA, List<Fraction> sequenciaB)
        {
            // Ordenar sequência A para encontrar a mediana
            List<double> aOrdenada = sequenciaA.OrderBy(x => x).ToList();

            // Calcular o valor da mediana (metade dos números)
            double valorReferencia;
            int tamanho = aOrdenada.Count;

            if (tamanho % 2 == 0)
            {
                // Se par, média dos dois valores centrais
                valorReferencia = (aOrdenada[tamanho / 2 - 1] + aOrdenada[tamanho / 2]) / 2.0;
            }
            else
            {
                // Se ímpar, valor central
                valorReferencia = aOrdenada[tamanho / 2];
            }

            Console.WriteLine($"\n=== RESULTADO ===");
            Console.WriteLine($"Valor de referência (mediana): {valorReferencia:F4}");
            Console.WriteLine("\nFrações maiores que pelo menos metade dos números em A:");

            List<Fraction> resultado = new List<Fraction>();

            foreach (Fraction fracao in sequenciaB)
            {
                double valorDecimal = fracao.ToDecimal();

                if (valorDecimal > valorReferencia)
                {
                    resultado.Add(fracao);
                    Console.WriteLine($"{fracao} = {valorDecimal:F4}");
                }
            }

            if (resultado.Count == 0)
            {
                Console.WriteLine("Nenhuma fração atende ao critério.");
            }
            else
            {
                Console.WriteLine($"\nTotal: {resultado.Count} fração(ões) encontrada(s).");
            }
        }
    }
}