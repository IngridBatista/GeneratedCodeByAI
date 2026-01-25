namespace GPT.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_1
{
    public class GptArrayDifferenceSeniorParticipant1
    {
        public static int[] Diferenca(int[] primeiro, int[] segundo)
        {
            if (primeiro == null) throw new ArgumentNullException(nameof(primeiro));
            if (segundo == null) throw new ArgumentNullException(nameof(segundo));

            // Coloca os elementos do segundo array em um HashSet para busca rápida
            var conjuntoSegundo = new HashSet<int>(segundo);

            // Filtra apenas os elementos do primeiro que não estão no segundo
            return primeiro
                .Where(x => !conjuntoSegundo.Contains(x))
                .ToArray();
        }
    }
}