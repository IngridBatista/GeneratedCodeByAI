namespace DEEPSEEK.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_4
{
    public class Fraction
    {
        public int Numerator { get; private set; }
        public int Denominator { get; private set; }

        public Fraction(int numerator, int denominator)
        {
            if (denominator == 0)
                throw new ArgumentException("Denominator cannot be zero.");

            Numerator = numerator;
            Denominator = denominator;
        }

        public double ToDouble() => (double)Numerator / Denominator;

        public bool IsLesser(Fraction other) => ToDouble() < other.ToDouble();
        public bool IsGreater(Fraction other) => ToDouble() > other.ToDouble();
        public bool IsLesser(double value) => ToDouble() < value;
        public bool IsGreater(double value) => ToDouble() > value;

        public override string ToString() => $"{Numerator}/{Denominator}";
    }
}