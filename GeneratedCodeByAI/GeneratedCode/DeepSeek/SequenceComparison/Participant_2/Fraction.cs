using System;
using System.Collections.Generic;
using System.Linq;

namespace DEEPSEEK.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_2
{
    // 1. Classe Fraction
    public class Fraction
    {
        public int Numerator { get; private set; }
        public int Denominator { get; private set; }

        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0)
                throw new ArgumentException("Denominador não pode ser zero.");

            Numerator = numerator;
            Denominator = denominator;
        }

        // Método para obter o valor decimal
        public double ToDecimal()
        {
            return (double)Numerator / Denominator;
        }

        // Sobrescrever ToString()
        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // 2. Sequência A (List<double>)
                Console.WriteLine("Digite os valores da sequência A (números double):");
                Console.WriteLine("Digite 0 para finalizar a entrada.");

                List<double> sequenceA = new List<double>();
                while (true)
                {
                    string input = Console.ReadLine();
                    if (double.TryParse(input, out double value))
                    {
                        if (value == 0)
                        {
                            break; // Para a leitura quando 0 for inserido
                        }
                        sequenceA.Add(value);
                    }
                    else
                    {
                        Console.WriteLine("Valor inválido. Digite um número double válido.");
                    }
                }

                // 3. Sequência B (List<Fraction>)
                Console.WriteLine("\nDigite as frações da sequência B (formato: numerador/denominador):");
                Console.WriteLine("Digite uma fração negativa para finalizar a entrada.");

                List<Fraction> sequenceB = new List<Fraction>();
                while (true)
                {
                    string input = Console.ReadLine();

                    // Verifica se é uma fração negativa para parar
                    if (input.StartsWith("-"))
                    {
                        break; // Para a leitura quando fração negativa for inserida
                    }

                    // Tenta parsear a fração
                    string[] parts = input.Split('/');
                    if (parts.Length == 2 &&
                        int.TryParse(parts[0], out int numerator) &&
                        int.TryParse(parts[1], out int denominator))
                    {
                        try
                        {
                            Fraction fraction = new Fraction(numerator, denominator);
                            sequenceB.Add(fraction);
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine($"Erro: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Formato inválido. Use o formato: numerador/denominador");
                    }
                }

                // 4. Validação
                if (sequenceA.Count == 0 || sequenceB.Count == 0)
                {
                    throw new InvalidOperationException("Ambas as sequências devem conter pelo menos um elemento.");
                }

                // 5. Saída
                Console.WriteLine("\nResultado:");
                Console.WriteLine($"Sequência A tem {sequenceA.Count} elementos.");
                Console.WriteLine($"Sequência B tem {sequenceB.Count} elementos.");

                // Calcula o limite: metade dos números na sequência A
                // Ordena a sequência A para encontrar a mediana da metade inferior
                var sortedA = sequenceA.OrderBy(x => x).ToList();
                int halfIndex = sortedA.Count / 2;
                double halfThreshold = sortedA[halfIndex];

                Console.WriteLine($"\nFração limite (mediana da metade inferior de A): {halfThreshold:F4}");
                Console.WriteLine("\nFrações em B cujo valor decimal é maior que pelo menos metade dos números em A:");

                bool foundAny = false;
                foreach (var fraction in sequenceB)
                {
                    double decimalValue = fraction.ToDecimal();
                    if (decimalValue > halfThreshold)
                    {
                        Console.WriteLine($"Fraçao: {fraction} = {decimalValue:F4}");
                        foundAny = true;
                    }
                }

                if (!foundAny)
                {
                    Console.WriteLine("Nenhuma fração atende ao critério.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }

            Console.WriteLine("\nPressione qualquer tecla para sair...");
            Console.ReadKey();
        }
    }
}