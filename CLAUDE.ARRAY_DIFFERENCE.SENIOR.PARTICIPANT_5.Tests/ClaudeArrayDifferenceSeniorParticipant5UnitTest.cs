using Testes.Compartilhados.Tarefa1;

namespace CLAUDE.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_5.Tests
{
    /// <summary>
    /// Testes da Questão 1 para Claude - Participante 5 (ClaudeArrayDifferenceSeniorParticipant5.Difference).
    /// Estruturalmente idêntico ao Participante 3: HashSet + foreach, nome conforme o enunciado.
    /// </summary>
    public class ClaudeArrayDifferenceSeniorParticipant5UnitTest
    {
        [Theory]
        [MemberData(nameof(Tarefa1TestData.CasosObrigatorios), MemberType = typeof(Tarefa1TestData))]
        [Trait("Escopo", "Obrigatorio")]
        public void Difference_CasosPadrao(string cenario, int[] a, int[] b, int[] esperado)
        {
            var resultado = ClaudeArrayDifferenceSeniorParticipant5.Difference(a, b);

            Assert.True(
                esperado.AsSpan().SequenceEqual(resultado),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", resultado)}]");
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        public void Difference_CasoDuplicatas_ComportamentoObservado()
        {
            // HashSet + foreach -> PRESERVA ocorrências.
            var resultado = ClaudeArrayDifferenceSeniorParticipant5.Difference(
                Tarefa1TestData.DuplicatasEntradaA,
                Tarefa1TestData.DuplicatasEntradaB);

            Assert.Equal(Tarefa1TestData.DuplicatasEsperadoPreservandoOcorrencias, resultado);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void Difference_PrimeiroArrayNulo_ComportamentoObservado()
        {
            Action act = () => ClaudeArrayDifferenceSeniorParticipant5.Difference(null, new[] { 1, 5, 7 });
            Assert.Throws<NullReferenceException>(act);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void Difference_SegundoArrayNulo_ComportamentoObservado()
        {
            Action act = () => ClaudeArrayDifferenceSeniorParticipant5.Difference(new[] { 1, 5, 7 }, null);
            Assert.Throws<ArgumentNullException>(act);
        }
    }
}
