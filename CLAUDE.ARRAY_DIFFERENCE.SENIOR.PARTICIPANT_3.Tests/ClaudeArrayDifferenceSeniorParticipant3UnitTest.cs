using Testes.Compartilhados.Tarefa1;

namespace CLAUDE.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_3.Tests
{
    /// <summary>
    /// Testes da Questão 1 para Claude - Participante 3 (ClaudeArrayDifferenceSeniorParticipant3.Difference).
    ///
    /// ATENÇÃO - VERIFICAR NUMERAÇÃO:
    /// O namespace de origem informa PARTICIPANT_3. Caso a numeração pretendida
    /// para esta submissão fosse "Participante 2", confirme e renomeie este
    /// arquivo/classe antes de consolidar a tabela de resultados.
    ///
    /// Nome do método conforme o enunciado. Usa HashSet + foreach -> mesmo padrão
    /// canônico observado repetidamente em GPT e DeepSeek.
    /// </summary>
    public class ClaudeArrayDifferenceSeniorParticipant3UnitTest
    {
        [Theory]
        [MemberData(nameof(Tarefa1TestData.CasosObrigatorios), MemberType = typeof(Tarefa1TestData))]
        [Trait("Escopo", "Obrigatorio")]
        public void Difference_CasosPadrao(string cenario, int[] a, int[] b, int[] esperado)
        {
            var resultado = ClaudeArrayDifferenceSeniorParticipant3.Difference(a, b);

            Assert.True(
                esperado.AsSpan().SequenceEqual(resultado),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", resultado)}]");
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        public void Difference_CasoDuplicatas_ComportamentoObservado()
        {
            var resultado = ClaudeArrayDifferenceSeniorParticipant3.Difference(
                Tarefa1TestData.DuplicatasEntradaA,
                Tarefa1TestData.DuplicatasEntradaB);

            Assert.Equal(Tarefa1TestData.DuplicatasEsperadoPreservandoOcorrencias, resultado);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void Difference_PrimeiroArrayNulo_ComportamentoObservado()
        {
            Action act = () => ClaudeArrayDifferenceSeniorParticipant3.Difference(null, new[] { 1, 5, 7 });
            Assert.Throws<NullReferenceException>(act);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void Difference_SegundoArrayNulo_ComportamentoObservado()
        {
            Action act = () => ClaudeArrayDifferenceSeniorParticipant3.Difference(new[] { 1, 5, 7 }, null);
            Assert.Throws<ArgumentNullException>(act);
        }
    }
}
