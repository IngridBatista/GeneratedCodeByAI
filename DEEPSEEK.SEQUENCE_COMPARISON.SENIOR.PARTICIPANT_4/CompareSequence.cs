namespace DEEPSEEK.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_4
{
    public class CompareSequence
    {
        public static void Execute()
        {
            try
            {
                // 1. Ler sequência A de doubles
                Console.WriteLine("Enter sequence A (double values, enter 0 to finish):");
                List<double> sequenceA = ReadDoubleSequence();

                // 2. Ler sequência B de frações
                Console.WriteLine("\nEnter sequence B (fractions as numerator/denominator, enter negative fraction to finish):");
                List<Fraction> sequenceB = ReadFractionSequence();

                // 3. Validar sequências não vazias
                if (sequenceA.Count == 0 || sequenceB.Count == 0)
                {
                    Console.WriteLine("Error: Both sequences must contain at least one element.");
                    return;
                }

                // 4. Calcular metade dos números em A
                double halfOfA = sequenceA.Count / 2.0;

                // 5. Encontrar e imprimir frações que atendem ao critério
                Console.WriteLine("\nFractions from sequence B greater than at least half of numbers in sequence A:");
                bool foundAny = false;

                foreach (var fraction in sequenceB)
                {
                    int countGreater = 0;

                    foreach (var number in sequenceA)
                    {
                        if (fraction.IsGreater(number))
                        {
                            countGreater++;
                        }
                    }

                    if (countGreater >= halfOfA)
                    {
                        Console.WriteLine($"{fraction} (value: {fraction.ToDouble():F4})");
                        foundAny = true;
                    }
                }

                if (!foundAny)
                {
                    Console.WriteLine("No fractions meet the criteria.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static List<double> ReadDoubleSequence()
        {
            List<double> sequence = new List<double>();

            while (true)
            {
                Console.Write("Enter a double value: ");
                string input = Console.ReadLine()?.Trim();

                if (double.TryParse(input, out double value))
                {
                    if (value == 0)
                        break;

                    sequence.Add(value);
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid double value.");
                }
            }

            return sequence;
        }

        private static List<Fraction> ReadFractionSequence()
        {
            List<Fraction> sequence = new List<Fraction>();

            while (true)
            {
                Console.Write("Enter fraction (format: numerator/denominator): ");
                string input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Invalid input. Please enter a fraction in format 'numerator/denominator'.");
                    continue;
                }

                string[] parts = input.Split('/');

                if (parts.Length != 2 ||
                    !int.TryParse(parts[0], out int numerator) ||
                    !int.TryParse(parts[1], out int denominator))
                {
                    Console.WriteLine("Invalid format. Use 'numerator/denominator' (e.g., 3/4).");
                    continue;
                }

                try
                {
                    Fraction fraction = new Fraction(numerator, denominator);

                    // Verificar se a fração é negativa (critério de parada)
                    if (fraction.ToDouble() < 0)
                        break;

                    sequence.Add(fraction);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            return sequence;
        }
    }
}