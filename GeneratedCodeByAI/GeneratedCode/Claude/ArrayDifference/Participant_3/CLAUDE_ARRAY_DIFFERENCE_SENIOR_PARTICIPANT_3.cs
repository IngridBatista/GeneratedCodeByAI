namespace CLAUDE.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_3
{
    public class CLAUDE_ARRAY_DIFFERENCE_SENIOR_PARTICIPANT_3
    {
        public static int[] Difference(int[] a, int[] b)
        {
            HashSet<int> setB = new HashSet<int>(b);
            List<int> result = new List<int>();

            foreach (int element in a)
            {
                if (!setB.Contains(element))
                {
                    result.Add(element);
                }
            }

            return result.ToArray();
        }
    }
}