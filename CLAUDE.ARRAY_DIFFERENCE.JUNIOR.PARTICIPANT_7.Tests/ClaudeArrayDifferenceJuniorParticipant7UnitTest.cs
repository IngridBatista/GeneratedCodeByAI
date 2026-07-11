using Testes.Compartilhados.Tarefa1;

namespace CLAUDE.ARRAY_DIFFERENCE.JUNIOR.PARTICIPANT_7.Tests
{
    /// <summary>
    /// Testes da Questão 1 para Claude - Participante 7, nível Junior
    /// (ClaudeArrayDifferenceJuniorParticipant7.Difference).
    /// Nome do método conforme o enunciado. Inclui validação explícita de nulo
    /// (terceiro caso entre todos os participantes analisados a fazer isso,
    /// junto com GPT Participante 1 e DeepSeek Participante 7) e early-returns
    /// para arrays vazios. Usa Where + HashSet -> preserva duplicatas.
    /// </summary>
    public class ClaudeArrayDifferenceJuniorParticipant7UnitTest
    {
        [Theory]
        [MemberData(nameof(Tarefa1TestData.CasosObrigatorios), MemberType = typeof(Tarefa1TestData))]
        [Trait("Escopo", "Obrigatorio")]
        public void Difference_CasosPadrao(string cenario, int[] a, int[] b, int[] esperado)
        {
            var resultado = ClaudeArrayDifferenceJuniorParticipant7.Difference(a, b);

            Assert.True(
                esperado.AsSpan().SequenceEqual(resultado),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", resultado)}]");
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        public void Difference_CasoDuplicatas_ComportamentoObservado()
        {
            // Where + HashSet -> PRESERVA ocorrências.
            var resultado = ClaudeArrayDifferenceJuniorParticipant7.Difference(
                Tarefa1TestData.DuplicatasEntradaA,
                Tarefa1TestData.DuplicatasEntradaB);

            Assert.Equal(Tarefa1TestData.DuplicatasEsperadoPreservandoOcorrencias, resultado);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void Difference_PrimeiroArrayNulo_DeveLancarArgumentNullException()
        {
            Action act = () => ClaudeArrayDifferenceJuniorParticipant7.Difference(null, new[] { 1, 5, 7 });
            var ex = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("a", ex.ParamName);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void Difference_SegundoArrayNulo_DeveLancarArgumentNullException()
        {
            Action act = () => ClaudeArrayDifferenceJuniorParticipant7.Difference(new[] { 1, 5, 7 }, null);
            var ex = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("b", ex.ParamName);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void Difference_AmbosArraysNulos_DeveLancarArgumentNullException()
        {
            Action act = () => ClaudeArrayDifferenceJuniorParticipant7.Difference(null, null);
            var ex = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("a", ex.ParamName);
        }
    }
}
