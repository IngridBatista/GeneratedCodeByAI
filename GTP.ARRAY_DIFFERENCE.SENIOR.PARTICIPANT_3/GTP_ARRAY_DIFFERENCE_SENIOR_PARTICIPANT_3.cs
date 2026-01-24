namespace GTP.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_3
{
    public class GTP_ARRAY_DIFFERENCE_SENIOR_PARTICIPANT_3
    {
        public static int[] Difference(int[] a, int[] b)
        {
            var conjuntoB = new HashSet<int>(b);
            var resultado = new List<int>();

            foreach (var valor in a)
            {
                if (!conjuntoB.Contains(valor))
                    resultado.Add(valor);
            }

            return resultado.ToArray();
        }
    }
}