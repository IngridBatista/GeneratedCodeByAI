using System.Text.RegularExpressions;
using Testes.Compartilhados.Tarefa2;

namespace DEEPSEEK.SEQUENCE_COMPARISON.PLENO.PARTICIPANT_6.Tests
{
    /// <summary>
    /// Testes da Questão 2 para DeepSeek - Participante 6, nível Pleno (CompareSequence).
    ///
    /// ATENÇÃO - NÃO CONFORMIDADE FUNCIONAL GRAVE (entendimento fundamentalmente
    /// errado do enunciado, mais severo que os algoritmos de mediana dos
    /// Participantes 2 e 5):
    /// Em vez de contar quantos elementos de A cada fração de B supera e comparar
    /// essa contagem com pelo menos metade da QUANTIDADE de elementos de A, esta
    /// implementação divide CADA VALOR INDIVIDUAL de A por 2 (criando uma lista
    /// "halfOfA"), e verifica se a fração supera PELO MENOS UM desses valores
    /// divididos ("countGreater > 0"). Isso NÃO é equivalente ao critério do
    /// enunciado e diverge em casos comuns, não apenas em limites extremos --
    /// ver testes específicos abaixo demonstrando divergência em casos que
    /// deveriam ser claramente excluídos.
    ///
    /// Formato de entrada é DIFERENTE de todos os demais: numerador e
    /// denominador são lidos em DUAS LINHAS SEPARADAS ("Numerador: " e
    /// "Denominador: "), não "numerador/denominador" numa linha só. Foi
    /// necessário um conversor específico para reaproveitar Tarefa2TestData.
    ///
    /// IsNegative() está correta (verifica a combinação de sinais dos dois
    /// componentes, sem o bug de string bruta visto no DeepSeek Participante 2).
    /// </summary>
    public class FractionUnitTest
    {
        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        public void Fraction_Construtor_ComDenominadorZero_DeveLancarArgumentException()
        {
            Action act = () => new Fraction(1, 0);
            Assert.Throws<ArgumentException>(act);
        }

        [Theory]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        [InlineData(-3, 4, true)]
        [InlineData(3, -4, true)]
        [InlineData(-3, -4, false)]
        [InlineData(3, 4, false)]
        [InlineData(0, 4, false)]
        public void Fraction_IsNegative_DetectaCorretamente(int num, int den, bool esperado)
        {
            var fracao = new Fraction(num, den);
            Assert.Equal(esperado, fracao.IsNegative());
        }

        [Theory]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        [InlineData(3, 4, 1, 2, true)]
        [InlineData(1, 2, 3, 4, false)]
        public void Fraction_IsGreater_ComOutraFracao_ComparaCorretamente(int numA, int denA, int numB, int denB, bool esperado)
        {
            var a = new Fraction(numA, denA);
            var b = new Fraction(numB, denB);
            Assert.Equal(esperado, a.IsGreater(b));
        }

        [Theory]
        [MemberData(nameof(Tarefa2TestData.CasosObrigatoriosFiltro), MemberType = typeof(Tarefa2TestData))]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "FiltroPrincipal")]
        public void Main_CasosCompartilhados(string cenario, string entradaConsoleComBarra, (int Numerador, int Denominador)[] esperado)
        {
            var entradaConvertida = ConverterParaFormatoDuasLinhas(entradaConsoleComBarra);
            var fracoesImpressas = ExecutarECapturarFracoesImpressas(entradaConvertida);

            var divergenciaEsperada = cenario == "ContagemImparNoLimiar_DeveExcluir" || cenario == "EmpateNaoContaComoMaior_ComparacaoDeveSerEstrita";

            Assert.True(
                EhIgual(esperado, fracoesImpressas),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", fracoesImpressas)}]" +
                (divergenciaEsperada
                    ? " (DIVERGÊNCIA ESPERADA: algoritmo divide cada valor de A por 2 e checa 'maior que pelo menos um', não a contagem correta — ver nota da classe)"
                    : ""));
        }

        [Fact]
        [Trait("Escopo", "NaoConformidadeFuncionalGrave")]
        [Trait("Componente", "FiltroPrincipal")]
        public void Main_FracaoQueDeveriaSerExcluida_EhIncluidaIncorretamente()
        {
            // A = [1,2,3,4,5]. Fração 3/1 (valor=3). Critério correto: supera
            // apenas 2 de 5 elementos (1 e 2), 2 < ceil(5/2)=3 -> deveria ser EXCLUÍDA.
            //
            // Algoritmo desta implementação: halfOfA = [0.5,1,1.5,2,2.5].
            // 3 supera TODOS os 5 valores de halfOfA -> countGreater=5>0 -> INCLUÍDA
            // (incorretamente).
            var entrada = ConverterParaFormatoDuasLinhas("1\n2\n3\n4\n5\n0\n3/1\n-1/1\n");

            var fracoesImpressas = ExecutarECapturarFracoesImpressas(entrada);

            // Comportamento real (incorreto) desta implementação:
            Assert.Single(fracoesImpressas);
            Assert.Equal((3, 1), fracoesImpressas[0]);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "FiltroPrincipal")]
        public void Main_NenhumaFracaoAtendeAoCriterio_ImprimeMensagemInformativa()
        {
            // Escolhida propositalmente para que NENHUM valor de halfOfA seja
            // superado, evitando a divergência do algoritmo nesta verificação pontual.
            var entrada = ConverterParaFormatoDuasLinhas("100\n200\n0\n1/10\n-1/1\n");
            var (_, saidaCompleta) = ExecutarECapturarTudo(entrada);
            Assert.Contains("Nenhuma fração atende ao critério.", saidaCompleta);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Main_SequenciaAVazia_ImprimeErro()
        {
            var entrada = ConverterParaFormatoDuasLinhas(Tarefa2TestData.EntradaSequenciaAVazia);
            var (fracoesImpressas, saidaCompleta) = ExecutarECapturarTudo(entrada);

            Assert.Empty(fracoesImpressas);
            Assert.Contains("Erro: A sequência A está vazia.", saidaCompleta);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Main_SequenciaBVazia_ImprimeErro()
        {
            var entrada = ConverterParaFormatoDuasLinhas(Tarefa2TestData.EntradaSequenciaBVazia);
            var (fracoesImpressas, saidaCompleta) = ExecutarECapturarTudo(entrada);

            Assert.Empty(fracoesImpressas);
            Assert.Contains("Erro: A sequência B está vazia.", saidaCompleta);
        }

        private static readonly Regex PadraoFracaoComBarra = new Regex(@"(-?\d+)/(-?\d+)", RegexOptions.Compiled);
        private static readonly Regex PadraoFracaoImpressa = new Regex(@"(-?\d+)/(-?\d+)", RegexOptions.Compiled);

        private static string ConverterParaFormatoDuasLinhas(string entradaComBarra)
        {
            // "num/den" -> "num\nden" (numerador e denominador em linhas separadas).
            return PadraoFracaoComBarra.Replace(entradaComBarra, "$1\n$2");
        }

        private static bool EhIgual((int, int)[] esperado, List<(int, int)> obtido)
        {
            if (esperado.Length != obtido.Count) return false;
            for (int i = 0; i < esperado.Length; i++)
                if (esperado[i] != obtido[i]) return false;
            return true;
        }

        private static List<(int, int)> ExecutarECapturarFracoesImpressas(string entradaSimulada)
        {
            var (fracoes, _) = ExecutarECapturarTudo(entradaSimulada);
            return fracoes;
        }

        private static (List<(int, int)> Fracoes, string SaidaCompleta) ExecutarECapturarTudo(string entradaSimulada)
        {
            using var leitorEntrada = new StringReader(entradaSimulada);
            using var escritorSaida = new StringWriter();
            var entradaOriginal = Console.In;
            var saidaOriginal = Console.Out;

            try
            {
                Console.SetIn(leitorEntrada);
                Console.SetOut(escritorSaida);
                CompareSequence.Main();
            }
            finally
            {
                Console.SetIn(entradaOriginal);
                Console.SetOut(saidaOriginal);
            }

            var saidaCompleta = escritorSaida.ToString();
            var fracoesEncontradas = new List<(int, int)>();

            foreach (var linha in saidaCompleta.Split('\n'))
            {
                var linhaLimpa = linha.Trim().TrimEnd('\r');
                // Frações impressas como "Fração: 3/1 (Valor: 3,0000)".
                if (!linhaLimpa.StartsWith("Fração:")) continue;

                var match = PadraoFracaoImpressa.Match(linhaLimpa);
                if (match.Success)
                {
                    fracoesEncontradas.Add((int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)));
                }
            }

            return (fracoesEncontradas, saidaCompleta);
        }
    }
}
