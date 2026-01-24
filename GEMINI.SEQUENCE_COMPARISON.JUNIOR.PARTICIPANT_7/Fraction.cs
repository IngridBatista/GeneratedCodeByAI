using System;

namespace GEMINI.SEQUENCE_COMPARISON.JUNIOR.PARTICIPANT_7
{
    // Fraction.cs

    /// <summary>
    /// Representa um número racional (fração) de forma imutável.
    /// A fração é sempre armazenada em sua forma mais simples.
    /// </summary>
    public class Fraction : IComparable<Fraction>, IComparable<double>
    {
        /// <summary>
        /// O numerador da fração.
        /// </summary>
        public int Numerator { get; }

        /// <summary>
        /// O denominador da fração.
        /// </summary>
        public int Denominator { get; }

        /// <summary>
        /// Inicializa uma nova instância da classe Fraction.
        /// </summary>
        /// <param name="numerator">O numerador.</param>
        /// <param name="denominator">O denominador (não pode ser zero).</param>
        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0)
            {
                throw new ArgumentException("O denominador não pode ser zero.", nameof(denominator));
            }

            // Garante que o sinal negativo fique sempre no numerador
            if (denominator < 0)
            {
                numerator *= -1;
                denominator *= -1;
            }

            // Simplifica a fração encontrando o máximo divisor comum (MDC)
            int commonDivisor = Gcd(Math.Abs(numerator), denominator);
            Numerator = numerator / commonDivisor;
            Denominator = denominator / commonDivisor;
        }

        /// <summary>
        /// Converte a fração para seu valor em double.
        /// </summary>
        public double ToDouble() => (double)Numerator / Denominator;

        /// <summary>
        /// Verifica se esta fração é maior que outra.
        /// </summary>
        public bool IsGreater(Fraction other) => this.CompareTo(other) > 0;

        /// <summary>
        /// Verifica se esta fração é menor que outra.
        /// </summary>
        public bool IsLesser(Fraction other) => this.CompareTo(other) < 0;

        /// <summary>
        /// Verifica se esta fração é maior que um valor double.
        /// </summary>
        public bool IsGreater(double value) => this.CompareTo(value) > 0;

        /// <summary>
        /// Verifica se esta fração é menor que um valor double.
        /// </summary>
        public bool IsLesser(double value) => this.CompareTo(value) < 0;

        /// <summary>
        /// Compara a instância atual com outro objeto Fraction.
        /// Usa multiplicação cruzada para evitar imprecisões de ponto flutuante.
        /// </summary>
        public int CompareTo(Fraction? other)
        {
            if (other is null) return 1;
            // a/b > c/d  <=>  a*d > c*b
            long crossProduct1 = (long)this.Numerator * other.Denominator;
            long crossProduct2 = (long)other.Numerator * this.Denominator;
            return crossProduct1.CompareTo(crossProduct2);
        }

        /// <summary>
        /// Compara a instância atual com um valor double.
        /// </summary>
        public int CompareTo(double other)
        {
            return this.ToDouble().CompareTo(other);
        }

        /// <summary>
        /// Retorna a representação em string da fração.
        /// </summary>
        public override string ToString() => $"{Numerator}/{Denominator}";

        /// <summary>
        /// Calcula o Máximo Divisor Comum (MDC) usando o algoritmo de Euclides.
        /// </summary>
        private static int Gcd(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
    }
}