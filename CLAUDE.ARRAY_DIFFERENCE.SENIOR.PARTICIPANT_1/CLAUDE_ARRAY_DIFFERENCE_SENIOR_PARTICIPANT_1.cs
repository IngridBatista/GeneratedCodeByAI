namespace CLAUDE.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_1
{
    public class CLAUDE_ARRAY_DIFFERENCE_SENIOR_PARTICIPANT_1
    {
        public static int[] ObterElementosExclusivos(int[] array1, int[] array2)
        {
            if (array1 == null || array2 == null)
                return array1 ?? new int[0];

            return array1.Except(array2).ToArray();
        }
    }
}