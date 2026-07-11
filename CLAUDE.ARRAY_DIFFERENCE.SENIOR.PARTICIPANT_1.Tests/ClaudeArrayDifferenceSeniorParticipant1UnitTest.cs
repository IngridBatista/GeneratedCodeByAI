using Testes.Compartilhados.Tarefa1;

namespace CLAUDE.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_1.Tests
{
    /// <summary>
    /// Testes da Questão 1 para Claude - Participante 1 (ObterElementosExclusivos).
    ///
    /// ATENÇÃO - NÃO CONFORMIDADE COM O ENUNCIADO:
    /// O enunciado pede "Difference"; esta implementação nomeia o método como
    /// "ObterElementosExclusivos".
    ///
    /// ATENÇÃO - COMPORTAMENTO ÚNICO COM NULO:
    /// Diferente de todos os demais participantes analisados até agora (que lançam
    /// ArgumentNullException ou NullReferenceException), esta implementação NUNCA
    /// lança exceção para argumentos nulos. Em vez disso:
    ///   - array1 nulo  -> retorna array vazio
    ///   - array2 nulo  -> retorna array1 original sem nenhuma remoção
    ///   - ambos nulos  -> retorna array vazio
    /// Isso é uma escolha de design defensiva (fail-safe), mas não é uma validação
    /// no sentido tradicional (não sinaliza erro ao chamador).
    /// </summary>
    public class ClaudeArrayDifferenceSeniorParticipant1UnitTest
    {
        [Fact]
        [Trait("Escopo", "NaoConformidadeDeAssinatura")]
        public void ObterElementosExclusivos_NomeDoMetodoDivergeDoEnunciado()
        {
            const string nomeEsperado = "Difference";
            const string nomeImplementado = "ObterElementosExclusivos";

            Assert.NotEqual(nomeEsperado, nomeImplementado);
        }

        [Theory]
        [MemberData(nameof(Tarefa1TestData.CasosObrigatorios), MemberType = typeof(Tarefa1TestData))]
        [Trait("Escopo", "Obrigatorio")]
        public void ObterElementosExclusivos_CasosPadrao(string cenario, int[] a, int[] b, int[] esperado)
        {
            var resultado = ClaudeArrayDifferenceSeniorParticipant1.ObterElementosExclusivos(a, b);

            Assert.True(
                esperado.AsSpan().SequenceEqual(resultado),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", resultado)}]");
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        public void ObterElementosExclusivos_CasoDuplicatas_ComportamentoObservado()
        {
            var resultado = ClaudeArrayDifferenceSeniorParticipant1.ObterElementosExclusivos(
                Tarefa1TestData.DuplicatasEntradaA,
                Tarefa1TestData.DuplicatasEntradaB);

            Assert.Equal(Tarefa1TestData.DuplicatasEsperadoDeduplicado, resultado);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void ObterElementosExclusivos_PrimeiroArrayNulo_RetornaArrayVazio()
        {
            var resultado = ClaudeArrayDifferenceSeniorParticipant1.ObterElementosExclusivos(null, new[] { 1, 5, 7 });

            Assert.Empty(resultado);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void ObterElementosExclusivos_SegundoArrayNulo_RetornaPrimeiroArrayOriginal()
        {
            var a = new[] { 1, 5, 7 };
            var resultado = ClaudeArrayDifferenceSeniorParticipant1.ObterElementosExclusivos(a, null);

            Assert.Equal(a, resultado);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void ObterElementosExclusivos_AmbosArraysNulos_RetornaArrayVazio()
        {
            var resultado = ClaudeArrayDifferenceSeniorParticipant1.ObterElementosExclusivos(null, null);

            Assert.Empty(resultado);
        }
    }
}
