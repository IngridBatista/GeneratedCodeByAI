using System.Reflection;

namespace DEEPSEEK.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_1.Tests
{
    /// <summary>
    /// Testes da Questão 2 para DeepSeek - Participante 1.
    ///
    /// ATENÇÃO - NÃO CONFORMIDADE FUNCIONAL GRAVE (terceira ocorrência do mesmo
    /// padrão, após GPT e Gemini Participante 1):
    /// CompararSequencias() NÃO implementa o requisito 3 (filtro de B > metade
    /// de A). Faz comparação item-a-item entre A e B, e ainda uma demonstração
    /// avulsa convertendo A[0] numa "fração aproximada" (multiplicando por 100).
    /// O requisito central da tarefa está ausente.
    ///
    /// Campos sequenciaA/sequenciaB são privados, sem getters públicos —
    /// diferente do GPT e Gemini Participante 1 (que expunham via propriedades).
    /// Uso reflection para inspecionar o estado após a leitura.
    /// </summary>
    public class FracaoUnitTest
    {
        [Fact]
        [Trait("Escopo", "NaoConformidadeFuncionalGrave")]
        public void CompararSequencias_NaoImplementaFiltroDoEnunciado()
        {
            Assert.True(
                true,
                "CompararSequencias() faz comparação item-a-item e uma demonstração " +
                "avulsa de conversão de A[0] em fração aproximada, não o filtro de " +
                "'fração > metade dos elementos de A' exigido pelo enunciado. " +
                "Requisito 3: NÃO ATENDIDO. Casos de Tarefa2TestData não aplicáveis " +
                "a esta submissão.");
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fracao")]
        public void Fracao_Construtor_ComDenominadorZero_DeveLancarArgumentException()
        {
            Action act = () => new Fracao(1, 0);
            Assert.Throws<ArgumentException>(act);
        }

        [Theory]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fracao")]
        [InlineData(3, 4, 1, 2, true)]
        [InlineData(1, 2, 3, 4, false)]
        [InlineData(1, 2, 2, 4, false)]
        public void Fracao_IsGreater_ComparaCorretamente(int numA, int denA, int numB, int denB, bool esperado)
        {
            var a = new Fracao(numA, denA);
            var b = new Fracao(numB, denB);
            Assert.Equal(esperado, a.IsGreater(b));
        }

        [Theory]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fracao")]
        [InlineData(1, 2, 3, 4, true)]
        [InlineData(3, 4, 1, 2, false)]
        public void Fracao_IsLesser_ComparaCorretamente(int numA, int denA, int numB, int denB, bool esperado)
        {
            var a = new Fracao(numA, denA);
            var b = new Fracao(numB, denB);
            Assert.Equal(esperado, a.IsLesser(b));
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "LeituraA")]
        public void LerSequencias_LeituraNormalDeA_ParaNoZero()
        {
            var (seqA, _) = ExecutarLerSequenciasECapturarListas("3.5\n2.1\n7.0\n0\n-1/1\n");
            Assert.Equal(new[] { 3.5, 2.1, 7.0 }, seqA);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "LeituraA")]
        public void LerSequencias_PrimeiraEntradaZero_SequenciaAVazia()
        {
            var (seqA, _) = ExecutarLerSequenciasECapturarListas("0\n-1/1\n");
            Assert.Empty(seqA);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "LeituraB")]
        public void LerSequencias_LeituraNormalDeB_ParaNaFracaoNegativa()
        {
            var (_, seqB) = ExecutarLerSequenciasECapturarListas("1\n2\n0\n3/4\n5/2\n-1/1\n");

            Assert.Equal(2, seqB.Count);
            Assert.Equal(3, seqB[0].Numerador);
            Assert.Equal(4, seqB[0].Denominador);
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        [Trait("Componente", "LeituraB")]
        public void LerSequencias_FracaoZero_NaoEhTratadaComoSentinela()
        {
            var (_, seqB) = ExecutarLerSequenciasECapturarListas("1\n2\n0\n0/1\n3/4\n-1/2\n");

            Assert.Equal(2, seqB.Count);
            Assert.Equal(0, seqB[0].Numerador);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Executar_SequenciaAVazia_ImprimeErro()
        {
            var saida = ExecutarComparadorCompletoECapturarSaida("0\n-1/1\n");
            Assert.Contains("ERRO: Sequência A está vazia. O programa será encerrado.", saida);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Executar_SequenciaBVazia_ImprimeErro()
        {
            var saida = ExecutarComparadorCompletoECapturarSaida("1\n2\n0\n-1/1\n");
            Assert.Contains("ERRO: Sequência B está vazia. O programa será encerrado.", saida);
        }

        private static (List<double>, List<Fracao>) ExecutarLerSequenciasECapturarListas(string entradaSimulada)
        {
            var comparador = new ComparadorSequencias();
            using var leitorEntrada = new StringReader(entradaSimulada);
            var entradaOriginal = Console.In;
            var saidaOriginal = Console.Out;

            try
            {
                Console.SetIn(leitorEntrada);
                Console.SetOut(TextWriter.Null);
                comparador.LerSequencias();
            }
            finally
            {
                Console.SetIn(entradaOriginal);
                Console.SetOut(saidaOriginal);
            }

            var campoA = typeof(ComparadorSequencias).GetField("sequenciaA", BindingFlags.NonPublic | BindingFlags.Instance);
            var campoB = typeof(ComparadorSequencias).GetField("sequenciaB", BindingFlags.NonPublic | BindingFlags.Instance);

            var seqA = (List<double>)campoA.GetValue(comparador);
            var seqB = (List<Fracao>)campoB.GetValue(comparador);

            return (seqA, seqB);
        }

        private static string ExecutarComparadorCompletoECapturarSaida(string entradaSimulada)
        {
            var comparador = new ComparadorSequencias();
            using var leitorEntrada = new StringReader(entradaSimulada);
            using var escritorSaida = new StringWriter();
            var entradaOriginal = Console.In;
            var saidaOriginal = Console.Out;

            try
            {
                Console.SetIn(leitorEntrada);
                Console.SetOut(escritorSaida);
                comparador.LerSequencias();
                comparador.CompararSequencias();
            }
            finally
            {
                Console.SetIn(entradaOriginal);
                Console.SetOut(saidaOriginal);
            }

            return escritorSaida.ToString();
        }
    }
}
