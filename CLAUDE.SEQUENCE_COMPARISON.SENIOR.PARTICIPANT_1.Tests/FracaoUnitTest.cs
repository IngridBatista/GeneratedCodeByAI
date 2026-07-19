using System.Reflection;

namespace CLAUDE.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_1.Tests
{
    /// <summary>
    /// Testes da Questão 2 para Claude - Participante 1.
    ///
    /// ATENÇÃO - NÃO CONFORMIDADE FUNCIONAL GRAVE (QUARTA ocorrência do mesmo
    /// padrão, completando 4 de 4 LLMs: GPT, Gemini, DeepSeek e agora Claude,
    /// todos com "Participante 1" falhando exatamente da mesma forma):
    /// CompararSequencias() NÃO implementa o requisito 3. Em vez disso, calcula
    /// estatísticas (soma, média, máximo, mínimo) de A e B, compara as médias, e
    /// faz uma comparação elemento-a-elemento. O requisito central da tarefa
    /// está ausente.
    ///
    /// Essa coincidência (100% dos "Participante 1" com o mesmo tipo de erro,
    /// entre 4 LLMs independentes) sugere fortemente um artefato metodológico
    /// na coleta de dados nessa posição específica da amostra, não uma
    /// característica genuína dos modelos — vale investigar e mencionar como
    /// limitação na monografia.
    /// </summary>
    public class FracaoUnitTest
    {
        [Fact]
        [Trait("Escopo", "NaoConformidadeFuncionalGrave")]
        public void CompararSequencias_NaoImplementaFiltroDoEnunciado()
        {
            Assert.Fail(
                "CompararSequencias() calcula estatísticas (soma, média, máximo, " +
                "mínimo) e faz comparação elemento-a-elemento, não o filtro de " +
                "'fração > metade dos elementos de A' exigido pelo enunciado. " +
                "Requisito 3: NÃO ATENDIDO. Quarta ocorrência do mesmo padrão " +
                "entre os 4 LLMs analisados (GPT, Gemini, DeepSeek, Claude), " +
                "todos no 'Participante 1'.");
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
        public void LerSequenciaA_LeituraNormal_RetornaTrueEArmazenaValores()
        {
            var (comparador, retorno) = ExecutarLerSequenciaA("3.5\n2.1\n7.0\n0\n");

            Assert.True(retorno);
            var seqA = ObterSequenciaA(comparador);
            Assert.Equal(new[] { 3.5, 2.1, 7.0 }, seqA);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "LeituraA")]
        public void LerSequenciaA_PrimeiraEntradaZero_RetornaFalse()
        {
            var (_, retorno) = ExecutarLerSequenciaA("0\n");
            Assert.False(retorno);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "LeituraB")]
        public void LerSequenciaB_LeituraNormal_RetornaTrueEArmazenaFracoes()
        {
            var (comparador, retorno) = ExecutarLerSequenciaB("3/4\n5/2\n-1/1\n");

            Assert.True(retorno);
            var seqB = ObterSequenciaB(comparador);
            Assert.Equal(2, seqB.Count);
            Assert.Equal(3, seqB[0].Numerador);
            Assert.Equal(4, seqB[0].Denominador);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "LeituraB")]
        public void LerSequenciaB_PrimeiraFracaoNegativa_RetornaFalse()
        {
            var (_, retorno) = ExecutarLerSequenciaB("-1/2\n");
            Assert.False(retorno);
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        [Trait("Componente", "LeituraB")]
        public void LerSequenciaB_FracaoZero_NaoEhTratadaComoSentinela()
        {
            var (comparador, retorno) = ExecutarLerSequenciaB("0/1\n3/4\n-1/2\n");

            Assert.True(retorno);
            var seqB = ObterSequenciaB(comparador);
            Assert.Equal(2, seqB.Count);
            Assert.Equal(0, seqB[0].Numerador);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Executar_SequenciaAVazia_ImprimeErroEEncerra()
        {
            var saida = ExecutarProgramaCompletoECapturarSaida("0\n-1/1\n");
            Assert.Contains("[ERRO] A sequência A está vazia!", saida);
            Assert.Contains("Programa encerrado.", saida);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Executar_SequenciaBVazia_ImprimeErroEEncerra()
        {
            var saida = ExecutarProgramaCompletoECapturarSaida("1\n2\n0\n-1/1\n");
            Assert.Contains("[ERRO] A sequência B está vazia!", saida);
            Assert.Contains("Programa encerrado.", saida);
        }

        private static List<double> ObterSequenciaA(ComparadorSequencias comparador)
        {
            var campo = typeof(ComparadorSequencias).GetField("sequenciaA", BindingFlags.NonPublic | BindingFlags.Instance);
            return (List<double>)campo.GetValue(comparador);
        }

        private static List<Fracao> ObterSequenciaB(ComparadorSequencias comparador)
        {
            var campo = typeof(ComparadorSequencias).GetField("sequenciaB", BindingFlags.NonPublic | BindingFlags.Instance);
            return (List<Fracao>)campo.GetValue(comparador);
        }

        private static (ComparadorSequencias, bool) ExecutarLerSequenciaA(string entradaSimulada)
        {
            var comparador = new ComparadorSequencias();
            using var leitorEntrada = new StringReader(entradaSimulada);
            var entradaOriginal = Console.In;
            var saidaOriginal = Console.Out;
            bool retorno;

            try
            {
                Console.SetIn(leitorEntrada);
                Console.SetOut(TextWriter.Null);
                retorno = comparador.LerSequenciaA();
            }
            finally
            {
                Console.SetIn(entradaOriginal);
                Console.SetOut(saidaOriginal);
            }

            return (comparador, retorno);
        }

        private static (ComparadorSequencias, bool) ExecutarLerSequenciaB(string entradaSimulada)
        {
            var comparador = new ComparadorSequencias();
            using var leitorEntrada = new StringReader(entradaSimulada);
            var entradaOriginal = Console.In;
            var saidaOriginal = Console.Out;
            bool retorno;

            try
            {
                Console.SetIn(leitorEntrada);
                Console.SetOut(TextWriter.Null);
                retorno = comparador.LerSequenciaB();
            }
            finally
            {
                Console.SetIn(entradaOriginal);
                Console.SetOut(saidaOriginal);
            }

            return (comparador, retorno);
        }

        private static string ExecutarProgramaCompletoECapturarSaida(string entradaSimulada)
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
                comparador.Executar();
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
