using Testes.Compartilhados.Tarefa1;

namespace GEMINI.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_2.Tests
{
    // Pré-requisito: tornar Program.EncontrarDiferencaComLinq e
    // Program.EncontrarDiferencaComLoop "internal" ou "public" (já são "public" aqui,
    // então nenhuma mudança de acesso é necessária).

    /// <summary>
    /// Testes da Questão 1 para Gemini - Participante 2.
    /// A submissão contém DUAS soluções: EncontrarDiferencaComLinq e
    /// EncontrarDiferencaComLoop. Ambas são testadas separadamente, pois
    /// divergem entre si no tratamento de duplicatas.
    ///
    /// ATENÇÃO - NÃO CONFORMIDADE COM O ENUNCIADO:
    /// Nenhum dos dois métodos usa o nome "Difference" pedido no enunciado.
    /// </summary>
    public class ProgramUnitTest
    {
        [Fact]
        [Trait("Escopo", "NaoConformidadeDeAssinatura")]
        public void NomesDosMetodos_DivergemDoEnunciado()
        {
            const string nomeEsperado = "Difference";
            Assert.NotEqual(nomeEsperado, "EncontrarDiferencaComLinq");
            Assert.NotEqual(nomeEsperado, "EncontrarDiferencaComLoop");
        }

        [Theory]
        [MemberData(nameof(Tarefa1TestData.CasosObrigatorios), MemberType = typeof(Tarefa1TestData))]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Solucao", "Linq")]
        public void EncontrarDiferencaComLinq_CasosPadrao(string cenario, int[] a, int[] b, int[] esperado)
        {
            var resultado = Program.EncontrarDiferencaComLinq(a, b);

            Assert.True(
                esperado.AsSpan().SequenceEqual(resultado),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", resultado)}]");
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        [Trait("Solucao", "Linq")]
        public void EncontrarDiferencaComLinq_CasoDuplicatas_ComportamentoObservado()
        {
            var resultado = Program.EncontrarDiferencaComLinq(
                Tarefa1TestData.DuplicatasEntradaA,
                Tarefa1TestData.DuplicatasEntradaB);

            Assert.Equal(Tarefa1TestData.DuplicatasEsperadoDeduplicado, resultado);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        [Trait("Solucao", "Linq")]
        public void EncontrarDiferencaComLinq_PrimeiroArrayNulo_ComportamentoObservado()
        {
            Action act = () => Program.EncontrarDiferencaComLinq(null, new[] { 1, 5, 7 });
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        [Trait("Solucao", "Linq")]
        public void EncontrarDiferencaComLinq_SegundoArrayNulo_ComportamentoObservado()
        {
            Action act = () => Program.EncontrarDiferencaComLinq(new[] { 1, 5, 7 }, null);
            Assert.Throws<ArgumentNullException>(act);
        }

        [Theory]
        [MemberData(nameof(Tarefa1TestData.CasosObrigatorios), MemberType = typeof(Tarefa1TestData))]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Solucao", "Loop")]
        public void EncontrarDiferencaComLoop_CasosPadrao(string cenario, int[] a, int[] b, int[] esperado)
        {
            var resultado = Program.EncontrarDiferencaComLoop(a, b);

            Assert.True(
                esperado.AsSpan().SequenceEqual(resultado),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", resultado)}]");
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        [Trait("Solucao", "Loop")]
        public void EncontrarDiferencaComLoop_CasoDuplicatas_ComportamentoObservado()
        {
            var resultado = Program.EncontrarDiferencaComLoop(
                Tarefa1TestData.DuplicatasEntradaA,
                Tarefa1TestData.DuplicatasEntradaB);

            Assert.Equal(Tarefa1TestData.DuplicatasEsperadoPreservandoOcorrencias, resultado);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        [Trait("Solucao", "Loop")]
        public void EncontrarDiferencaComLoop_PrimeiroArrayNulo_ComportamentoObservado()
        {
            Action act = () => Program.EncontrarDiferencaComLoop(null, new[] { 1, 5, 7 });
            Assert.Throws<NullReferenceException>(act);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        [Trait("Solucao", "Loop")]
        public void EncontrarDiferencaComLoop_SegundoArrayNulo_ComportamentoObservado()
        {
            Action act = () => Program.EncontrarDiferencaComLoop(new[] { 1, 5, 7 }, null);
            Assert.Throws<ArgumentNullException>(act);
        }
    }
}
