using System;
using System.Collections.Generic;

namespace DEEPSEEK.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_1
{
    // Classe para representar uma fração
    public class Fracao
    {
        public int Numerador { get; private set; }
        public int Denominador { get; private set; }
        public double Valor => (double)Numerador / Denominador;

        public Fracao(int numerador, int denominador)
        {
            if (denominador == 0)
                throw new ArgumentException("Denominador não pode ser zero");

            Numerador = numerador;
            Denominador = denominador;
        }

        // Método para verificar se esta fração é menor que outra
        public bool IsLesser(Fracao outra)
        {
            return this.Valor < outra.Valor;
        }

        // Método para verificar se esta fração é maior que outra
        public bool IsGreater(Fracao outra)
        {
            return this.Valor > outra.Valor;
        }

        public override string ToString()
        {
            return $"{Numerador}/{Denominador}";
        }
    }

    // Classe principal para comparar sequências
    public class ComparadorSequencias
    {
        private List<double> sequenciaA;
        private List<Fracao> sequenciaB;

        public ComparadorSequencias()
        {
            sequenciaA = new List<double>();
            sequenciaB = new List<Fracao>();
        }

        // Método para ler as sequências do console
        public void LerSequencias()
        {
            Console.WriteLine("=== LEITURA DA SEQUÊNCIA A ===");
            Console.WriteLine("Digite valores double (digite 0 para finalizar a sequência A):");

            bool lendoA = true;
            bool lendoB = false;

            while (true)
            {
                Console.Write("> ");
                string entrada = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(entrada))
                    continue;

                if (lendoA)
                {
                    // Tentar converter para double
                    if (double.TryParse(entrada, out double valor))
                    {
                        if (valor == 0)
                        {
                            Console.WriteLine("\nZero digitado! Agora digite os valores para a sequência B (no formato numerador/denominador).");
                            Console.WriteLine("Digite uma fração com valor negativo para finalizar.");
                            lendoA = false;
                            lendoB = true;
                            continue;
                        }

                        sequenciaA.Add(valor);
                        Console.WriteLine($"Valor {valor} adicionado à sequência A.");
                    }
                    else
                    {
                        Console.WriteLine("Erro: Digite um valor double válido para a sequência A.");
                    }
                }
                else if (lendoB)
                {
                    // Verificar se é uma fração no formato correto
                    if (entrada.Contains("/"))
                    {
                        string[] partes = entrada.Split('/');

                        if (partes.Length == 2 &&
                            int.TryParse(partes[0], out int numerador) &&
                            int.TryParse(partes[1], out int denominador))
                        {
                            try
                            {
                                Fracao fracao = new Fracao(numerador, denominador);

                                // Verificar se o valor da fração é negativo
                                if (fracao.Valor < 0)
                                {
                                    Console.WriteLine($"Fração {fracao} tem valor negativo ({fracao.Valor:F2}). Finalizando leitura.");
                                    break;
                                }

                                sequenciaB.Add(fracao);
                                Console.WriteLine($"Fração {fracao} (valor: {fracao.Valor:F2}) adicionada à sequência B.");
                            }
                            catch (ArgumentException ex)
                            {
                                Console.WriteLine($"Erro: {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Erro: Formato inválido. Use numerador/denominador (ex: 3/4)");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Erro: Formato inválido. Use numerador/denominador (ex: 3/4)");
                    }
                }
            }
        }

        // Método para comparar as sequências
        public void CompararSequencias()
        {
            Console.WriteLine("\n=== RESULTADO DA COMPARAÇÃO ===");

            // Verificar se as sequências estão vazias
            if (sequenciaA.Count == 0)
            {
                Console.WriteLine("ERRO: Sequência A está vazia. O programa será encerrado.");
                Console.WriteLine("Motivo: Nenhum valor double foi inserido antes do zero.");
                return;
            }

            if (sequenciaB.Count == 0)
            {
                Console.WriteLine("ERRO: Sequência B está vazia. O programa será encerrado.");
                Console.WriteLine("Motivo: A primeira fração inserida já tinha valor negativo ou nenhuma fração válida foi inserida.");
                return;
            }

            // Exibir as sequências
            Console.WriteLine($"Sequência A ({sequenciaA.Count} elementos):");
            foreach (var valor in sequenciaA)
            {
                Console.Write($"{valor:F2} ");
            }

            Console.WriteLine($"\n\nSequência B ({sequenciaB.Count} elementos):");
            foreach (var fracao in sequenciaB)
            {
                Console.Write($"{fracao} ({fracao.Valor:F2}) ");
            }

            Console.WriteLine("\n\n=== COMPARAÇÃO DETALHADA ===");

            // Comparar elementos correspondentes
            int minLength = Math.Min(sequenciaA.Count, sequenciaB.Count);

            for (int i = 0; i < minLength; i++)
            {
                double valorA = sequenciaA[i];
                Fracao fracaoB = sequenciaB[i];
                double valorB = fracaoB.Valor;

                Console.Write($"Posição {i + 1}: A={valorA:F2}, B={fracaoB} ({valorB:F2}) -> ");

                if (valorA > valorB)
                {
                    Console.WriteLine("A > B");
                }
                else if (valorA < valorB)
                {
                    Console.WriteLine("A < B");
                }
                else
                {
                    Console.WriteLine("A = B");
                }
            }

            // Informar sobre elementos restantes
            if (sequenciaA.Count > sequenciaB.Count)
            {
                Console.WriteLine($"\nAtenção: Sequência A tem {sequenciaA.Count - sequenciaB.Count} elementos a mais que B.");
            }
            else if (sequenciaB.Count > sequenciaA.Count)
            {
                Console.WriteLine($"\nAtenção: Sequência B tem {sequenciaB.Count - sequenciaA.Count} elementos a mais que A.");
            }

            // Comparação usando os métodos da classe Fracao
            Console.WriteLine("\n=== COMPARAÇÃO USANDO MÉTODOS DA CLASSE FRAÇÃO ===");

            if (minLength > 0)
            {
                // Criar uma fração a partir do primeiro valor de A para demonstração
                // (convertendo para fração aproximada)
                int numeradorAprox = (int)(sequenciaA[0] * 100);
                Fracao fracaoAprox = new Fracao(numeradorAprox, 100);
                Fracao primeiraFracaoB = sequenciaB[0];

                Console.WriteLine($"Comparando primeira posição:");
                Console.WriteLine($"  Aproximação de A como fração: {fracaoAprox} ({fracaoAprox.Valor:F2})");
                Console.WriteLine($"  B: {primeiraFracaoB} ({primeiraFracaoB.Valor:F2})");

                if (fracaoAprox.IsLesser(primeiraFracaoB))
                    Console.WriteLine($"  Resultado: A é menor que B (usando IsLesser)");
                else if (fracaoAprox.IsGreater(primeiraFracaoB))
                    Console.WriteLine($"  Resultado: A é maior que B (usando IsGreater)");
                else
                    Console.WriteLine($"  Resultado: A é igual a B");
            }
        }

        // Método principal para executar o programa
        public void Executar()
        {
            Console.WriteLine("=== PROGRAMA DE COMPARAÇÃO DE SEQUÊNCIAS ===");
            Console.WriteLine("Instruções:");
            Console.WriteLine("1. Primeiro, digite valores double para a sequência A");
            Console.WriteLine("2. Digite 0 para finalizar a sequência A");
            Console.WriteLine("3. Depois, digite frações no formato numerador/denominador para a sequência B");
            Console.WriteLine("4. Digite uma fração com valor negativo para finalizar");
            Console.WriteLine();

            LerSequencias();
            CompararSequencias();
        }
    }

    // Classe principal do programa
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ComparadorSequencias comparador = new ComparadorSequencias();
                comparador.Executar();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
            }

            Console.WriteLine("\nPressione qualquer tecla para sair...");
            Console.ReadKey();
        }
    }
}