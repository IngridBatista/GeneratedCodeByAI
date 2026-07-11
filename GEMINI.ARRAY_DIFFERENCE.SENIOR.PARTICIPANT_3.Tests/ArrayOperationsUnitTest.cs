using Testes.Compartilhados.Tarefa1;

namespace GEMINI.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_3.Tests
{
    /// <summary>
    /// Testes da Questão 1 para Gemini - Participante 3.
    /// A submissão contém DUAS soluções: Difference (LINQ/Except) e
    /// Difference_Manual (HashSet + foreach), com comportamentos divergentes
    /// de duplicatas, assim como o Participante 2.
    ///
    /// CONFORMIDADE COM O ENUNCIADO: o método principal já se chama "Difference",
    /// exatamente como pedido. Não há não conformidade de nome para a Solução 1.
    /// A Solução 2 ("Difference_Manual") é um método adicional, extra ao pedido.
    /// </summary>
    public class ArrayOperationsUnitTest
    {
        [Theory]
        [MemberData(nameof(Tarefa1TestData.CasosObrigatorios), MemberType = typeof(Tarefa1TestData))]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Solucao", "Linq")]
        public void Difference_CasosPadrao(string cenario, int[] a, int[] b, int[] esperado)
        {
            var resultado = ArrayOperations.Difference(a, b);

            Assert.True(
                esperado.AsSpan().SequenceEqual(resultado),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", resultado)}]");
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        [Trait("Solucao", "Linq")]
        public void Difference_CasoDuplicatas_ComportamentoObservado()
        {
            var resultado = ArrayOperations.Difference(
                Tarefa1TestData.DuplicatasEntradaA,
                Tarefa1TestData.DuplicatasEntradaB);

            Assert.Equal(Tarefa1TestData.DuplicatasEsperadoDeduplicado, resultado);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        [Trait("Solucao", "Linq")]
        public void Difference_PrimeiroArrayNulo_ComportamentoObservado()
        {
            Action act = () => ArrayOperations.Difference(null, new[] { 1, 5, 7 });
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        [Trait("Solucao", "Linq")]
        public void Difference_SegundoArrayNulo_ComportamentoObservado()
        {
            Action act = () => ArrayOperations.Difference(new[] { 1, 5, 7 }, null);
            Assert.Throws<ArgumentNullException>(act);
        }

        [Theory]
        [MemberData(nameof(Tarefa1TestData.CasosObrigatorios), MemberType = typeof(Tarefa1TestData))]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Solucao", "Manual")]
        public void DifferenceManual_CasosPadrao(string cenario, int[] a, int[] b, int[] esperado)
        {
            var resultado = ArrayOperations.Difference_Manual(a, b);

            Assert.True(
                esperado.AsSpan().SequenceEqual(resultado),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", resultado)}]");
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        [Trait("Solucao", "Manual")]
        public void DifferenceManual_CasoDuplicatas_ComportamentoObservado()
        {
            var resultado = ArrayOperations.Difference_Manual(
                Tarefa1TestData.DuplicatasEntradaA,
                Tarefa1TestData.DuplicatasEntradaB);

            Assert.Equal(Tarefa1TestData.DuplicatasEsperadoPreservandoOcorrencias, resultado);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        [Trait("Solucao", "Manual")]
        public void DifferenceManual_PrimeiroArrayNulo_ComportamentoObservado()
        {
            Action act = () => ArrayOperations.Difference_Manual(null, new[] { 1, 5, 7 });
            Assert.Throws<NullReferenceException>(act);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        [Trait("Solucao", "Manual")]
        public void DifferenceManual_SegundoArrayNulo_ComportamentoObservado()
        {
            Action act = () => ArrayOperations.Difference_Manual(new[] { 1, 5, 7 }, null);
            Assert.Throws<ArgumentNullException>(act);
        }
    }
}
