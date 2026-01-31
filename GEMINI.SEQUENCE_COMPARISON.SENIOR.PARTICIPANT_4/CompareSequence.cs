namespace GEMINI.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_4
{
    // CompareSequence.cs

    /// <summary>
    /// Orquestra a leitura de sequências, a comparação e a impressão dos resultados.
    /// </summary>
    public class CompareSequence
    {
        private readonly List<double> _sequenceA = new();
        private readonly List<Fraction> _sequenceB = new();

        /// <summary>
        /// Executa o fluxo completo do programa.
        /// </summary>
        public void Run()
        {
            ReadSequenceA();
            ReadSequenceB();

            if (_sequenceA.Count == 0)
            {
                Console.WriteLine("Erro: A sequência A não pode ser vazia.");
                return;
            }

            if (_sequenceB.Count == 0)
            {
                Console.WriteLine("Erro: A sequência B não pode ser vazia.");
                return;
            }

            ProcessAndPrintResults();
        }

        private void ReadSequenceA()
        {
            Console.WriteLine("Insira os valores da sequência A (tipo double). Digite 0 para terminar.");
            while (true)
            {
                Console.Write("Valor: ");
                string? input = Console.ReadLine();

                if (!double.TryParse(input, out double value))
                {
                    Console.WriteLine("Entrada inválida. Por favor, insira um número double válido.");
                    continue;
                }

                if (value == 0)
                {
                    break;
                }

                _sequenceA.Add(value);
            }
        }

        private void ReadSequenceB()
        {
            Console.WriteLine("\nInsira os valores da sequência B (frações no formato 'numerador/denominador').");
            Console.WriteLine("Uma fração negativa (ex: -1/2) encerrará a leitura.");
            while (true)
            {
                Console.Write("Fração: ");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Entrada inválida.");
                    continue;
                }

                string[] parts = input.Split('/');
                if (parts.Length != 2 || !long.TryParse(parts[0], out long numerator) || !long.TryParse(parts[1], out long denominator))
                {
                    Console.WriteLine("Formato inválido. Use 'numerador/denominador'.");
                    continue;
                }

                try
                {
                    var fraction = new Fraction(numerator, denominator);

                    if (fraction.ToDouble() < 0)
                    {
                        break; // Condição de parada
                    }

                    _sequenceB.Add(fraction);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                }
            }
        }

        private void ProcessAndPrintResults()
        {
            Console.WriteLine("\n--- Resultados ---");
            Console.WriteLine("Frações da sequência B maiores que pelo menos metade dos valores da sequência A:");

            // O limiar é metade do número de elementos em A.
            // Usamos A.Count / 2.0 para garantir a divisão de ponto flutuante.
            double thresholdCount = _sequenceA.Count / 2.0;

            var resultFractions = _sequenceB
                .Where(fractionB => _sequenceA.Count(valueA => fractionB.IsGreater(valueA)) >= thresholdCount)
                .ToList();

            if (resultFractions.Any())
            {
                foreach (var fraction in resultFractions)
                {
                    Console.WriteLine(fraction.ToString());
                }
            }
            else
            {
                Console.WriteLine("Nenhuma fração encontrada que satisfaça a condição.");
            }
        }
    }
}