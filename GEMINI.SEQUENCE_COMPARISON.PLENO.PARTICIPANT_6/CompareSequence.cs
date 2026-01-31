using System;
using System.Collections.Generic;
using System.Linq;

namespace GEMINI.SEQUENCE_COMPARISON.PLENO.PARTICIPANT_6
{
    // CompareSequence.cs

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

                if (!ValidateSequences())
                {
                    return; // A validação já imprimiu a mensagem de erro.
                }

                ProcessAndPrintResults();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nOcorreu um erro inesperado: {ex.Message}");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Lê a sequência A de valores double do console.
        /// </summary>
        private void ReadSequenceA()
        {
            Console.WriteLine("--- Inserção da Sequência A (números decimais) ---");
            Console.WriteLine("Digite um número por linha. Digite '0' para finalizar.");

            while (true)
            {
                Console.Write("Digite um número: ");
                string input = Console.ReadLine();

                if (double.TryParse(input, out double value))
                {
                    if (value == 0)
                    {
                        break; // Condição de parada
                    }
                    sequenceA.Add(value);
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Por favor, digite um número válido.");
                }
            }
            Console.WriteLine("--- Fim da Sequência A ---\n");
        }

        /// <summary>
        /// Lê a sequência B de frações do console.
        /// </summary>
        private void ReadSequenceB()
        {
            Console.WriteLine("--- Inserção da Sequência B (frações) ---");
            Console.WriteLine("Digite uma fração no formato a/b. Uma fração negativa para finalizar.");

            while (true)
            {
                Console.Write("Digite uma fração: ");
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
                    Console.WriteLine($"Formato inválido: {ex.Message}");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                }
            }
            Console.WriteLine("--- Fim da Sequência B ---\n");
        }

        /// <summary>
        /// Valida se as sequências não estão vazias.
        /// </summary>
        /// <returns>True se ambas as sequências são válidas, False caso contrário.</returns>
        private bool ValidateSequences()
        {
            if (sequenceA.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Erro: A sequência A está vazia. A execução será interrompida.");
                Console.ResetColor();
                return false;
            }

            if (sequenceB.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Erro: A sequência B está vazia. A execução será interrompida.");
                Console.ResetColor();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Processa as sequências e imprime as frações que atendem ao critério.
        /// </summary>
        private void ProcessAndPrintResults()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("--- Resultado ---");
            Console.WriteLine("Frações da sequência B maiores que pelo menos metade dos valores em A:");
            Console.ResetColor();

            // O critério é ser maior que PELO MENOS metade, então o número de elementos
            // que a fração deve superar é A.Count / 2.0.
            double requiredCount = sequenceA.Count / 2.0;
            bool foundAny = false;

            foreach (Fraction fraction in sequenceB)
            {
                // Usando LINQ para contar quantos números em A são menores que a fração atual.
                int countOfSmallerNumbers = sequenceA.Count(numberA => fraction.ToDouble() > numberA);

                if (countOfSmallerNumbers >= requiredCount)
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