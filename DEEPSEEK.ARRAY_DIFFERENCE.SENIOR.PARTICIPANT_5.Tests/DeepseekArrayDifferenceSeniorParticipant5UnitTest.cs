using Testes.Compartilhados.Tarefa1;

namespace DEEPSEEK.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_5.Tests
{
    /// <summary>
    /// Testes da Questão 1 para DeepSeek - Participante 5 (DeepseekArrayDifferenceSeniorParticipant5.Difference).
    /// Estruturalmente idêntico aos Participantes 3 e 4: HashSet + foreach, nome conforme o enunciado.
    /// </summary>
    public class DeepseekArrayDifferenceSeniorParticipant5UnitTest
    {
        [Theory]
        [MemberData(nameof(Tarefa1TestData.CasosObrigatorios), MemberType = typeof(Tarefa1TestData))]
        [Trait("Escopo", "Obrigatorio")]
        public void Difference_CasosPadrao(string cenario, int[] a, int[] b, int[] esperado)
        {
            var resultado = DeepseekArrayDifferenceSeniorParticipant5.Difference(a, b);

            Assert.True(
                esperado.AsSpan().SequenceEqual(resultado),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", resultado)}]");
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        public void Difference_CasoDuplicatas_ComportamentoObservado()
        {
            var resultado = DeepseekArrayDifferenceSeniorParticipant5.Difference(
                Tarefa1TestData.DuplicatasEntradaA,
                Tarefa1TestData.DuplicatasEntradaB);

            Assert.Equal(Tarefa1TestData.DuplicatasEsperadoPreservandoOcorrencias, resultado);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void Difference_PrimeiroArrayNulo_ComportamentoObservado()
        {
            Action act = () => DeepseekArrayDifferenceSeniorParticipant5.Difference(null, new[] { 1, 5, 7 });
            Assert.Throws<NullReferenceException>(act);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void Difference_SegundoArrayNulo_ComportamentoObservado()
        {
            Action act = () => DeepseekArrayDifferenceSeniorParticipant5.Difference(new[] { 1, 5, 7 }, null);
            Assert.Throws<ArgumentNullException>(act);
        }
    }
}
