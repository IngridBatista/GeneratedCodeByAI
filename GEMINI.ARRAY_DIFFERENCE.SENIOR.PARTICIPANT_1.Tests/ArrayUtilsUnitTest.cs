using Testes.Compartilhados.Tarefa1;

namespace GEMINI.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_1.Tests
{
    /// <summary>
    /// Testes da Questão 1 para Gemini - Participante 1 (ArrayUtils.ElementosExclusivos).
    ///
    /// ATENÇÃO - NÃO CONFORMIDADE COM O ENUNCIADO:
    /// O enunciado pede a assinatura "public static int[] Difference(int[] a, int[] b)".
    /// Esta implementação nomeia o método como "ElementosExclusivos".
    ///
    /// ATENÇÃO - COMPORTAMENTO DE DUPLICATAS DIVERGENTE:
    /// Esta implementação usa LINQ .Except(), que por definição retorna elementos
    /// DISTINTOS do primeiro array ausentes no segundo. Diferente de todos os 7
    /// participantes do GPT (que preservavam duplicatas), aqui o resultado é deduplicado.
    /// </summary>
    public class ArrayUtilsUnitTest
    {
        [Fact]
        [Trait("Escopo", "NaoConformidadeDeAssinatura")]
        public void ElementosExclusivos_NomeDoMetodoDivergeDoEnunciado()
        {
            const string nomeEsperado = "Difference";
            const string nomeImplementado = "ElementosExclusivos";

            Assert.NotEqual(nomeEsperado, nomeImplementado);
        }

        [Theory]
        [MemberData(nameof(Tarefa1TestData.CasosObrigatorios), MemberType = typeof(Tarefa1TestData))]
        [Trait("Escopo", "Obrigatorio")]
        public void ElementosExclusivos_CasosPadrao(string cenario, int[] a, int[] b, int[] esperado)
        {
            // Act
            var resultado = ArrayUtils.ElementosExclusivos(a, b);

            // Assert
            Assert.True(
                esperado.AsSpan().SequenceEqual(resultado),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", resultado)}]");
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        public void ElementosExclusivos_CasoDuplicatas_ComportamentoObservado()
        {
            var resultado = ArrayUtils.ElementosExclusivos(
                Tarefa1TestData.DuplicatasEntradaA,
                Tarefa1TestData.DuplicatasEntradaB);

            Assert.Equal(Tarefa1TestData.DuplicatasEsperadoDeduplicado, resultado);
        }


        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void ElementosExclusivos_PrimeiroArrayNulo_ComportamentoObservado()
        {
            Action act = () => ArrayUtils.ElementosExclusivos(null, new[] { 1, 5, 7 });
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void ElementosExclusivos_SegundoArrayNulo_ComportamentoObservado()
        {
            Action act = () => ArrayUtils.ElementosExclusivos(new[] { 1, 5, 7 }, null);
            Assert.Throws<ArgumentNullException>(act);
        }
    }
}
