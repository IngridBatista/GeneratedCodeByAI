namespace GEMINI.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_4
{
    // Fraction.cs

    /// <summary>
    /// Representa uma fração com um numerador e um denominador.
    /// Esta classe é imutável.
    /// </summary>
    public class Fraction
    {
        public long Numerator { get; }
        public long Denominator { get; }

        /// <summary>
        /// Inicializa uma nova instância da classe Fraction.
        /// </summary>
        /// <param name="numerator">O numerador da fração.</param>
        /// <param name="denominator">O denominador da fração. Não pode ser zero.</param>
        /// <exception cref="ArgumentException">Lançada se o denominador for zero.</exception>
        public Fraction(long numerator, long denominator)
        {
            if (denominator == 0)
            {
                throw new ArgumentException("O denominador não pode ser zero.", nameof(denominator));
            }

            // Normaliza o sinal para que o denominador seja sempre positivo.
            if (denominator < 0)
            {
                Numerator = -numerator;
                Denominator = -denominator;
            }
            else
            {
                Numerator = numerator;
                Denominator = denominator;
            }
        }

        /// <summary>
        /// Converte a fração para seu valor de ponto flutuante (double).
        /// </summary>
        /// <returns>O valor decimal da fração.</returns>
        public double ToDouble() => (double)Numerator / Denominator;

        /// <summary>
        /// Verifica se o valor da fração é menor que um valor double especificado.
        /// </summary>
        public bool IsLesser(double value) => ToDouble() < value;

        /// <summary>
        /// Verifica se o valor da fração é maior que um valor double especificado.
        /// </summary>
        public bool IsGreater(double value) => ToDouble() > value;

        /// <summary>
        /// Retorna uma representação em string da fração no formato "numerador/denominador".
        /// </summary>
        public override string ToString() => $"{Numerator}/{Denominator}";
    }
}