namespace GEMINI.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_5
{
    // CompareSequence.cs
    public class CompareSequence
    {
        /// <summary>
        /// Executa o fluxo principal do programa: ler sequências, processar e imprimir resultados.
        /// </summary>
        public void Run()
        {
            try
            {
                var sequenceA = ReadDoubleSequence();
                var sequenceB = ReadFractionSequence();

                if (sequenceA.Count == 0 || sequenceB.Count == 0)
                {
                    Console.WriteLine("\nErro: Uma ou ambas as sequências estão vazias. O programa será encerrado.");
                    return;
                }

                var resultFractions = FindFractionsGreaterThanHalfOfSequenceA(sequenceA, sequenceB);

                PrintResults(resultFractions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nOcorreu um erro inesperado: {ex.Message}");
            }
        }

        /// <summary>
        /// Lê uma sequência de doubles do console até que o usuário digite '0'.
        /// </summary>
        private List<double> ReadDoubleSequence()
        {
            Console.WriteLine("--- Sequência A (números decimais) ---");
            Console.WriteLine("Digite números decimais. Digite '0' para finalizar.");

            var sequence = new List<double>();
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine() ?? "";

                if (double.TryParse(input, out double number))
                {
                    if (number == 0)
                    {
                        break;
                    }
                    sequence.Add(number);
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Por favor, digite um número válido.");
                }
            }
            return sequence;
        }

        /// <summary>
        /// Lê uma sequência de frações do console até que uma fração negativa seja inserida.
        /// </summary>
        private List<Fraction> ReadFractionSequence()
        {
            Console.WriteLine("\n--- Sequência B (frações no formato 'numerador/denominador') ---");
            Console.WriteLine("Digite frações (ex: 3/4). Digite uma fração negativa para finalizar.");

            var sequence = new List<Fraction>();
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine() ?? "";

                if (TryParseFraction(input, out Fraction fraction))
                {
                    if (fraction.ToDouble() < 0)
                    {
                        break; // Condição de parada
                    }
                    sequence.Add(fraction);
                }
                else
                {
                    Console.WriteLine("Formato de fração inválido. Use 'numerador/denominador' (ex: 5/2).");
                }
            }
            return sequence;
        }

        /// <summary>
        /// Tenta converter uma string no formato "n/d" para um objeto Fraction.
        /// </summary>
        private bool TryParseFraction(string input, out Fraction fraction)
        {
            fraction = default;
            var parts = input.Split('/');

            if (parts.Length != 2) return false;

            if (int.TryParse(parts[0], out int numerator) && int.TryParse(parts[1], out int denominator))
            {
                try
                {
                    fraction = new Fraction(numerator, denominator);
                    return true;
                }
                catch (ArgumentException) // Captura o erro de denominador zero
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Encontra todas as frações na sequência B que são maiores que pelo menos metade dos elementos da sequência A.
        /// </summary>
        private List<Fraction> FindFractionsGreaterThanHalfOfSequenceA(IReadOnlyList<double> sequenceA, IEnumerable<Fraction> sequenceB)
        {
            var result = new List<Fraction>();
            int threshold = sequenceA.Count / 2;

            foreach (var fraction in sequenceB)
            {
                // Usando LINQ para uma contagem limpa e expressiva
                int countGreater = sequenceA.Count(number => fraction.IsGreater(number));

                if (countGreater > threshold)
                {
                    result.Add(fraction);
                }
            }
            return result;
        }

        /// <summary>
        /// Imprime a lista de frações resultante no console.
        /// </summary>
        private void PrintResults(IReadOnlyList<Fraction> fractions)
        {
            Console.WriteLine("\n--- Resultado ---");
            if (fractions.Count == 0)
            {
                Console.WriteLine("Nenhuma fração da sequência B atendeu ao critério.");
            }
            else
            {
                Console.WriteLine("Frações da sequência B maiores que pelo menos metade dos números da sequência A:");
                foreach (var fraction in fractions)
                {
                    Console.WriteLine(fraction.ToString());
                }
            }
        }
    }
}