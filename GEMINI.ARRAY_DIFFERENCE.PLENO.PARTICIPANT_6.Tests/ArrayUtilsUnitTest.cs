using Testes.Compartilhados.Tarefa1;

namespace GEMINI.ARRAY_DIFFERENCE.PLENO.PARTICIPANT_6.Tests
{
    /// <summary>
    /// Testes da Questão 1 para Gemini - Participante 6, nível Pleno (ArrayUtils.ElementosUnicosEmA).
    ///
    /// ATENÇÃO - NÃO CONFORMIDADE COM O ENUNCIADO:
    /// O enunciado pede "Difference"; esta implementação nomeia o método como "ElementosUnicosEmA".
    ///
    /// ATENÇÃO - VERIFICAR NUMERAÇÃO:
    /// O namespace de origem informa PARTICIPANT_6 (nível Pleno). Caso a numeração pretendida
    /// para esta submissão fosse "Participante 7", confirme e renomeie este arquivo/classe
    /// antes de consolidar a tabela de resultados.
    /// </summary>
    public class ArrayUtilsUnitTest
    {
        [Fact]
        [Trait("Escopo", "NaoConformidadeDeAssinatura")]
        public void ElementosUnicosEmA_NomeDoMetodoDivergeDoEnunciado()
        {
            const string nomeEsperado = "Difference";
            const string nomeImplementado = "ElementosUnicosEmA";

            Assert.NotEqual(nomeEsperado, nomeImplementado);
        }

        [Theory]
        [MemberData(nameof(Tarefa1TestData.CasosObrigatorios), MemberType = typeof(Tarefa1TestData))]
        [Trait("Escopo", "Obrigatorio")]
        public void ElementosUnicosEmA_CasosPadrao(string cenario, int[] a, int[] b, int[] esperado)
        {
            var resultado = ArrayUtils.ElementosUnicosEmA(a, b);

            Assert.True(
                esperado.AsSpan().SequenceEqual(resultado),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", resultado)}]");
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        public void ElementosUnicosEmA_CasoDuplicatas_ComportamentoObservado()
        {
            var resultado = ArrayUtils.ElementosUnicosEmA(
                Tarefa1TestData.DuplicatasEntradaA,
                Tarefa1TestData.DuplicatasEntradaB);

            Assert.Equal(Tarefa1TestData.DuplicatasEsperadoDeduplicado, resultado);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void ElementosUnicosEmA_PrimeiroArrayNulo_ComportamentoObservado()
        {
            Action act = () => ArrayUtils.ElementosUnicosEmA(null, new[] { 1, 5, 7 });
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void ElementosUnicosEmA_SegundoArrayNulo_ComportamentoObservado()
        {
            Action act = () => ArrayUtils.ElementosUnicosEmA(new[] { 1, 5, 7 }, null);
            Assert.Throws<ArgumentNullException>(act);
        }
    }
}
