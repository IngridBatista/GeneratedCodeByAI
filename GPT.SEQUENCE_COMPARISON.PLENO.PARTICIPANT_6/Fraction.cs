using System;
using System.Collections.Generic;
using System.Globalization;

namespace GPT.SEQUENCE_COMPARISON.PLENO.PARTICIPANT_6
{
    public class Fraction : IComparable<Fraction>
    {
        public long Numerator { get; private set; }
        public long Denominator { get; private set; }

        public Fraction(long numerator, long denominator)
        {
            if (denominator == 0)
                throw new ArgumentException("Denominador não pode ser zero.");

            // Normaliza o sinal: denominador sempre positivo
            if (denominator < 0)
            {
                numerator = -numerator;
                denominator = -denominator;
            }

            long gcd = Gcd(Math.Abs(numerator), Math.Abs(denominator));
            Numerator = numerator / gcd;
            Denominator = denominator / gcd;
        }

        // Máximo divisor comum (algoritmo de Euclides)
        private static long Gcd(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a == 0 ? 1 : a;
        }

        public double ToDouble()
        {
            return (double)Numerator / Denominator;
        }

        public bool IsLesser(Fraction other)
        {
            return CompareTo(other) < 0;
        }

        public bool IsGreater(Fraction other)
        {
            return CompareTo(other) > 0;
        }

        public int CompareTo(Fraction other)
        {
            // Compara a/b com c/d usando multiplicação cruzada: a*d ? c*b
            // Usa decimal para reduzir risco de overflow em casos grandes
            decimal left = (decimal)Numerator * other.Denominator;
            decimal right = (decimal)other.Numerator * Denominator;
            return left.CompareTo(right);
        }

        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }
    }

    public class CompareSequence
    {
        public static void Main(string[] args)
        {
            // 1. Ler sequência A de doubles até 0
            List<double> sequenceA = new List<double>();
            Console.WriteLine("Digite valores double para a sequência A (0 para encerrar):");

            while (true)
            {
                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Entrada vazia. Tente novamente.");
                    continue;
                }

                if (!double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                {
                    Console.WriteLine("Valor inválido. Digite um número double.");
                    continue;
                }

                if (value == 0.0)
                    break;

                sequenceA.Add(value);
            }

            // 2. Ler sequência B de frações até fração negativa
            List<Fraction> sequenceB = new List<Fraction>();
            Console.WriteLine("Digite frações para a sequência B no formato numerador/denominador.");
            Console.WriteLine("A leitura será encerrada quando for digitada uma fração negativa.");

            while (true)
            {
                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Entrada vazia. Tente novamente.");
                    continue;
                }

                // Espera formato "a/b"
                string[] parts = input.Split('/');
                if (parts.Length != 2)
                {
                    Console.WriteLine("Formato inválido. Use numerador/denominador (ex: 3/4).");
                    continue;
                }

                if (!long.TryParse(parts[0].Trim(), out long num) ||
                    !long.TryParse(parts[1].Trim(), out long den))
                {
                    Console.WriteLine("Numerador ou denominador inválido. Use inteiros.");
                    continue;
                }

                Fraction fraction;
                try
                {
                    fraction = new Fraction(num, den);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                    continue;
                }

                // Verifica se é negativa (valor numérico < 0)
                if (fraction.ToDouble() < 0)
                {
                    // Não adiciona à sequência e encerra leitura
                    break;
                }

                sequenceB.Add(fraction);
            }

            // 3. Verificar se A ou B estão vazias
            if (sequenceA.Count == 0)
            {
                Console.WriteLine("Erro: a sequência A está vazia. Encerrando o programa.");
                return;
            }

            if (sequenceB.Count == 0)
            {
                Console.WriteLine("Erro: a sequência B está vazia. Encerrando o programa.");
                return;
            }

            // 4. Imprimir frações de B cujo valor seja maior do que
            //    pelo menos metade dos números da sequência A.
            Console.WriteLine("Frações de B cujo valor é maior que pelo menos metade dos valores de A:");

            foreach (var frac in sequenceB)
            {
                double fracValue = frac.ToDouble();
                int countGreater = 0;

                foreach (var a in sequenceA)
                {
                    if (fracValue > a)
                        countGreater++;
                }

                if (countGreater >= sequenceA.Count / 2.0)
                {
                    Console.WriteLine(frac);
                }
            }
        }
    }
}