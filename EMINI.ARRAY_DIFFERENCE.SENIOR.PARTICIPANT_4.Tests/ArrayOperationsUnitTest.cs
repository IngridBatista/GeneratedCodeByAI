using GEMINI.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_4;
using Testes.Compartilhados.Tarefa1;

namespace EMINI.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_4.Tests
{
    /// <summary>
    /// Testes da Questão 1 para Gemini - Participante 4 (ArrayOperations.Difference).
    /// Nome do método conforme o enunciado. Usa .Except() -> mesmo padrão de
    /// deduplicação e ArgumentNullException observado nos Participantes 1 e 3 (solução LINQ).
    /// </summary>
    public class ArrayOperationsUnitTest
    {
        [Theory]
        [MemberData(nameof(Tarefa1TestData.CasosObrigatorios), MemberType = typeof(Tarefa1TestData))]
        [Trait("Escopo", "Obrigatorio")]
        public void Difference_CasosPadrao(string cenario, int[] a, int[] b, int[] esperado)
        {
            var resultado = ArrayOperations.Difference(a, b);

            Assert.True(
                esperado.AsSpan().SequenceEqual(resultado),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", resultado)}]");
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        public void Difference_CasoDuplicatas_ComportamentoObservado()
        {
            var resultado = ArrayOperations.Difference(
                Tarefa1TestData.DuplicatasEntradaA,
                Tarefa1TestData.DuplicatasEntradaB);

            Assert.Equal(Tarefa1TestData.DuplicatasEsperadoDeduplicado, resultado);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void Difference_PrimeiroArrayNulo_ComportamentoObservado()
        {
            Action act = () => ArrayOperations.Difference(null, new[] { 1, 5, 7 });
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void Difference_SegundoArrayNulo_ComportamentoObservado()
        {
            Action act = () => ArrayOperations.Difference(new[] { 1, 5, 7 }, null);
            Assert.Throws<ArgumentNullException>(act);
        }
    }

}
