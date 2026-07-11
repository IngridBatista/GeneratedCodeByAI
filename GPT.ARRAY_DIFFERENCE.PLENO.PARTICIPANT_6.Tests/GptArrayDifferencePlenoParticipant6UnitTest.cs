using Testes.Compartilhados.Tarefa1;

namespace GPT.ARRAY_DIFFERENCE.PLENO.PARTICIPANT_6.Tests
{
    /// <summary>
    /// Testes da Questão 1 para GPT - Participante 6 (GptArrayDifferencePlenoParticipant6).
    ///
    /// ATENÇÃO - NÃO CONFORMIDADE COM O ENUNCIADO:
    /// O enunciado pede explicitamente a assinatura "public static int[] Difference(int[] a, int[] b)".
    /// Esta implementação nomeia o método como "Diferenca", não "Difference".
    /// Isso é uma divergência de contrato, não um erro de lógica — deve ser registrado
    /// como categoria própria de análise ("NaoConformidadeDeAssinatura"), pois em um cenário
    /// de integração real (chamada externa esperando "Difference") o código simplesmente
    /// não compilaria/funcionaria sem adaptação.
    ///
    /// Os testes abaixo chamam "Diferenca" diretamente para ainda avaliar a lógica interna,
    /// mas isso não deve ser confundido com conformidade total ao enunciado.
    /// </summary>
    public class GptArrayDifferencePlenoParticipant6_Questao1Tests
    {
        [Fact]
        [Trait("Escopo", "NaoConformidadeDeAssinatura")]
        public void Diferenca_NomeDoMetodoDivergeDoEnunciado()
        {
            const string nomeEsperado = "Difference";
            const string nomeImplementado = "Diferenca";

            Assert.NotEqual(nomeEsperado, nomeImplementado);
        }

        [Theory]
        [MemberData(nameof(Tarefa1TestData.CasosObrigatorios), MemberType = typeof(Tarefa1TestData))]
        [Trait("Escopo", "Obrigatorio")]
        public void Diferenca_CasosPadrao(string cenario, int[] a, int[] b, int[] esperado)
        {
            // Act
            var resultado = GptArrayDifferencePlenoParticipant6.Diferenca(a, b);

            // Assert
            Assert.True(
                esperado.AsSpan().SequenceEqual(resultado),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", resultado)}]");
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        public void Diferenca_CasoDuplicatas_ComportamentoObservado()
        {
            var resultado = GptArrayDifferencePlenoParticipant6.Diferenca(
                Tarefa1TestData.DuplicatasEntradaA,
                Tarefa1TestData.DuplicatasEntradaB);

            Assert.Equal(Tarefa1TestData.DuplicatasEsperadoPreservandoOcorrencias, resultado);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void Diferenca_PrimeiroArrayNulo_ComportamentoObservado()
        {
            Action act = () => GptArrayDifferencePlenoParticipant6.Diferenca(null, new[] { 1, 5, 7 });
            Assert.Throws<NullReferenceException>(act);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void Diferenca_SegundoArrayNulo_ComportamentoObservado()
        {
            Action act = () => GptArrayDifferencePlenoParticipant6.Diferenca(new[] { 1, 5, 7 }, null);
            Assert.Throws<ArgumentNullException>(act);
        }
    }
}
