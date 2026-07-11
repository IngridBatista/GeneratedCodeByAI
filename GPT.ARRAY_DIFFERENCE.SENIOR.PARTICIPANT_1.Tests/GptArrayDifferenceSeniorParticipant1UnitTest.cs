using Testes.Compartilhados.Tarefa1;

namespace GPT.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_1.Tests
{
    /// <summary>
    /// Testes da Questão 1 para GPT - Participante 1 (GptArrayDifferenceSeniorParticipant1).
    /// Critério oficial: os 9 CasosObrigatorios() + o caso de duplicatas (categoria à parte).
    /// Testes de nulo NÃO fazem parte do contrato (enunciado assume args não nulos) e
    /// ficam marcados com Trait "ForaDoContrato" — não devem contar na taxa de aprovação.
    /// </summary>
    public class GptArrayDifferenceSeniorParticipant1UnitTest
    {
        [Theory]
        [MemberData(nameof(Tarefa1TestData.CasosObrigatorios), MemberType = typeof(Tarefa1TestData))]
        [Trait("Escopo", "Obrigatorio")]
        public void Diferenca_CasosPadrao(string cenario, int[] a, int[] b, int[] esperado)
        {
            // Act
            var resultado = GptArrayDifferenceSeniorParticipant1.Diferenca(a, b);

            // Assert
            Assert.True(
                esperado.AsSpan().SequenceEqual(resultado),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", resultado)}]");
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        public void Diferenca_CasoDuplicatas_ComportamentoObservado()
        {
            var resultado = GptArrayDifferenceSeniorParticipant1.Diferenca(
                Tarefa1TestData.DuplicatasEntradaA,
                Tarefa1TestData.DuplicatasEntradaB);

            Assert.Equal(Tarefa1TestData.DuplicatasEsperadoPreservandoOcorrencias, resultado);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void Diferenca_PrimeiroArrayNulo_ComportamentoObservado()
        {
            Action act = () => GptArrayDifferenceSeniorParticipant1.Diferenca(null, new[] { 1, 5, 7 });
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void Diferenca_SegundoArrayNulo_ComportamentoObservado()
        {
            Action act = () => GptArrayDifferenceSeniorParticipant1.Diferenca(new[] { 1, 5, 7 }, null);
            Assert.Throws<ArgumentNullException>(act);
        }
    }
}
