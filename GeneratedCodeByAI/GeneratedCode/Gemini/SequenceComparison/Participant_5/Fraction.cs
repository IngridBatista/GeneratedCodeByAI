using System;

namespace GEMINI.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_5
{
    // Fraction.cs

    /// <summary>
    /// Representa uma fração com um numerador e um denominador.
    /// Esta classe é imutável.
    /// </summary>
    public sealed class Fraction
    {
        public int Numerator { get; }
        public int Denominator { get; }

        /// <summary>
        /// Inicializa uma nova instância da classe Fraction.
        /// </summary>
        /// <param name="numerator">O numerador da fração.</param>
        /// <param name="denominator">O denominador da fração (não pode ser zero).</param>
        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0)
            {
                throw new ArgumentException("O denominador não pode ser zero.", nameof(denominator));
            }

            Numerator = numerator;
            Denominator = denominator;
        }

        /// <summary>
        /// Converte a fração para seu valor de ponto flutuante (double).
        /// </summary>
        public double ToDouble() => (double)Numerator / Denominator;

        /// <summary>
        /// Verifica se o valor da fração é menor que um determinado número.
        /// </summary>
        public bool IsLesser(double value) => ToDouble() < value;

        /// <summary>
        /// Verifica se o valor da fração é maior que um determinado número.
        /// </summary>
        public bool IsGreater(double value) => ToDouble() > value;

        /// <summary>
        /// Tenta analisar uma string no formato "numerador/denominador" para criar uma Fraction.
        /// </summary>
        /// <param name="input">A string a ser analisada.</param>
        /// <param name="fraction">A fração resultante, se a análise for bem-sucedida.</param>
        /// <returns>True se a análise for bem-sucedida; caso contrário, false.</returns>
        public static bool TryParse(string? input, out Fraction? fraction)
        {
            fraction = null;
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            var parts = input.Split('/');
            if (parts.Length != 2)
            {
                return false;
            }

            if (int.TryParse(parts[0], out int numerator) &&
                int.TryParse(parts[1], out int denominator) &&
                denominator != 0)
            {
                fraction = new Fraction(numerator, denominator);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Retorna a representação da fração em string.
        /// </summary>
        public override string ToString() => $"{Numerator}/{Denominator}";
    }
}