using System;
using System.Collections.Generic;
using System.Linq;

namespace GEMINI.SEQUENCE_COMPARISON.PLENO.PARTICIPANT_6
{
    /// <summary>
    /// Lê duas sequências de números (uma de doubles, outra de frações)
    /// e compara os elementos entre elas conforme regras específicas.
    /// </summary>
    public class CompareSequence
    {
        private List<double> sequenceA;
        private List<Fraction> sequenceB;

        public CompareSequence()
        {
            sequenceA = new List<double>();
            sequenceB = new List<Fraction>();
        }

        /// <summary>
        /// Executa o fluxo principal do programa.
        /// </summary>
        public void Run()
        {
            try
            {
                ReadSequenceA();
                ReadSequenceB();

                // Validação das sequências
                if (sequenceA.Count == 0)
                {
                    Console.WriteLine("\nErro: A sequência A está vazia. O programa será encerrado.");
                    return;
                }

                if (sequenceB.Count == 0)
                {
                    Console.WriteLine("\nErro: A sequência B está vazia. O programa será encerrado.");
                    return;
                }

                ProcessAndPrintResults();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nOcorreu um erro inesperado: {ex.Message}");
            }
        }

        /// <summary>
        /// Lê a sequência A de valores double do console.
        /// A leitura para quando o usuário digita 0.
        /// </summary>
        private void ReadSequenceA()
        {
            Console.WriteLine("--- Inserção da Sequência A (números decimais) ---");
            Console.WriteLine("Digite um número por linha. Digite '0' para finalizar.");

            while (true)
            {
                Console.Write("Digite um número para A: ");
                string input = Console.ReadLine();

                if (double.TryParse(input, out double number))
                {
                    if (number == 0)
                    {
                        break; // Condição de parada
                    }
                    sequenceA.Add(number);
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Por favor, digite um número válido.");
                }
            }
            Console.WriteLine("--- Fim da Sequência A ---");
        }

        /// <summary>
        /// Lê a sequência B de frações do console.
        /// A leitura para quando uma fração com valor negativo é inserida.
        /// </summary>
        private void ReadSequenceB()
        {
            Console.WriteLine("\n--- Inserção da Sequência B (frações) ---");
            Console.WriteLine("Digite uma fração no formato 'numerador/denominador'.");
            Console.WriteLine("Digite uma fração com valor negativo (ex: -1/2) para finalizar.");

            while (true)
            {
                Console.Write("Digite uma fração para B: ");
                string input = Console.ReadLine();

                try
                {
                    Fraction fraction = Fraction.Parse(input);

                    if (fraction.ToDouble() < 0)
                    {
                        break; // Condição de parada
                    }
                    sequenceB.Add(fraction);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Erro de formato: {ex.Message}");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Erro de argumento: {ex.Message}");
                }
            }
            Console.WriteLine("--- Fim da Sequência B ---");
        }

        /// <summary>
        /// Processa as sequências e imprime as frações de B que são maiores
        /// que pelo menos metade dos elementos de A.
        /// </summary>
        private void ProcessAndPrintResults()
        {
            Console.WriteLine("\n--- Resultado ---");
            Console.WriteLine("Frações da sequência B maiores que pelo menos metade dos números da sequência A:");

            // O número mínimo de elementos de A que uma fração de B deve superar.
            // Usamos Count / 2.0 para garantir a divisão de ponto flutuante.
            double threshold = sequenceA.Count / 2.0;
            bool foundAny = false;

            foreach (var fraction in sequenceB)
            {
                // Conta quantos números em A são menores que a fração atual.
                // Usamos LINQ para uma contagem concisa.
                int countSmallerInA = sequenceA.Count(numberA -> fraction.IsGreater(numberA));

                if (countSmallerInA >= threshold)
                {
                    Console.WriteLine(fraction);
                    foundAny = true;
                }
            }

            if (!foundAny)
            {
                Console.WriteLine("Nenhuma fração da sequência B atendeu ao critério.");
            }
        }
    }
}