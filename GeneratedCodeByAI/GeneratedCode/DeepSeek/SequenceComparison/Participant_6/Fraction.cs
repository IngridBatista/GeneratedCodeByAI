using System;
using System.Collections.Generic;

namespace DEEPSEEK.SEQUENCE_COMPARISON.PLENO.PARTICIPANT_6
{
    // Classe para representar frações
    public class Fraction
    {
        public int Numerator { get; private set; }
        public int Denominator { get; private set; }

        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0)
                throw new ArgumentException("Denominator cannot be zero.");

            Numerator = numerator;
            Denominator = denominator;
        }

        // Valor numérico da fração
        public double Value => (double)Numerator / Denominator;

        // Método para verificar se esta fração é menor que outra
        public bool IsLesser(Fraction other)
        {
            return this.Value < other.Value;
        }

        // Método para verificar se esta fração é maior que outra
        public bool IsGreater(Fraction other)
        {
            return this.Value > other.Value;
        }

        // Método para verificar se esta fração é menor que um double
        public bool IsLesser(double value)
        {
            return this.Value < value;
        }

        // Método para verificar se esta fração é maior que um double
        public bool IsGreater(double value)
        {
            return this.Value > value;
        }

        // Método para verificar se o valor é negativo
        public bool IsNegative()
        {
            return (Numerator < 0 && Denominator > 0) || (Numerator > 0 && Denominator < 0);
        }

        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }
    }

    // Classe principal que implementa a lógica solicitada
    public class CompareSequence
    {
        public static void Main()
        {
            try
            {
                // 1. Ler sequência A de doubles
                Console.WriteLine("Digite os números da sequência A (digite 0 para encerrar):");
                List<double> sequenceA = new List<double>();

                while (true)
                {
                    string input = Console.ReadLine();
                    if (double.TryParse(input, out double value))
                    {
                        if (value == 0)
                            break;

                        sequenceA.Add(value);
                    }
                    else
                    {
                        Console.WriteLine("Valor inválido. Digite um número válido.");
                    }
                }

                // Verificar se A está vazia
                if (sequenceA.Count == 0)
                {
                    Console.WriteLine("Erro: A sequência A está vazia.");
                    return;
                }

                // 2. Ler sequência B de frações
                Console.WriteLine("\nDigite as frações da sequência B (formato: numerador denominador):");
                Console.WriteLine("Digite uma fração negativa para encerrar.");
                List<Fraction> sequenceB = new List<Fraction>();

                while (true)
                {
                    Console.Write("Numerador: ");
                    string numInput = Console.ReadLine();

                    if (!int.TryParse(numInput, out int numerator))
                    {
                        Console.WriteLine("Numerador inválido. Tente novamente.");
                        continue;
                    }

                    Console.Write("Denominador: ");
                    string denInput = Console.ReadLine();

                    if (!int.TryParse(denInput, out int denominator))
                    {
                        Console.WriteLine("Denominador inválido. Tente novamente.");
                        continue;
                    }

                    try
                    {
                        Fraction fraction = new Fraction(numerator, denominator);

                        // Verificar se a fração é negativa
                        if (fraction.IsNegative())
                        {
                            Console.WriteLine("Fração negativa detectada. Encerrando entrada da sequência B.");
                            break;
                        }

                        sequenceB.Add(fraction);
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Erro: {ex.Message}");
                    }
                }

                // Verificar se B está vazia
                if (sequenceB.Count == 0)
                {
                    Console.WriteLine("Erro: A sequência B está vazia.");
                    return;
                }

                // 3. Calcular metade dos números da sequência A
                List<double> halfOfA = new List<double>();
                foreach (double value in sequenceA)
                {
                    halfOfA.Add(value / 2.0);
                }

                // 4. Encontrar e imprimir frações que são maiores que pelo menos metade dos números de A
                Console.WriteLine("\nFrações da sequência B que são maiores que pelo menos metade dos números de A:");
                bool foundAny = false;

                foreach (Fraction fraction in sequenceB)
                {
                    int countGreater = 0;

                    foreach (double halfValue in halfOfA)
                    {
                        if (fraction.IsGreater(halfValue))
                        {
                            countGreater++;
                        }
                    }

                    // Verificar se é maior que pelo menos metade dos números de A
                    if (countGreater > 0)
                    {
                        Console.WriteLine($"Fração: {fraction} (Valor: {fraction.Value:F4})");
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
                Console.WriteLine($"Erro inesperado: {ex.Message}");
            }
        }
    }
}