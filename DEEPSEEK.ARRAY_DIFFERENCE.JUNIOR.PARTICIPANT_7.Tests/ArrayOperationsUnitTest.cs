using Testes.Compartilhados.Tarefa1;

namespace DEEPSEEK.ARRAY_DIFFERENCE.JUNIOR.PARTICIPANT_7.Tests
{
    /// <summary>
    /// Testes da Questão 1 para DeepSeek - Participante 7, nível Junior.
    /// A submissão contém DUAS implementações, ambas nomeadas "Difference" (conforme
    /// o enunciado): ArrayOperations (LINQ Where + HashSet) e
    /// ArrayOperationsAlternative (foreach + HashSet). Ambas incluem validação
    /// explícita de nulo e early-returns para arrays vazios.
    ///
    /// Nota: das ~25 submissões analisadas até aqui entre os 3 LLMs, este é apenas
    /// o segundo caso com validação defensiva de nulo (o primeiro foi GPT Participante 1),
    /// apesar do enunciado dispensar essa necessidade.
    /// </summary>
    public class ArrayOperationsUnitTest
    {
        [Theory]
        [MemberData(nameof(Tarefa1TestData.CasosObrigatorios), MemberType = typeof(Tarefa1TestData))]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Solucao", "Linq")]
        public void ArrayOperations_Difference_CasosPadrao(string cenario, int[] a, int[] b, int[] esperado)
        {
            var resultado = ArrayOperations.Difference(a, b);

            Assert.True(
                esperado.AsSpan().SequenceEqual(resultado),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", resultado)}]");
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        [Trait("Solucao", "Linq")]
        public void ArrayOperations_Difference_CasoDuplicatas_ComportamentoObservado()
        {
            var resultado = ArrayOperations.Difference(
                Tarefa1TestData.DuplicatasEntradaA,
                Tarefa1TestData.DuplicatasEntradaB);

            Assert.Equal(Tarefa1TestData.DuplicatasEsperadoPreservandoOcorrencias, resultado);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        [Trait("Solucao", "Linq")]
        public void ArrayOperations_Difference_PrimeiroArrayNulo_DeveLancarArgumentNullException()
        {
            Action act = () => ArrayOperations.Difference(null, new[] { 1, 5, 7 });
            var ex = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("a", ex.ParamName);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        [Trait("Solucao", "Linq")]
        public void ArrayOperations_Difference_SegundoArrayNulo_DeveLancarArgumentNullException()
        {
            Action act = () => ArrayOperations.Difference(new[] { 1, 5, 7 }, null);
            var ex = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("b", ex.ParamName);
        }

        [Theory]
        [MemberData(nameof(Tarefa1TestData.CasosObrigatorios), MemberType = typeof(Tarefa1TestData))]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Solucao", "Alternative")]
        public void ArrayOperationsAlternative_Difference_CasosPadrao(string cenario, int[] a, int[] b, int[] esperado)
        {
            var resultado = ArrayOperationsAlternative.Difference(a, b);

            Assert.True(
                esperado.AsSpan().SequenceEqual(resultado),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", resultado)}]");
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        [Trait("Solucao", "Alternative")]
        public void ArrayOperationsAlternative_Difference_CasoDuplicatas_ComportamentoObservado()
        {
            var resultado = ArrayOperationsAlternative.Difference(
                Tarefa1TestData.DuplicatasEntradaA,
                Tarefa1TestData.DuplicatasEntradaB);

            Assert.Equal(Tarefa1TestData.DuplicatasEsperadoPreservandoOcorrencias, resultado);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        [Trait("Solucao", "Alternative")]
        public void ArrayOperationsAlternative_Difference_PrimeiroArrayNulo_DeveLancarArgumentNullException()
        {
            Action act = () => ArrayOperationsAlternative.Difference(null, new[] { 1, 5, 7 });
            var ex = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("a", ex.ParamName);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        [Trait("Solucao", "Alternative")]
        public void ArrayOperationsAlternative_Difference_SegundoArrayNulo_DeveLancarArgumentNullException()
        {
            Action act = () => ArrayOperationsAlternative.Difference(new[] { 1, 5, 7 }, null);
            var ex = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("b", ex.ParamName);
        }
    }
}
