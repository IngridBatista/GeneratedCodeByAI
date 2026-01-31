using System;

namespace GEMINI.SEQUENCE_COMPARISON.PLENO.PARTICIPANT_6
{
    // Fraction.cs

    /// <summary>
    /// Representa uma fração com um numerador e um denominador.
    /// A fração é sempre armazenada em sua forma mais simples.
    /// </summary>
    public class Fraction : IComparable<Fraction>
    {
        public long Numerator { get; private set; }
        public long Denominator { get; private set; }

        public Fraction(long numerator, long denominator)
        {
            if (denominator == 0)
            {
                throw new ArgumentException("O denominador não pode ser zero.", nameof(denominator));
            }

            // Normaliza o sinal (o sinal fica sempre no numerador)
            if (denominator < 0)
            {
                numerator = -numerator;
                denominator = -denominator;
            }

            // Simplifica a fração usando o Máximo Divisor Comum (MDC)
            long commonDivisor = Gcd(Math.Abs(numerator), denominator);
            Numerator = numerator / commonDivisor;
            Denominator = denominator / commonDivisor;
        }

        /// <summary>
        /// Calcula o Máximo Divisor Comum (MDC) usando o algoritmo de Euclides.
        /// </summary>
        private static long Gcd(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        /// <summary>
        /// Converte a fração para seu valor de ponto flutuante (double).
        /// </summary>
        public double ToDouble()
        {
            return (double)Numerator / Denominator;
        }

        /// <summary>
        /// Verifica se esta fração é menor que outra fração.
        /// </summary>
        public bool IsLesser(Fraction other)
        {
            // Compara usando multiplicação cruzada para evitar problemas de precisão com double
            return this.Numerator * other.Denominator < other.Numerator * this.Denominator;
        }

        /// <summary>
        /// Verifica se esta fração é maior que outra fração.
        /// </summary>
        public bool IsGreater(Fraction other)
        {
            return this.Numerator * other.Denominator > other.Numerator * this.Denominator;
        }

        /// <summary>
        /// Compara esta fração com outra.
        /// </summary>
        /// <returns>-1 se esta fração for menor, 1 se for maior, 0 se forem iguais.</returns>
        public int CompareTo(Fraction other)
        {
            long crossProduct1 = this.Numerator * other.Denominator;
            long crossProduct2 = other.Numerator * this.Denominator;

            if (crossProduct1 < crossProduct2) return -1;
            if (crossProduct1 > crossProduct2) return 1;
            return 0;
        }

        /// <summary>
        /// Cria uma instância de Fraction a partir de uma string no formato "numerador/denominador".
        /// </summary>
        public static Fraction Parse(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new FormatException("A string de entrada não pode ser nula ou vazia.");
            }

            string[] parts = s.Split('/');
            if (parts.Length != 2)
            {
                throw new FormatException("A fração deve estar no formato 'numerador/denominador'.");
            }

            if (!long.TryParse(parts[0].Trim(), out long numerator) || !long.TryParse(parts[1].Trim(), out long denominator))
            {
                throw new FormatException("Numerador e denominador devem ser números inteiros válidos.");
            }

            return new Fraction(numerator, denominator);
        }

        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }
    }
}