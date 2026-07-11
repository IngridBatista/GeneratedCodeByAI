using Testes.Compartilhados.Tarefa1;

namespace DEEPSEEK.ARRAY_DIFFERENCE.PLENO.PARTICIPANT_6.Tests
{
    /// <summary>
    /// Testes da Questão 1 para DeepSeek - Participante 6, nível Pleno (Program.ElementosExclusivos).
    ///
    /// ATENÇÃO - NÃO CONFORMIDADE COM O ENUNCIADO:
    /// O enunciado pede "Difference"; esta implementação nomeia o método como "ElementosExclusivos".
    /// Logicamente é idêntica ao padrão HashSet + foreach visto nos Participantes 3, 4 e 5.
    /// </summary>
    public class ProgramUnitTest
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
            var resultado = Program.ElementosExclusivos(a, b);

            Assert.True(
                esperado.AsSpan().SequenceEqual(resultado),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", resultado)}]");
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        public void ElementosExclusivos_CasoDuplicatas_ComportamentoObservado()
        {
            var resultado = Program.ElementosExclusivos(
                Tarefa1TestData.DuplicatasEntradaA,
                Tarefa1TestData.DuplicatasEntradaB);

            Assert.Equal(Tarefa1TestData.DuplicatasEsperadoPreservandoOcorrencias, resultado);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void ElementosExclusivos_PrimeiroArrayNulo_ComportamentoObservado()
        {
            Action act = () => Program.ElementosExclusivos(null, new[] { 1, 5, 7 });
            Assert.Throws<NullReferenceException>(act);
        }

        [Fact]
        [Trait("Escopo", "ForaDoContrato")]
        public void ElementosExclusivos_SegundoArrayNulo_ComportamentoObservado()
        {
            Action act = () => Program.ElementosExclusivos(new[] { 1, 5, 7 }, null);
            Assert.Throws<ArgumentNullException>(act);
        }
    }
}
