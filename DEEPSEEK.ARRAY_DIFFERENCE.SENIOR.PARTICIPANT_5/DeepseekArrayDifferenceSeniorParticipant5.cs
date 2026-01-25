namespace DEEPSEEK.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_5
{
    public class DeepseekArrayDifferenceSeniorParticipant5
    {
        public static int[] Difference(int[] firstArray, int[] secondArray)
        {
            HashSet<int> secondSet = new HashSet<int>(secondArray);
            List<int> result = new List<int>();

            foreach (int number in firstArray)
            {
                if (!secondSet.Contains(number))
                {
                    result.Add(number);
                }
            }

            return result.ToArray();
        }
    }
}