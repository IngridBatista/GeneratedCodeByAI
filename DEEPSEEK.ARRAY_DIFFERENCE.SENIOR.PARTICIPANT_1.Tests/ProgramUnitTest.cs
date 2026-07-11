using Testes.Compartilhados.Tarefa1;

namespace DEEPSEEK.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_1.Tests
{
    /// <summary>
    /// Testes da Questão 1 para DeepSeek - Participante 1.
    /// A submissão contém TRÊS variantes: DiferencaEntreArrays (LINQ + Contains),
    /// DiferencaEntreArraysOtimizado (LINQ + HashSet) e DiferencaEntreArraysSemLINQ
    /// (loops aninhados). Todas testadas separadamente.
    ///
    /// ATENÇÃO - NÃO CONFORMIDADE COM O ENUNCIADO:
    /// Nenhuma das três variantes usa o nome "Difference" pedido no enunciado.
    /// </summary>
    public class ProgramUnitTest
    {
        [Fact]
        [Trait("Escopo", "NaoConformidadeDeAssinatura")]
        public void NomesDosMetodos_DivergemDoEnunciado()
        {
            const string nomeEsperado = "Difference";
            Assert.NotEqual(nomeEsperado, "DiferencaEntreArrays");
            Assert.NotEqual(nomeEsperado, "DiferencaEntreArraysOtimizado");
            Assert.NotEqual(nomeEsperado, "DiferencaEntreArraysSemLINQ");
        }

        [Theory]
        [MemberData(nameof(Tarefa1TestData.CasosObrigatorios), MemberType = typeof(Tarefa1TestData))]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Solucao", "Linq")]
        public void DiferencaEntreArrays_CasosPadrao(string cenario, int[] a, int[] b, int[] esperado)
        {
            var resultado = Program.DiferencaEntreArrays(a, b);

            Assert.True(
                esperado.AsSpan().SequenceEqual(resultado),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", resultado)}]");
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        [Trait("Solucao", "Linq")]
        public void DiferencaEntreArrays_CasoDuplicatas_ComportamentoObservado()
        {
            var resultado = Program.DiferencaEntreArrays(
                Tarefa1TestData.DuplicatasEntradaA,
                Tarefa1TestData.DuplicatasEntradaB);

            Assert.Equal(Tarefa1TestData.DuplicatasEsperadoPreservandoOcorrencias, resultado);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        [Trait("Solucao", "Linq")]
        public void DiferencaEntreArrays_PrimeiroArrayNulo_ComportamentoObservado()
        {
            Action act = () => Program.DiferencaEntreArrays(null, new[] { 1, 5, 7 });
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        [Trait("Solucao", "Linq")]
        public void DiferencaEntreArrays_SegundoArrayNulo_ComportamentoObservado()
        {
            Action act = () => Program.DiferencaEntreArrays(new[] { 1, 5, 7 }, null);
            Assert.Throws<ArgumentNullException>(act);
        }


        [Theory]
        [MemberData(nameof(Tarefa1TestData.CasosObrigatorios), MemberType = typeof(Tarefa1TestData))]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Solucao", "Otimizado")]
        public void DiferencaEntreArraysOtimizado_CasosPadrao(string cenario, int[] a, int[] b, int[] esperado)
        {
            var resultado = Program.DiferencaEntreArraysOtimizado(a, b);

            Assert.True(
                esperado.AsSpan().SequenceEqual(resultado),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", resultado)}]");
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        [Trait("Solucao", "Otimizado")]
        public void DiferencaEntreArraysOtimizado_CasoDuplicatas_ComportamentoObservado()
        {
            var resultado = Program.DiferencaEntreArraysOtimizado(
                Tarefa1TestData.DuplicatasEntradaA,
                Tarefa1TestData.DuplicatasEntradaB);

            Assert.Equal(Tarefa1TestData.DuplicatasEsperadoPreservandoOcorrencias, resultado);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        [Trait("Solucao", "Otimizado")]
        public void DiferencaEntreArraysOtimizado_PrimeiroArrayNulo_ComportamentoObservado()
        {
            Action act = () => Program.DiferencaEntreArraysOtimizado(null, new[] { 1, 5, 7 });
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        [Trait("Solucao", "Otimizado")]
        public void DiferencaEntreArraysOtimizado_SegundoArrayNulo_ComportamentoObservado()
        {
            Action act = () => Program.DiferencaEntreArraysOtimizado(new[] { 1, 5, 7 }, null);
            Assert.Throws<ArgumentNullException>(act);
        }

        [Theory]
        [MemberData(nameof(Tarefa1TestData.CasosObrigatorios), MemberType = typeof(Tarefa1TestData))]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Solucao", "SemLinq")]
        public void DiferencaEntreArraysSemLINQ_CasosPadrao(string cenario, int[] a, int[] b, int[] esperado)
        {
            var resultado = Program.DiferencaEntreArraysSemLINQ(a, b);

            Assert.True(
                esperado.AsSpan().SequenceEqual(resultado),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", resultado)}]");
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        [Trait("Solucao", "SemLinq")]
        public void DiferencaEntreArraysSemLINQ_CasoDuplicatas_ComportamentoObservado()
        {
            var resultado = Program.DiferencaEntreArraysSemLINQ(
                Tarefa1TestData.DuplicatasEntradaA,
                Tarefa1TestData.DuplicatasEntradaB);

            Assert.Equal(Tarefa1TestData.DuplicatasEsperadoPreservandoOcorrencias, resultado);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        [Trait("Solucao", "SemLinq")]
        public void DiferencaEntreArraysSemLINQ_PrimeiroArrayNulo_ComportamentoObservado()
        {
            Action act = () => Program.DiferencaEntreArraysSemLINQ(null, new[] { 1, 5, 7 });
            Assert.Throws<NullReferenceException>(act);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        [Trait("Solucao", "SemLinq")]
        public void DiferencaEntreArraysSemLINQ_SegundoArrayNulo_ComportamentoObservado()
        {
            Action act = () => Program.DiferencaEntreArraysSemLINQ(new[] { 1, 5, 7 }, null);
            Assert.Throws<NullReferenceException>(act);
        }
    }
}
