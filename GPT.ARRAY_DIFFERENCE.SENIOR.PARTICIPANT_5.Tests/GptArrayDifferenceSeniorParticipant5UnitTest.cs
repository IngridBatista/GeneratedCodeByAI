using Testes.Compartilhados.Tarefa1;

namespace GPT.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_5.Tests
{
    /// <summary>
    /// Testes da Questão 1 para GPT - Participante 5 (GptArrayDifferenceSeniorParticipant5).
    /// Mesmo critério oficial dos demais participantes: os 9 CasosObrigatorios() +
    /// caso de duplicatas (categoria à parte). Testes de nulo ficam fora do
    /// contrato, já que o enunciado assume argumentos não nulos.
    /// </summary>
    public class GptArrayDifferenceSeniorParticipant5UnitTest
    {
        [Theory]
        [MemberData(nameof(Tarefa1TestData.CasosObrigatorios), MemberType = typeof(Tarefa1TestData))]
        [Trait("Escopo", "Obrigatorio")]
        public void Difference_CasosPadrao(string cenario, int[] a, int[] b, int[] esperado)
        {
            // Act
            var resultado = GptArrayDifferenceSeniorParticipant5.Difference(a, b);

            // Assert
            Assert.True(
                esperado.AsSpan().SequenceEqual(resultado),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", resultado)}]");
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        public void Difference_CasoDuplicatas_ComportamentoObservado()
        {
            var resultado = GptArrayDifferenceSeniorParticipant5.Difference(
                Tarefa1TestData.DuplicatasEntradaA,
                Tarefa1TestData.DuplicatasEntradaB);

            Assert.Equal(Tarefa1TestData.DuplicatasEsperadoPreservandoOcorrencias, resultado);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void Difference_PrimeiroArrayNulo_ComportamentoObservado()
        {
            Action act = () => GptArrayDifferenceSeniorParticipant5.Difference(null, new[] { 1, 5, 7 });
            Assert.Throws<NullReferenceException>(act);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void Difference_SegundoArrayNulo_ComportamentoObservado()
        {
            Action act = () => GptArrayDifferenceSeniorParticipant5.Difference(new[] { 1, 5, 7 }, null);
            Assert.Throws<NullReferenceException>(act);
        }
    }
}
