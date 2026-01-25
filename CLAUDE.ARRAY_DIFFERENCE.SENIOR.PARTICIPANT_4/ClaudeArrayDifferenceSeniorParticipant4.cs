namespace CLAUDE.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_4
{
    public class ClaudeArrayDifferenceSeniorParticipant4
    {
        public static int[] Difference(int[] a, int[] b)
        {
            return a.Where(x => !b.Contains(x)).ToArray();
        }
    }
}