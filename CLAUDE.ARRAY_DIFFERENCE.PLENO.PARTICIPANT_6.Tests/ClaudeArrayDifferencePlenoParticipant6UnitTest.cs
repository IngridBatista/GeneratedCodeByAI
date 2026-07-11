using Testes.Compartilhados.Tarefa1;

namespace CLAUDE.ARRAY_DIFFERENCE.PLENO.PARTICIPANT_6.Tests
{
    /// <summary>
    /// Testes da Questão 1 para Claude - Participante 6, nível Pleno (ArrayDiff).
    ///
    /// ATENÇÃO - NÃO CONFORMIDADE COM O ENUNCIADO:
    /// O enunciado pede "Difference"; esta implementação nomeia o método como "ArrayDiff".
    /// Logicamente idêntica ao padrão HashSet + foreach visto nos Participantes 3 e 5.
    /// </summary>
    public class ClaudeArrayDifferencePlenoParticipant6UnitTest
    {
        [Fact]
        [Trait("Escopo", "NaoConformidadeDeAssinatura")]
        public void ArrayDiff_NomeDoMetodoDivergeDoEnunciado()
        {
            const string nomeEsperado = "Difference";
            const string nomeImplementado = "ArrayDiff";

            Assert.NotEqual(nomeEsperado, nomeImplementado);
        }

        [Theory]
        [MemberData(nameof(Tarefa1TestData.CasosObrigatorios), MemberType = typeof(Tarefa1TestData))]
        [Trait("Escopo", "Obrigatorio")]
        public void ArrayDiff_CasosPadrao(string cenario, int[] a, int[] b, int[] esperado)
        {
            var resultado = ClaudeArrayDifferencePlenoParticipant6.ArrayDiff(a, b);

            Assert.True(
                esperado.AsSpan().SequenceEqual(resultado),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", resultado)}]");
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        public void ArrayDiff_CasoDuplicatas_ComportamentoObservado()
        {
            var resultado = ClaudeArrayDifferencePlenoParticipant6.ArrayDiff(
                Tarefa1TestData.DuplicatasEntradaA,
                Tarefa1TestData.DuplicatasEntradaB);

            Assert.Equal(Tarefa1TestData.DuplicatasEsperadoPreservandoOcorrencias, resultado);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void ArrayDiff_PrimeiroArrayNulo_ComportamentoObservado()
        {
            Action act = () => ClaudeArrayDifferencePlenoParticipant6.ArrayDiff(null, new[] { 1, 5, 7 });
            Assert.Throws<NullReferenceException>(act);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void ArrayDiff_SegundoArrayNulo_ComportamentoObservado()
        {
            Action act = () => ClaudeArrayDifferencePlenoParticipant6.ArrayDiff(new[] { 1, 5, 7 }, null);
            Assert.Throws<ArgumentNullException>(act);
        }
    }
}
