using System;
using System.Collections.Generic;
using System.Linq;

namespace GEMINI.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_1
{
    // ComparadorDeSequencias.cs
    public class ComparadorDeSequencias
    {
        public List<double> SequenciaA { get; private set; }
        public List<Fracao> SequenciaB { get; private set; }

        public ComparadorDeSequencias()
        {
            SequenciaA = new List<double>();
            SequenciaB = new List<Fracao>();
        }

        /// <summary>
        /// Método principal para executar todo o processo.
        /// </summary>
        public void Executar()
        {
            LerSequenciaA();

            // Verifica se a Sequência A ficou vazia
            if (SequenciaA.Count == 0)
            {
                Console.WriteLine("\nPrograma encerrado: A Sequência A está vazia porque o primeiro valor digitado foi zero.");
                return;
            }

            Console.WriteLine("\n--- Sequência A finalizada. Agora, insira os valores para a Sequência B. ---");

            LerSequenciaB();

            // Verifica se a Sequência B ficou vazia
            if (SequenciaB.Count == 0)
            {
                Console.WriteLine("\nPrograma encerrado: A Sequência B está vazia porque o primeiro valor inserido foi uma fração negativa.");
                return;
            }

            CompararSequencias();
        }

        /// <summary>
        /// Lê os valores da Sequência A (double) do console.
        /// </summary>
        private void LerSequenciaA()
        {
            Console.WriteLine("--- Início da Leitura da Sequência A (valores double) ---");
            Console.WriteLine("Digite um número por vez. Digite '0' para finalizar.");

            while (true)
            {
                Console.Write("Valor para A: ");
                string input = Console.ReadLine();

                if (double.TryParse(input, out double valor))
                {
                    if (valor == 0)
                    {
                        break; // Encerra a leitura para a Sequência A
                    }
                    SequenciaA.Add(valor);
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Por favor, digite um número double válido.");
                }
            }
        }

        /// <summary>
        /// Lê os valores da Sequência B (frações) do console.
        /// </summary>
        private void LerSequenciaB()
        {
            Console.WriteLine("--- Início da Leitura da Sequência B (frações no formato numerador/denominador) ---");
            Console.WriteLine("Digite uma fração por vez. Uma fração com valor menor que zero encerrará a leitura.");

            while (true)
            {
                Console.Write("Valor para B (ex: 3/4): ");
                string input = Console.ReadLine();

                string[] partes = input.Split('/');

                if (partes.Length != 2 || !int.TryParse(partes[0], out int num) || !int.TryParse(partes[1], out int den))
                {
                    Console.WriteLine("Formato inválido. Use 'numerador/denominador' com números inteiros.");
                    continue;
                }

                try
                {
                    Fracao fracao = new Fracao(num, den);

                    if (fracao.ToDouble() < 0)
                    {
                        Console.WriteLine("Fração com valor negativo detectada. Leitura da Sequência B encerrada.");
                        break; // Encerra a leitura para a Sequência B
                    }

                    SequenciaB.Add(fracao);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Compara as sequências A e B e exibe os resultados.
        /// </summary>
        private void CompararSequencias()
        {
            Console.WriteLine("\n================== RESULTADO DA COMPARAÇÃO ==================");
            Console.WriteLine($"Sequência A: [ {string.Join(", ", SequenciaA)} ]");
            Console.WriteLine($"Sequência B: [ {string.Join(", ", SequenciaB)} ]");
            Console.WriteLine("-------------------------------------------------------------");

            int tamanhoMinimo = Math.Min(SequenciaA.Count, SequenciaB.Count);

            Console.WriteLine("Comparação item a item (até o limite da menor sequência):");
            for (int i = 0; i < tamanhoMinimo; i++)
            {
                double valorA = SequenciaA[i];
                Fracao fracaoB = SequenciaB[i];
                double valorB = fracaoB.ToDouble();

                string resultado;
                if (valorA > valorB)
                {
                    resultado = "maior que";
                }
                else if (valorA < valorB)
                {
                    resultado = "menor que";
                }
                else
                {
                    resultado = "igual a";
                }
                Console.WriteLine($"A[{i}] ({valorA:F2}) é {resultado} B[{i}] ({fracaoB} = {valorB:F2})");
            }

            if (SequenciaA.Count != SequenciaB.Count)
            {
                Console.WriteLine("\nAs sequências têm tamanhos diferentes. A comparação foi feita até o final da menor.");
            }

            Console.WriteLine("-------------------------------------------------------------");
            Console.WriteLine("Comparação da soma total:");
            double somaA = SequenciaA.Sum();
            double somaB = SequenciaB.Sum(f => f.ToDouble());

            string resultadoSoma;
            if (somaA > somaB)
            {
                resultadoSoma = "maior que";
            }
            else if (somaA < somaB)
            {
                resultadoSoma = "menor que";
            }
            else
            {
                resultadoSoma = "igual a";
            }

            Console.WriteLine($"Soma de A ({somaA:F2}) é {resultadoSoma} a soma de B ({somaB:F2}).");
            Console.WriteLine("=============================================================");
        }
    }
}