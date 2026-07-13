using System.Reflection;
using System.Text.RegularExpressions;
using Testes.Compartilhados.Tarefa2;

namespace GPT.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_3.Tests
{
    /// <summary>
    /// Testes da Questão 2 para GPT - Participante 3.
    ///
    /// ATENÇÃO - NÃO CONFORMIDADE GRAVE DE TIPO DE DADO:
    /// A deveria ser List&lt;double&gt; conforme o enunciado; esta implementação usa
    /// List&lt;int&gt;, ignorando silenciosamente qualquer valor decimal digitado.
    ///
    /// Os casos obrigatórios de filtro (Tarefa2TestData) usam apenas valores
    /// inteiros em A, portanto continuam válidos e comparáveis mesmo com essa
    /// limitação de tipo desta implementação específica.
    /// </summary>
    public class ProgramUnitTest
    {
        [Theory]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        [InlineData(3, 4, 1, 2, true)]
        [InlineData(1, 2, 3, 4, false)]
        [InlineData(1, 2, 2, 4, false)]
        public void Fraction_IsGreater_ComparaCorretamente(
            int numA, int denA, int numB, int denB, bool esperado)
        {
            var a = new Fraction(numA, denA);
            var b = new Fraction(numB, denB);
            Assert.Equal(esperado, a.IsGreater(b));
        }

        [Theory]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        [InlineData(1, 2, 3, 4, true)]
        [InlineData(3, 4, 1, 2, false)]
        [InlineData(1, 2, 2, 4, false)]
        public void Fraction_IsLesser_ComparaCorretamente(
            int numA, int denA, int numB, int denB, bool esperado)
        {
            var a = new Fraction(numA, denA);
            var b = new Fraction(numB, denB);
            Assert.Equal(esperado, a.IsLesser(b));
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        public void Fraction_IsGreater_UsaComparacaoPorMultiplicacaoCruzada_EvitaImprecisaoDePontoFlutuante()
        {
            var a = new Fraction(1, 3);
            var b = new Fraction(2, 6);

            Assert.False(a.IsGreater(b));
            Assert.False(a.IsLesser(b));
        }

        [Fact]
        [Trait("Escopo", "NaoConformidadeDeTipoGrave")]
        public void SequenciaA_ValoresDecimaisSaoSilenciosamenteIgnorados()
        {
            var listaA = ExecutarRunECapturarListaA("3.5\n2.5\n0\n-1/1\n");
            Assert.Empty(listaA);
        }

        [Fact]
        [Trait("Escopo", "NaoConformidadeDeTipoGrave")]
        public void SequenciaA_ApenasValoresDecimais_DisparaErroDeSequenciaVaziaIncorretamente()
        {
            var (_, saidaCompleta) = ExecutarRunECapturarTudo("3.5\n2.5\n7.25\n0\n-1/1\n");
            Assert.Contains("Erro: a lista A está nula ou vazia.", saidaCompleta);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        public void SequenciaA_ValoresInteiros_SaoAceitosNormalmente()
        {
            var listaA = ExecutarRunECapturarListaA("1\n2\n3\n0\n-1/1\n");
            Assert.Equal(new[] { 1, 2, 3 }, listaA);
        }

        [Theory]
        [MemberData(nameof(Tarefa2TestData.CasosObrigatoriosFiltro), MemberType = typeof(Tarefa2TestData))]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "FiltroPrincipal")]
        public void Run_CasosCompartilhados(string cenario, string entradaConsole, (int Numerador, int Denominador)[] esperado)
        {
            var fracoesImpressas = ExecutarRunECapturarFracoesImpressas(entradaConsole);

            Assert.True(
                EhIgual(esperado, fracoesImpressas),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", fracoesImpressas)}]");
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Run_SequenciaAVazia_ImprimeErro()
        {
            var (_, saidaCompleta) = ExecutarRunECapturarTudo(Tarefa2TestData.EntradaSequenciaAVazia);
            Assert.Contains("Erro: a lista A está nula ou vazia.", saidaCompleta);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Run_SequenciaBVazia_ImprimeErro()
        {
            var (_, saidaCompleta) = ExecutarRunECapturarTudo(Tarefa2TestData.EntradaSequenciaBVazia);
            Assert.Contains("Erro: a lista B está nula ou vazia.", saidaCompleta);
        }

        private static readonly Regex PadraoFracao = new Regex(@"^(-?\d+)/(-?\d+)$", RegexOptions.Compiled);

        private static bool EhIgual((int, int)[] esperado, List<(int, int)> obtido)
        {
            if (esperado.Length != obtido.Count) return false;
            for (int i = 0; i < esperado.Length; i++)
                if (esperado[i] != obtido[i]) return false;
            return true;
        }
        private static List<(int, int)> ExecutarRunECapturarFracoesImpressas(string entradaSimulada)
        {
            var (_, saidaCompleta) = ExecutarRunECapturarTudo(entradaSimulada);
            var fracoesEncontradas = new List<(int, int)>();

            foreach (var linha in saidaCompleta.Split('\n'))
            {
                var linhaLimpa = linha.Trim().TrimEnd('\r');
                var match = PadraoFracao.Match(linhaLimpa);
                if (match.Success)
                    fracoesEncontradas.Add((int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)));
            }

            return fracoesEncontradas;
        }

        private static List<int> ExecutarRunECapturarListaA(string entradaSimulada)
        {
            var (programa, _) = ExecutarRunECapturarTudoComInstancia(entradaSimulada);
            var campoA = typeof(Program).GetField("A", BindingFlags.NonPublic | BindingFlags.Instance);
            return (List<int>)campoA.GetValue(programa);
        }

        private static (List<(int, int)>, string) ExecutarRunECapturarTudo(string entradaSimulada)
        {
            var (_, saidaCompleta) = ExecutarRunECapturarTudoComInstancia(entradaSimulada);
            var fracoes = new List<(int, int)>();
            foreach (var linha in saidaCompleta.Split('\n'))
            {
                var linhaLimpa = linha.Trim().TrimEnd('\r');
                var match = PadraoFracao.Match(linhaLimpa);
                if (match.Success)
                    fracoes.Add((int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)));
            }
            return (fracoes, saidaCompleta);
        }

        private static (Program, string) ExecutarRunECapturarTudoComInstancia(string entradaSimulada)
        {
            var programa = new Program();
            using var leitorEntrada = new StringReader(entradaSimulada);
            using var escritorSaida = new StringWriter();
            var entradaOriginal = Console.In;
            var saidaOriginal = Console.Out;

            try
            {
                Console.SetIn(leitorEntrada);
                Console.SetOut(escritorSaida);
                programa.Run();
            }
            finally
            {
                Console.SetIn(entradaOriginal);
                Console.SetOut(saidaOriginal);
            }

            return (programa, escritorSaida.ToString());
        }
    }
}
