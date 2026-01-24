using System;

namespace GEMINI.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_1
{
    // Program.cs
    class Program
    {
        static void Main(string[] args)
        {
            // Define a cultura para "pt-BR" para garantir que o ponto seja o separador decimal
            // Isso evita problemas se o sistema operacional estiver em um idioma que usa vírgula.
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("pt-BR");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt-BR");

            Console.WriteLine("Bem-vindo ao Comparador de Sequências!");

            ComparadorDeSequencias comparador = new ComparadorDeSequencias();
            comparador.Executar();

            Console.WriteLine("\nPressione qualquer tecla para sair...");
            Console.ReadKey();
        }
    }
}