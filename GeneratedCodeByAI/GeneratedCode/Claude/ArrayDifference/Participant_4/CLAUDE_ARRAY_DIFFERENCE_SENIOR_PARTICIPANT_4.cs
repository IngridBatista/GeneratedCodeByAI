namespace CLAUDE.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_4
{
    public class CLAUDE_ARRAY_DIFFERENCE_SENIOR_PARTICIPANT_4
    {
        public static int[] Difference(int[] a, int[] b)
        {
            return a.Where(x => !b.Contains(x)).ToArray();
        }
    }
}