namespace DEEPSEEK.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_3
{
    public class DeepseekSequenceComparisonSeniorParticipant3
    {
        public void Run()
        {
            List<Fraction> A = new List<Fraction>();

            string input;
            while (true)
            {
                input = Console.ReadLine();
                if (input == "0") break;

                string[] parts = input.Split('/');
                if (parts.Length == 2 && int.TryParse(parts[0], out int num) && int.TryParse(parts[1], out int den))
                {
                    A.Add(new Fraction(num, den));
                }
            }

            while (true)
            {
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) break;

                string[] parts = input.Split('/');
                if (parts.Length == 2 && int.TryParse(parts[0], out int num) && int.TryParse(parts[1], out int den))
                {
                    Fraction fraction = new Fraction(num, den);
                    if (fraction.IsGreater(new Fraction(0, 1)) || fraction.IsLesser(new Fraction(0, 1)))
                    {
                        B.Add(fraction);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (A == null || B == null || A.Count == 0 || B.Count == 0)
            {
                Console.WriteLine("Erro: Ambas as listas devem conter valores.");
                return;
            }

            int halfCount = A.Count / 2;
            foreach (Fraction bFraction in B)
            {
                int greaterCount = 0;
                foreach (Fraction aFraction in A)
                {
                    if (bFraction.IsGreater(aFraction))
                    {
                        greaterCount++;
                    }
                }

                if (greaterCount >= halfCount)
                {
                    Console.WriteLine($"{bFraction.Numerator}/{bFraction.Denominator}");
                }
            }
        }
    }
}