using Testes.Compartilhados.Tarefa1;

namespace CLAUDE.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_4.Tests
{
    /// <summary>
    /// Testes da Questão 1 para Claude - Participante 4 (ClaudeArrayDifferenceSeniorParticipant4.Difference).
    /// Nome do método conforme o enunciado. Usa LINQ Where + Contains (sem HashSet,
    /// portanto O(n*m)) -> preserva duplicatas, mesmo padrão de comportamento com
    /// nulo do GPT Participante 1 (a.Where valida a; b.Contains valida b).
    /// </summary>
    public class ClaudeArrayDifferenceSeniorParticipant4UnitTest
    {
        [Theory]
        [MemberData(nameof(Tarefa1TestData.CasosObrigatorios), MemberType = typeof(Tarefa1TestData))]
        [Trait("Escopo", "Obrigatorio")]
        public void Difference_CasosPadrao(string cenario, int[] a, int[] b, int[] esperado)
        {
            var resultado = ClaudeArrayDifferenceSeniorParticipant4.Difference(a, b);

            Assert.True(
                esperado.AsSpan().SequenceEqual(resultado),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", resultado)}]");
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        public void Difference_CasoDuplicatas_ComportamentoObservado()
        {
            var resultado = ClaudeArrayDifferenceSeniorParticipant4.Difference(
                Tarefa1TestData.DuplicatasEntradaA,
                Tarefa1TestData.DuplicatasEntradaB);

            Assert.Equal(Tarefa1TestData.DuplicatasEsperadoPreservandoOcorrencias, resultado);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void Difference_PrimeiroArrayNulo_ComportamentoObservado()
        {
            Action act = () => ClaudeArrayDifferenceSeniorParticipant4.Difference(null, new[] { 1, 5, 7 });
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void Difference_SegundoArrayNulo_ComportamentoObservado()
        {
            Action act = () => ClaudeArrayDifferenceSeniorParticipant4.Difference(new[] { 1, 5, 7 }, null);
            Assert.Throws<ArgumentNullException>(act);
        }
    }
}
