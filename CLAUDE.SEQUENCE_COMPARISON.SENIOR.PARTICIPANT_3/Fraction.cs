namespace CLAUDE.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_3
{
    private readonly List<Fraction> B = new List<Fraction>();

    public void Run()
    {
        List<int> A = new List<int>();

        while (true)
        {
            string input = Console.ReadLine();
            int value = int.Parse(input);
            if (value == 0) break;
            A.Add(value);
        }

        while (true)
        {
            string[] parts = Console.ReadLine().Split('/');
            Fraction fraction = new Fraction(int.Parse(parts[0]), int.Parse(parts[1]));
            if (fraction.IsLesser(new Fraction(0, 1))) break;
            B.Add(fraction);
        }

        if (A == null || A.Count == 0)
        {
            Console.WriteLine("Erro: Lista A está vazia ou nula");
            return;
        }

        if (B == null || B.Count == 0)
        {
            Console.WriteLine("Erro: Lista B está vazia ou nula");
            return;
        }

        int halfCount = A.Count / 2;

        foreach (Fraction fractionB in B)
        {
            int greaterCount = 0;

            foreach (int valueA in A)
            {
                Fraction fractionA = new Fraction(valueA, 1);
                if (fractionB.IsGreater(fractionA))
                {
                    greaterCount++;
                }
            }

            if (greaterCount >= halfCount)
            {
                Console.WriteLine($"{fractionB.Numerator}/{fractionB.Denominator}");
            }
        }
    }

    public class Fraction
    {
        public int Numerator { get; }
        public int Denominator { get; }

        public Fraction(int numerator, int denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        public bool IsLesser(Fraction other)
        {
            return Numerator * other.Denominator < other.Numerator * Denominator;
        }

        public bool IsGreater(Fraction other)
        {
            return Numerator * other.Denominator > other.Numerator * Denominator;
        }
    }
}