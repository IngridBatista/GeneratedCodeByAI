using System;

namespace GEMINI.SEQUENCE_COMPARISON.PLENO.PARTICIPANT_6
{
    /// <summary>
    /// Representa um número racional (fração) com um numerador e um denominador.
    /// </summary>
    public class Fraction
    {
        /// <summary>
        /// Obtém o numerador da fração.
        /// </summary>
        public long Numerator { get; private set; }

        /// <summary>
        /// Obtém o denominador da fração.
        /// </summary>
        public long Denominator { get; private set; }

        /// <summary>
        /// Inicializa uma nova instância da classe Fraction.
        /// </summary>
        /// <param name="numerator">O numerador.</param>
        /// <param name="denominator">O denominador (não pode ser zero).</param>
        public Fraction(long numerator, long denominator)
        {
            if (denominator == 0)
            {
                throw new ArgumentException("O denominador não pode ser zero.");
            }

            // Garante que o sinal da fração fique sempre no numerador
            if (denominator < 0)
            {
                numerator = -numerator;
                denominator = -denominator;
            }

            Numerator = numerator;
            Denominator = denominator;
        }

        /// <summary>
        /// Converte a fração para seu valor de ponto flutuante (double).
        /// </summary>
        /// <returns>O valor decimal da fração.</returns>
        public double ToDouble()
        {
            return (double)Numerator / Denominator;
        }

        /// <summary>
        /// Verifica se o valor desta fração é maior que um determinado valor double.
        /// </summary>
        public bool IsGreater(double value)
        {
            return this.ToDouble() > value;
        }

        /// <summary>
        /// Verifica se o valor desta fração é menor que um determinado valor double.
        /// </summary>
        public bool IsLesser(double value)
        {
            return this.ToDouble() < value;
        }

        /// <summary>
        /// Converte uma string no formato "numerador/denominador" para um objeto Fraction.
        /// </summary>
        /// <param name="input">A string a ser convertida.</param>
        /// <returns>Um objeto Fraction.</returns>
        public static Fraction Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new FormatException("A entrada não pode ser nula ou vazia.");
            }

            string[] parts = input.Split('/');
            if (parts.Length != 2)
            {
                throw new FormatException("Formato de fração inválido. Use 'numerador/denominador'.");
            }

            if (!long.TryParse(parts[0].Trim(), out long numerator) || !long.TryParse(parts[1].Trim(), out long denominator))
            {
                throw new FormatException("Numerador e denominador devem ser números inteiros válidos.");
            }

            return new Fraction(numerator, denominator);
        }

        /// <summary>
        /// Retorna a representação em string da fração.
        /// </summary>
        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }
    }
}