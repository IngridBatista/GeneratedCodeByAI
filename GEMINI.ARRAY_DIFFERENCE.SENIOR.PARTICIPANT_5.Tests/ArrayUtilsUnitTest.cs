using Testes.Compartilhados.Tarefa1;

namespace GEMINI.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_5.Tests
{
    /// <summary>
    /// Testes da Questão 1 para Gemini - Participante 5 (ArrayUtils.Difference).
    /// Nome do método conforme o enunciado. Usa .Except() -> mesmo padrão de
    /// deduplicação e ArgumentNullException observado nos demais participantes LINQ do Gemini.
    /// </summary>
    public class ArrayUtilsUnitTest
    {
        [Theory]
        [MemberData(nameof(Tarefa1TestData.CasosObrigatorios), MemberType = typeof(Tarefa1TestData))]
        [Trait("Escopo", "Obrigatorio")]
        public void Difference_CasosPadrao(string cenario, int[] a, int[] b, int[] esperado)
        {
            var resultado = ArrayUtils.Difference(a, b);

            Assert.True(
                esperado.AsSpan().SequenceEqual(resultado),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", resultado)}]");
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        public void Difference_CasoDuplicatas_ComportamentoObservado()
        {
            var resultado = ArrayUtils.Difference(
                Tarefa1TestData.DuplicatasEntradaA,
                Tarefa1TestData.DuplicatasEntradaB);

            Assert.Equal(Tarefa1TestData.DuplicatasEsperadoDeduplicado, resultado);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void Difference_PrimeiroArrayNulo_ComportamentoObservado()
        {
            Action act = () => ArrayUtils.Difference(null, new[] { 1, 5, 7 });
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void Difference_SegundoArrayNulo_ComportamentoObservado()
        {
            Action act = () => ArrayUtils.Difference(new[] { 1, 5, 7 }, null);
            Assert.Throws<ArgumentNullException>(act);
        }
    }
}
