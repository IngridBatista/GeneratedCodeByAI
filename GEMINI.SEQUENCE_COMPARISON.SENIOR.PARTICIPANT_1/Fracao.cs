using System;

namespace GEMINI.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_1
{
    // Fracao.cs
    public class Fracao
    {
        // Propriedades com set privado para garantir imutabilidade após a criação
        public int Numerador { get; private set; }
        public int Denominador { get; private set; }

        /// <summary>
        /// Construtor da classe Fracao.
        /// </summary>
        /// <param name="numerador">O numerador da fração.</param>
        /// <param name="denominador">O denominador da fração (não pode ser zero).</param>
        public Fracao(int numerador, int denominador)
        {
            if (denominador == 0)
            {
                throw new ArgumentException("O denominador não pode ser zero.");
            }
            Numerador = numerador;
            Denominador = denominador;
        }

        /// <summary>
        /// Converte a fração para seu valor decimal (double).
        /// </summary>
        public double ToDouble()
        {
            // A conversão para (double) é crucial para evitar a divisão de inteiros
            return (double)Numerador / Denominador;
        }

        /// <summary>
        /// Verifica se esta fração é menor que outra fração.
        /// </summary>
        /// <param name="outra">A outra fração para comparar.</param>
        /// <returns>True se esta fração for menor, caso contrário False.</returns>
        public bool IsLesser(Fracao outra)
        {
            return this.ToDouble() < outra.ToDouble();
        }

        /// <summary>
        /// Verifica se esta fração é maior que outra fração.
        /// </summary>
        /// <param name="outra">A outra fração para comparar.</param>
        /// <returns>True se esta fração for maior, caso contrário False.</returns>
        public bool IsGreater(Fracao outra)
        {
            return this.ToDouble() > outra.ToDouble();
        }

        /// <summary>
        /// Retorna a representação da fração em formato de string.
        /// </summary>
        public override string ToString()
        {
            return $"{Numerador}/{Denominador}";
        }
    }
}