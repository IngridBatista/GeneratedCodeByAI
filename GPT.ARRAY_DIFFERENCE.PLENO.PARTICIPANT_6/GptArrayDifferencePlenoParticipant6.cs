namespace GPT.ARRAY_DIFFERENCE.PLENO.PARTICIPANT_6
{
    public class GptArrayDifferencePlenoParticipant6
    {
        public static int[] Diferenca(int[] a, int[] b)
        {
            // Coloca todos os elementos de b em um conjunto para busca O(1)
            var conjuntoB = new HashSet<int>(b);

            // Lista temporária para armazenar os elementos de a que não estão em b
            var resultado = new List<int>();

            foreach (var x in a)
            {
                if (!conjuntoB.Contains(x))
                {
                    resultado.Add(x);
                }
            }

            return resultado.ToArray();
        }
    }
}