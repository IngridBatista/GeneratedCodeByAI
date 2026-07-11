using Testes.Compartilhados.Tarefa1;

namespace DEEPSEEK.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_3.Tests
{
    /// <summary>
    /// Testes da Questão 1 para DeepSeek - Participante 3 (DeepseekArrayDifferenceSeniorParticipant3.Difference).
    /// Nome do método conforme o enunciado. Usa HashSet + foreach -> mesmo padrão
    /// de preservação de duplicatas observado nos demais participantes que usam essa abordagem.
    /// </summary>
    public class DeepseekArrayDifferenceSeniorParticipant3UnitTest
    {
        [Theory]
        [MemberData(nameof(Tarefa1TestData.CasosObrigatorios), MemberType = typeof(Tarefa1TestData))]
        [Trait("Escopo", "Obrigatorio")]
        public void Difference_CasosPadrao(string cenario, int[] a, int[] b, int[] esperado)
        {
            var resultado = DeepseekArrayDifferenceSeniorParticipant3.Difference(a, b);

            Assert.True(
                esperado.AsSpan().SequenceEqual(resultado),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", resultado)}]");
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        public void Difference_CasoDuplicatas_ComportamentoObservado()
        {
            var resultado = DeepseekArrayDifferenceSeniorParticipant3.Difference(
                Tarefa1TestData.DuplicatasEntradaA,
                Tarefa1TestData.DuplicatasEntradaB);

            Assert.Equal(Tarefa1TestData.DuplicatasEsperadoPreservandoOcorrencias, resultado);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void Difference_PrimeiroArrayNulo_ComportamentoObservado()
        {
            Action act = () => DeepseekArrayDifferenceSeniorParticipant3.Difference(null, new[] { 1, 5, 7 });
            Assert.Throws<NullReferenceException>(act);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void Difference_SegundoArrayNulo_ComportamentoObservado()
        {
            Action act = () => DeepseekArrayDifferenceSeniorParticipant3.Difference(new[] { 1, 5, 7 }, null);
            Assert.Throws<ArgumentNullException>(act);
        }
    }
}
