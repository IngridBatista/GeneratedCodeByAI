using System;
using System.Collections.Generic;
using System.Linq;

namespace CLAUDE.SEQUENCE_COMPARISON.PLENO.PARTICIPANT_6
{
    // Classe para representar frações
    public class Fraction
    {
        public int Numerator { get; private set; }
        public int Denominator { get; private set; }

        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0)
            {
                throw new ArgumentException("Denominador não pode ser zero.");
            }

            Numerator = numerator;
            Denominator = denominator;
        }

        // Retorna o valor decimal da fração
        public double GetValue()
        {
            return (double)Numerator / Denominator;
        }

        // Verifica se a fração é menor que outra
        public bool IsLesser(Fraction other)
        {
            return this.GetValue() < other.GetValue();
        }

        // Verifica se a fração é menor que um valor double
        public bool IsLesser(double value)
        {
            return this.GetValue() < value;
        }

        // Verifica se a fração é maior que outra
        public bool IsGreater(Fraction other)
        {
            return this.GetValue() > other.GetValue();
        }

        // Verifica se a fração é maior que um valor double
        public bool IsGreater(double value)
        {
            return this.GetValue() > value;
        }

        // Verifica se a fração é negativa
        public bool IsNegative()
        {
            return this.GetValue() < 0;
        }

        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }
    }

    public class CompareSequence
    {
        private List<double> sequenceA;
        private List<Fraction> sequenceB;

        public CompareSequence()
        {
            sequenceA = new List<double>();
            sequenceB = new List<Fraction>();
        }

        // Lê a sequência A de valores double
        public void ReadSequenceA()
        {
            Console.WriteLine("Digite os valores da sequência A (digite 0 para encerrar):");

            while (true)
            {
                string input = Console.ReadLine();

                if (double.TryParse(input, out double value))
                {
                    if (value == 0)
                    {
                        break;
                    }
                    sequenceA.Add(value);
                }
                else
                {
                    Console.WriteLine("Valor inválido. Digite um número válido.");
                }
            }
        }

        // Lê a sequência B de frações
        public void ReadSequenceB()
        {
            Console.WriteLine("Digite as frações da sequência B no formato 'numerador/denominador'");
            Console.WriteLine("(a leitura para quando uma fração negativa for inserida):");

            while (true)
            {
                string input = Console.ReadLine();

                try
                {
                    string[] parts = input.Split('/');

                    if (parts.Length != 2)
                    {
                        Console.WriteLine("Formato inválido. Use: numerador/denominador");
                        continue;
                    }

                    if (int.TryParse(parts[0], out int numerator) &&
                        int.TryParse(parts[1], out int denominator))
                    {
                        Fraction fraction = new Fraction(numerator, denominator);

                        if (fraction.IsNegative())
                        {
                            // Fração negativa - encerra a leitura sem incluí-la
                            break;
                        }

                        sequenceB.Add(fraction);
                    }
                    else
                    {
                        Console.WriteLine("Valores inválidos. Digite números inteiros.");
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                }
            }
        }

        // Processa e imprime as frações que atendem ao critério
        public void ProcessAndPrint()
        {
            // Verifica se as sequências estão vazias
            if (sequenceA.Count == 0)
            {
                Console.WriteLine("Erro: A sequência A está vazia. Não é possível continuar.");
                return;
            }

            if (sequenceB.Count == 0)
            {
                Console.WriteLine("Erro: A sequência B está vazia. Não é possível continuar.");
                return;
            }

            // Ordena a sequência A para facilitar a comparação
            List<double> sortedA = sequenceA.OrderBy(x => x).ToList();

            // Calcula o índice da mediana (metade dos valores)
            int halfIndex = sortedA.Count / 2;
            double medianValue = sortedA[halfIndex];

            Console.WriteLine("\nFrações da sequência B maiores que pelo menos metade dos valores de A:");

            bool foundAny = false;
            foreach (Fraction fraction in sequenceB)
            {
                // Conta quantos valores de A são menores que a fração
                int countLesser = sequenceA.Count(a => a < fraction.GetValue());

                // Verifica se a fração é maior que pelo menos metade dos valores
                if (countLesser >= sequenceA.Count / 2.0)
                {
                    Console.WriteLine($"{fraction} = {fraction.GetValue():F4}");
                    foundAny = true;
                }
            }

            if (!foundAny)
            {
                Console.WriteLine("Nenhuma fração atende ao critério.");
            }
        }

        // Método principal para executar todo o processo
        public void Execute()
        {
            ReadSequenceA();
            ReadSequenceB();
            ProcessAndPrint();
        }
    }

    // Programa principal
    class Program
    {
        static void Main(string[] args)
        {
            CompareSequence compareSequence = new CompareSequence();
            compareSequence.Execute();
        }
    }
}