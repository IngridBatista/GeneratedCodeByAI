using System;

namespace GEMINI.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_4
{
    /// <summary>
    /// Representa uma fração com um numerador e um denominador.
    /// A fração é sempre mantida na sua forma mais simples e com sinal no numerador.
    /// </summary>
    public sealed class Fraction : IComparable<Fraction>, IEquatable<Fraction>
    {
        public long Numerator { get; }
        public long Denominator { get; }

        public Fraction(long numerator, long denominator)
        {
            if (denominator == 0)
            {
                throw new DivideByZeroException("O denominador não pode ser zero.");
            }

            // Garante que o sinal da fração esteja sempre no numerador.
            if (denominator < 0)
            {
                numerator = -numerator;
                denominator = -denominator;
            }

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

        public double ToDouble() => (double)Numerator / Denominator;

        public override string ToString() => $"{Numerator}/{Denominator}";

        // Sobrecarga de operadores para comparação com double
        public static bool operator >(Fraction a, double b) => a.ToDouble() > b;
        public static bool operator <(Fraction a, double b) => a.ToDouble() < b;
        public static bool operator >=(Fraction a, double b) => a.ToDouble() >= b;
        public static bool operator <=(Fraction a, double b) => a.ToDouble() <= b;

        // Implementação de interfaces para consistência
        public int CompareTo(Fraction? other)
        {
            if (other is null) return 1;
            // Comparação cruzada para evitar imprecisão de ponto flutuante
            return (this.Numerator * other.Denominator).CompareTo(other.Numerator * this.Denominator);
        }

        public bool Equals(Fraction? other)
        {
            if (other is null) return false;
            return this.Numerator == other.Numerator && this.Denominator == other.Denominator;
        }

        public override bool Equals(object? obj) => obj is Fraction other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(Numerator, Denominator);
    }
}