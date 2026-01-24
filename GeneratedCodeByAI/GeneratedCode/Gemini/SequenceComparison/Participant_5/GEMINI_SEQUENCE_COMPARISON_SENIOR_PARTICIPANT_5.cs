namespace GEMINI.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_5
{
    public class GEMINI_SEQUENCE_COMPARISON_SENIOR_PARTICIPANT_5
    {
        // Fraction.cs

        /// <summary>
        /// Representa um número racional (fração) de forma imutável.
        /// A fração é sempre armazenada em sua forma mais simples.
        /// </summary>
        public readonly struct Fraction : IComparable<Fraction>
        {
            public int Numerator { get; }
            public int Denominator { get; }

            public Fraction(int numerator, int denominator)
            {
                if (denominator == 0)
                {
                    throw new ArgumentException("O denominador não pode ser zero.", nameof(denominator));
                }

                // Garante que o sinal da fração fique apenas no numerador
                if (denominator < 0)
                {
                    numerator = -numerator;
                    denominator = -denominator;
                }

                // Simplifica a fração
                int commonDivisor = Gcd(Math.Abs(numerator), denominator);
                Numerator = numerator / commonDivisor;
                Denominator = denominator / commonDivisor;
            }

            public double ToDouble() => (double)Numerator / Denominator;

            public bool IsGreater(double value) => ToDouble() > value;

            public bool IsLesser(double value) => ToDouble() < value;

            public override string ToString() => $"{Numerator}/{Denominator}";

            // Implementação de IComparable para permitir comparações diretas
            public int CompareTo(Fraction other)
            {
                // Usando multiplicação cruzada para evitar imprecisões de ponto flutuante
                long crossProduct1 = (long)this.Numerator * other.Denominator;
                long crossProduct2 = (long)other.Numerator * this.Denominator;
                return crossProduct1.CompareTo(crossProduct2);
            }

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
}