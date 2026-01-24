using System;
using System.Collections.Generic;

namespace GEMINI.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_3
{
    public class Program
    {
        // Lista para armazenar os valores inteiros de A
        private readonly List<int> A = new List<int>();
        // Lista para armazenar as frações de B
        private readonly List<Fraction> B = new List<Fraction>();

        public void Run()
        {
            // 1. Leitura dos valores para a lista A
            Console.WriteLine("Digite os valores inteiros para a lista A (digite 0 para parar):");
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int valueA))
                {
                    if (valueA == 0)
                    {
                        break; // Encerra o loop se o valor for 0
                    }
                    A.Add(valueA);
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Por favor, digite um número inteiro.");
                }
            }

            // 2. Leitura dos valores para a lista B
            Console.WriteLine("\nDigite as frações para a lista B no formato 'numerador denominador' (uma fração negativa para parar):");
            while (true)
            {
                string input = Console.ReadLine();
                string[] parts = input.Split(' ');

                if (parts.Length != 2 || !int.TryParse(parts[0], out int num) || !int.TryParse(parts[1], out int den))
                {
                    Console.WriteLine("Formato inválido. Use 'numerador denominador' (ex: 3 4).");
                    continue;
                }

                // Condição de parada: valor da fração < 0
                // Assumindo denominador positivo, basta o numerador ser negativo.
                if (num < 0)
                {
                    break;
                }

                if (den == 0)
                {
                    Console.WriteLine("Erro: O denominador não pode ser zero.");
                    continue;
                }

                B.Add(new Fraction(num, den));
            }

            // 3. Validação das listas
            if (A.Count == 0)
            {
                Console.WriteLine("\nErro: A lista A não pode ser vazia.");
                return;
            }

            if (B.Count == 0)
            {
                Console.WriteLine("\nErro: A lista B não pode ser vazia.");
                return;
            }

            // 4. Processamento e exibição do resultado
            Console.WriteLine("\nFrações em B que são maiores que pelo menos metade dos valores em A:");
            foreach (var fractionB in B)
            {
                int countGreaterThan = 0;
                foreach (var valueA in A)
                {
                    // Converte o inteiro de A em uma fração para comparar (ex: 5 se torna 5/1)
                    var fractionA = new Fraction(valueA, 1);
                    if (fractionB.IsGreater(fractionA))
                    {
                        countGreaterThan++;
                    }
                }

                // Verifica se a contagem é maior ou igual à metade do tamanho da lista A
                if (countGreaterThan >= A.Count / 2.0)
                {
                    Console.WriteLine(fractionB);
                }
            }
        }
    }

    public class Fraction
    {
        public int Numerator { get; }
        public int Denominator { get; }

        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0)
            {
                throw new ArgumentException("O denominador não pode ser zero.");
            }
            Numerator = numerator;
            Denominator = denominator;
        }

        public bool IsGreater(Fraction other)
        {
            // Para evitar problemas de precisão com ponto flutuante, usamos multiplicação cruzada.
            // a/b > c/d  é o mesmo que  a*d > c*b (para denominadores positivos)
            // Usamos 'long' para evitar overflow de inteiros com números grandes.
            return (long)this.Numerator * other.Denominator > (long)other.Numerator * this.Denominator;
        }

        public bool IsLesser(Fraction other)
        {
            return (long)this.Numerator * other.Denominator < (long)other.Numerator * this.Denominator;
        }

        public override string ToString()
        {
            return $"{Numerator}/{Denominator}";
        }
    }

    // Para executar o programa, você precisaria de um ponto de entrada (Main)
    // Exemplo:
    // public static class EntryPoint
    // {
    //     public static void Main(string[] args)
    //     {
    //         Program p = new Program();
    //         p.Run();
    //     }
    // }
}