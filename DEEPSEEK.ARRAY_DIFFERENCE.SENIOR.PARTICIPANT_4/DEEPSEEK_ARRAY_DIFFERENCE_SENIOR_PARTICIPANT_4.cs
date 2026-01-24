namespace DEEPSEEK.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_4
{
    public class DEEPSEEK_ARRAY_DIFFERENCE_SENIOR_PARTICIPANT_4
    {
        public static int[] Difference(int[] a, int[] b)
        {
            // Usando HashSet para busca O(1) dos elementos de b
            HashSet<int> setB = new HashSet<int>(b);

            // Filtrando elementos de a que não estão em b
            List<int> result = new List<int>();

            foreach (int num in a)
            {
                if (!setB.Contains(num))
                {
                    result.Add(num);
                }
            }

            return result.ToArray();
        }
    }
}