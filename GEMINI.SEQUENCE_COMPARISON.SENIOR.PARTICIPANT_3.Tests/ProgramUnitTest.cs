using System.Text.RegularExpressions;
using Testes.Compartilhados.Tarefa2;

namespace GEMINI.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_3.Tests
{
    /// <summary>
    /// Testes da Questão 2 para Gemini - Participante 3.
    ///
    /// ATENÇÃO - NÃO CONFORMIDADE DE TIPO (igual ao GPT Participante 3):
    /// A é List&lt;int&gt;, não List&lt;double&gt; como o enunciado exige.
    ///
    /// ATENÇÃO - FORMATO DE ENTRADA DIFERENTE:
    /// Frações são lidas como "numerador denominador" (separadas por ESPAÇO),
    /// não "numerador/denominador" (formato usado por todos os outros ~15
    /// participantes já analisados). O enunciado não fixa o separador exato,
    /// então isso não é uma violação rígida, mas é um outlier notável.
    ///
    /// ATENÇÃO - BUG GRAVE NO SENTINELA DE FRAÇÃO NEGATIVA:
    /// A condição de parada verifica APENAS "num &lt; 0", assumindo denominador
    /// positivo (conforme o próprio comentário do código admite, sem validar).
    /// Uma fração como "3 -4" (valor real -0.75, negativo) NÃO é detectada como
    /// sentinela — é adicionada normalmente a B, quando deveria encerrar a
    /// leitura e ser descartada. Isso também compromete IsGreater/IsLesser para
    /// tais frações, já que a multiplicação cruzada só é válida com
    /// denominadores positivos.
    /// </summary>
    public class ProgramUnitTest
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
        [InlineData(3, 4, 1, 2, true)]
        [InlineData(1, 2, 3, 4, false)]
        public void Fraction_IsGreater_ComDenominadoresPositivos_ComparaCorretamente(
            int numA, int denA, int numB, int denB, bool esperado)
        {
            var a = new Fraction(numA, denA);
            var b = new Fraction(numB, denB);
            Assert.Equal(esperado, a.IsGreater(b));
        }

        [Fact]
        [Trait("Escopo", "NaoConformidadeGrave")]
        [Trait("Componente", "Fraction")]
        public void Fraction_IsGreater_ComDenominadorNegativo_ComparacaoIncorreta()
        {
            // 3/-4 = -0.75 (negativa). 1/2 = 0.5 (positiva). -0.75 NÃO é maior que 0.5.
            // Mas a multiplicação cruzada sem normalização de sinal calcula:
            // this.Numerator * other.Denominator = 3 * 2 = 6
            // other.Numerator * this.Denominator = 1 * (-4) = -4
            // 6 > -4 -> TRUE, resultado INCORRETO (deveria ser false).
            var fracaoComDenominadorNegativo = new Fraction(3, -4); // valor real: -0.75
            var meio = new Fraction(1, 2); // valor real: 0.5

            // Documentando o comportamento real (incorreto) da implementação:
            Assert.True(fracaoComDenominadorNegativo.IsGreater(meio));
            // O valor matematicamente correto seria False, já que -0.75 < 0.5.
        }

        [Fact]
        [Trait("Escopo", "NaoConformidadeDeTipoGrave")]
        public void SequenciaA_ValoresDecimaisSaoSilenciosamenteIgnorados()
        {
            var (_, saidaCompleta) = ExecutarRunECapturarTudo("3.5\n2.5\n0\n-1 1\n");

            // Como nenhum valor decimal é aceito, e nenhum inteiro foi digitado,
            // A permanece vazio, disparando o erro de sequência vazia mesmo
            // com entradas numéricas legítimas (double) fornecidas pelo usuário.
            Assert.Contains("Erro: A lista A não pode ser vazia.", saidaCompleta);
        }

        [Fact]
        [Trait("Escopo", "NaoConformidadeGrave")]
        [Trait("Componente", "LeituraB")]
        public void LerSequenciaB_FracaoComDenominadorNegativoENumeradorPositivo_NaoEhDetectadaComoSentinela()
        {
            // "3 -4" tem valor real -0.75 (negativo), mas o numerador (3) não é
            // negativo, então o sentinela ("num < 0") não dispara. A fração é
            // incorretamente adicionada a B, e a leitura só termina na entrada
            // seguinte "-1 1" (numerador genuinamente negativo).
            var programa = new Program();
            using var leitorEntrada = new StringReader("5\n0\n3 -4\n-1 1\n");
            var entradaOriginal = Console.In;
            var saidaOriginal = Console.Out;

            try
            {
                Console.SetIn(leitorEntrada);
                Console.SetOut(TextWriter.Null);
                programa.Run();
            }
            finally
            {
                Console.SetIn(entradaOriginal);
                Console.SetOut(saidaOriginal);
            }

            var campoB = typeof(Program).GetField("B", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var listaB = (List<Fraction>)campoB.GetValue(programa);

            // Comportamento real (incorreto): B contém a fração "3/-4", que
            // deveria ter sido tratada como sentinela e excluída.
            Assert.Single(listaB);
            Assert.Equal(3, listaB[0].Numerator);
            Assert.Equal(-4, listaB[0].Denominator);
        }

        [Theory]
        [MemberData(nameof(Tarefa2TestData.CasosObrigatoriosFiltro), MemberType = typeof(Tarefa2TestData))]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "FiltroPrincipal")]
        public void Run_CasosCompartilhados(string cenario, string entradaConsoleComBarra, (int Numerador, int Denominador)[] esperado)
        {
            var entradaComEspaco = ConverterFracoesParaFormatoEspaco(entradaConsoleComBarra);
            var fracoesImpressas = ExecutarRunECapturarFracoesImpressas(entradaComEspaco);

            Assert.True(
                EhIgual(esperado, fracoesImpressas),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", fracoesImpressas)}]");
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Run_SequenciaAVazia_ImprimeErro()
        {
            var (_, saidaCompleta) = ExecutarRunECapturarTudo("0\n-1 1\n");
            Assert.Contains("Erro: A lista A não pode ser vazia.", saidaCompleta);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Run_SequenciaBVazia_ImprimeErro()
        {
            var (_, saidaCompleta) = ExecutarRunECapturarTudo("1\n2\n0\n-1 1\n");
            Assert.Contains("Erro: A lista B não pode ser vazia.", saidaCompleta);
        }

        private static readonly Regex PadraoFracaoComBarra = new Regex(@"(-?\d+)/(-?\d+)", RegexOptions.Compiled);
        private static readonly Regex PadraoFracaoImpressa = new Regex(@"^(-?\d+)/(-?\d+)$", RegexOptions.Compiled);

        private static string ConverterFracoesParaFormatoEspaco(string entradaComBarra)
        {
            // Só afeta tokens "num/den" (seção B); a seção A usa inteiros simples,
            // sem barra, então não é afetada por esta substituição.
            return PadraoFracaoComBarra.Replace(entradaComBarra, "$1 $2");
        }

        private static bool EhIgual((int, int)[] esperado, List<(int, int)> obtido)
        {
            if (esperado.Length != obtido.Count) return false;
            for (int i = 0; i < esperado.Length; i++)
                if (esperado[i] != obtido[i]) return false;
            return true;
        }

        private static List<(int, int)> ExecutarRunECapturarFracoesImpressas(string entradaSimulada)
        {
            var (fracoes, _) = ExecutarRunECapturarTudo(entradaSimulada);
            return fracoes;
        }

        private static (List<(int, int)> Fracoes, string SaidaCompleta) ExecutarRunECapturarTudo(string entradaSimulada)
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

            var saidaCompleta = escritorSaida.ToString();
            var fracoesEncontradas = new List<(int, int)>();

            foreach (var linha in saidaCompleta.Split('\n'))
            {
                var linhaLimpa = linha.Trim().TrimEnd('\r');
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
