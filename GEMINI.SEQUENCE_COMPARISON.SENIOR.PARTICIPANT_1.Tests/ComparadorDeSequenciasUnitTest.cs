using System.Globalization;

namespace GEMINI.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_1.Tests
{
    /// <summary>
    /// Testes da Questão 2 para Gemini - Participante 1.
    ///
    /// ATENÇÃO - NÃO CONFORMIDADE FUNCIONAL GRAVE (igual ao GPT Participante 1):
    /// CompararSequencias() NÃO implementa o requisito 3 (filtro de B > metade de A).
    /// Em vez disso, faz uma comparação item-a-item entre A e B e uma comparação
    /// de somas totais. O requisito central da tarefa está ausente.
    ///
    /// ATENÇÃO - RISCO DE BUG DE CULTURA (Program.cs, não incluído nos testes por
    /// não ser a classe de lógica, mas documentado aqui por afetar o comportamento
    /// real do programa):
    /// Program.Main() define Thread.CurrentCulture como "pt-BR" antes de qualquer
    /// leitura. Em pt-BR, "." é o separador de milhar (não decimal). Segundo a
    /// documentação da Microsoft e relatos no repositório dotnet/runtime,
    /// NumberStyles.AllowThousands (usado implicitamente por double.TryParse(string))
    /// NÃO valida o agrupamento correto do separador de milhar — apenas permite o
    /// caractere e o ignora. Isso sugere que a entrada "3.5" seria interpretada como
    /// "35" (35.0), não 3.5, sob essa cultura. NÃO EXECUTADO em ambiente .NET real
    /// neste momento (sem runtime disponível) — a previsão é baseada em documentação
    /// oficial; recomenda-se confirmar rodando o teste abaixo no ambiente real antes
    /// de reportar como fato consolidado.
    /// </summary>
    public class ComparadorDeSequenciasUnitTest
    {
        [Fact]
        [Trait("Escopo", "NaoConformidadeFuncionalGrave")]
        public void CompararSequencias_NaoImplementaFiltroDoEnunciado()
        {
            Assert.True(
                true,
                "CompararSequencias() faz comparação item-a-item e de somas totais, " +
                "não o filtro de 'fração > metade dos elementos de A' exigido pelo " +
                "enunciado. Requisito 3: NÃO ATENDIDO. Casos de Tarefa2TestData não " +
                "aplicáveis a esta submissão.");
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
        [InlineData(1, 2, 2, 4, false)]
        public void Fracao_IsLesser_ComparaCorretamente(int numA, int denA, int numB, int denB, bool esperado)
        {
            var a = new Fracao(numA, denA);
            var b = new Fracao(numB, denB);
            Assert.Equal(esperado, a.IsLesser(b));
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "LeituraA")]
        public void LerSequenciaA_LeituraNormal_ParaNoZero_CulturaPadrao()
        {
            // Usa cultura invariante/padrão do ambiente de teste, SEM o "pt-BR"
            // forçado por Program.Main() (que só existe nesse Program, não em
            // ComparadorDeSequencias). Aqui o parsing deve funcionar normalmente.
            var comparador = ExecutarComCulturaEspecifica("3.5\n2.1\n7.0\n0\n-1/1\n", CultureInfo.InvariantCulture);

            Assert.Equal(new[] { 3.5, 2.1, 7.0 }, comparador.SequenciaA);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "LeituraA")]
        public void LerSequenciaA_PrimeiraEntradaZero_SequenciaVazia()
        {
            var comparador = ExecutarComCulturaEspecifica("0\n-1/1\n", CultureInfo.InvariantCulture);
            Assert.Empty(comparador.SequenciaA);
        }8

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "LeituraB")]
        public void LerSequenciaB_LeituraNormal_ParaNaFracaoNegativa()
        {
            var comparador = ExecutarComCulturaEspecifica("1\n2\n0\n3/4\n5/2\n-1/1\n", CultureInfo.InvariantCulture);

            Assert.Equal(2, comparador.SequenciaB.Count);
            Assert.Equal(3, comparador.SequenciaB[0].Numerador);
            Assert.Equal(4, comparador.SequenciaB[0].Denominador);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Executar_SequenciaAVazia_ImprimeMensagemDeEncerramento()
        {
            var (comparador, saida) = ExecutarComCulturaEspecificaECapturarSaida("0\n-1/1\n", CultureInfo.InvariantCulture);

            Assert.Empty(comparador.SequenciaA);
            Assert.Contains("Programa encerrado: A Sequência A está vazia", saida);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Executar_SequenciaBVazia_ImprimeMensagemDeEncerramento()
        {
            var (comparador, saida) = ExecutarComCulturaEspecificaECapturarSaida("1\n2\n0\n-1/1\n", CultureInfo.InvariantCulture);

            Assert.Empty(comparador.SequenciaB);
            Assert.Contains("Programa encerrado: A Sequência B está vazia", saida);
        }

        [Fact]
        [Trait("Escopo", "RiscoDeBugDeCultura")]
        [Trait("Componente", "LeituraA")]
        public void LerSequenciaA_SobCulturaPtBR_PodeMisinterpretarPontoComoSeparadorDeMilhar()
        {
            // ATENÇÃO: previsão baseada em documentação oficial (NumberStyles.AllowThousands
            // não valida agrupamento), NÃO confirmada por execução real neste ambiente
            // (sem runtime .NET disponível). Confirme rodando este teste antes de
            // reportar como fato consolidado na monografia.
            //
            // Se a previsão estiver correta, "3.5" sob pt-BR seria interpretado como
            // "35" (35.0), já que "." é o separador de milhar nessa cultura e o
            // caractere seria apenas removido, não usado como separador decimal.
            var comparador = ExecutarComCulturaEspecifica("3.5\n0\n-1/1\n", new CultureInfo("pt-BR"));

            // Esperado SE a previsão de bug estiver correta:
            Assert.Equal(new[] { 35.0 }, comparador.SequenciaA);

            // Se este assert falhar, o comportamento real observado deve substituir
            // esta expectativa e a nota de "risco não confirmado" no cabeçalho da
            // classe deve ser atualizada com o resultado real.
        }

        private static ComparadorDeSequencias ExecutarComCulturaEspecifica(string entradaSimulada, CultureInfo cultura)
        {
            var (comparador, _) = ExecutarComCulturaEspecificaECapturarSaida(entradaSimulada, cultura);
            return comparador;
        }

        private static (ComparadorDeSequencias, string) ExecutarComCulturaEspecificaECapturarSaida(
            string entradaSimulada, CultureInfo cultura)
        {
            var comparador = new ComparadorDeSequencias();
            using var leitorEntrada = new StringReader(entradaSimulada);
            using var escritorSaida = new StringWriter();
            var entradaOriginal = Console.In;
            var saidaOriginal = Console.Out;
            var culturaOriginal = Thread.CurrentThread.CurrentCulture;

            try
            {
                Console.SetIn(leitorEntrada);
                Console.SetOut(escritorSaida);
                Thread.CurrentThread.CurrentCulture = cultura;
                comparador.Executar();
            }
            finally
            {
                // Restaura estado global para não afetar outros testes (importante:
                // Thread.CurrentCulture é estado mutável compartilhado).
                Console.SetIn(entradaOriginal);
                Console.SetOut(saidaOriginal);
                Thread.CurrentThread.CurrentCulture = culturaOriginal;
            }

            return (comparador, escritorSaida.ToString());
        }
    }
}
