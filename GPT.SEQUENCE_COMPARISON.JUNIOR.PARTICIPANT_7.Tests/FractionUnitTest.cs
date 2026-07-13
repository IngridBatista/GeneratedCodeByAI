using System.Text.RegularExpressions;
using Testes.Compartilhados.Tarefa2;

namespace GPT.SEQUENCE_COMPARISON.JUNIOR.PARTICIPANT_7.Tests
{
    /// <summary>
    /// Testes da Questão 2 para GPT - Participante 7, nível Junior (CompareSequence).
    ///
    /// DESTAQUES:
    /// - Nome de classe conforme o enunciado.
    /// - Fraction reduz automaticamente à forma canônica via MDC (igual a P5 e P6).
    /// - CompareTo usa bloco "checked", fazendo overflow na multiplicação cruzada
    ///   lançar OverflowException em vez de produzir um resultado incorreto
    ///   silenciosamente — nenhum outro participante fez essa proteção.
    /// - Main() envolve tudo em try/catch genérico, capturando qualquer exceção
    ///   inesperada com mensagem amigável.
    /// - Critério do limiar usa ">=" (mesma interpretação majoritária de P2, P3, P4, P6;
    ///   NÃO diverge como P5).
    /// </summary>
    public class FractionUnitTest
    {
        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        public void Fraction_Construtor_ComDenominadorZero_DeveLancarArgumentException()
        {
            Action act = () => new Fraction(1, 0);
            var ex = Assert.Throws<ArgumentException>(act);
            Assert.Equal("denominator", ex.ParamName);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        public void Fraction_ReduzAFormaCanonica_ViaMdc()
        {
            var fracao = new Fraction(4, 8);

            Assert.Equal(1, fracao.Numerator);
            Assert.Equal(2, fracao.Denominator);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        public void Fraction_ComNumeradorZero_ReduzParaZeroSobreUm()
        {
            var fracao = new Fraction(0, 5);

            Assert.Equal(0, fracao.Numerator);
            Assert.Equal(1, fracao.Denominator);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        public void Fraction_ComDenominadorNegativo_NormalizaSinal()
        {
            var fracao = new Fraction(3, -4);

            Assert.Equal(-3, fracao.Numerator);
            Assert.Equal(4, fracao.Denominator);
        }

        [Theory]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        [InlineData(3, 4, 1, 2, true)]
        [InlineData(1, 2, 3, 4, false)]
        public void Fraction_IsGreater_ComparaCorretamente(long numA, long denA, long numB, long denB, bool esperado)
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
        public void Fraction_IsLesser_ComparaCorretamente(long numA, long denA, long numB, long denB, bool esperado)
        {
            var a = new Fraction(numA, denA);
            var b = new Fraction(numB, denB);
            Assert.Equal(esperado, a.IsLesser(b));
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        public void Fraction_IsEqual_ComFracoesEquivalentes_RetornaTrue()
        {
            var a = new Fraction(2, 4);
            var b = new Fraction(1, 2);
            Assert.True(a.IsEqual(b));
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        public void Fraction_CompareTo_ComArgumentoNulo_DeveLancarArgumentNullException()
        {
            var fracao = new Fraction(1, 2);
            Action act = () => fracao.CompareTo(null);
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        [Trait("Escopo", "Robustez")]
        [Trait("Componente", "Fraction")]
        public void Fraction_CompareTo_ComOverflowNaMultiplicacaoCruzada_DeveLancarOverflowException()
        {
            // Único participante (entre todos os LLMs analisados até agora) a usar
            // bloco "checked" na multiplicação cruzada, fazendo overflow lançar
            // OverflowException explicitamente, em vez de produzir silenciosamente
            // um resultado de comparação incorreto (wraparound).
            var fracaoGrande = new Fraction(long.MaxValue, 1);
            var fracaoComDenominadorMaior = new Fraction(1, 2);

            Action act = () => fracaoGrande.CompareTo(fracaoComDenominadorMaior);

            Assert.Throws<OverflowException>(act);
        }

        [Theory]
        [MemberData(nameof(Tarefa2TestData.CasosObrigatoriosFiltro), MemberType = typeof(Tarefa2TestData))]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "FiltroPrincipal")]
        public void Main_CasosCompartilhados(string cenario, string entradaConsole, (int Numerador, int Denominador)[] esperado)
        {
            var fracoesImpressas = ExecutarMainECapturarFracoesImpressas(entradaConsole);

            Assert.True(
                EhIgual(esperado, fracoesImpressas),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", fracoesImpressas)}]");
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "FiltroPrincipal")]
        public void Main_NenhumaFracaoAtendeAoCriterio_ImprimeMensagemInformativa()
        {
            var (_, saidaCompleta) = ExecutarMainECapturarTudo("5\n6\n7\n0\n1/10\n-1/1\n");
            Assert.Contains("Nenhuma fração atende ao critério.", saidaCompleta);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Main_SequenciaAVazia_ImprimeErro()
        {
            var (fracoesImpressas, saidaCompleta) = ExecutarMainECapturarTudo(Tarefa2TestData.EntradaSequenciaAVazia);

            Assert.Empty(fracoesImpressas);
            Assert.Contains("Erro: A sequência A está vazia.", saidaCompleta);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Main_SequenciaBVazia_ImprimeErro()
        {
            var (fracoesImpressas, saidaCompleta) = ExecutarMainECapturarTudo(Tarefa2TestData.EntradaSequenciaBVazia);

            Assert.Empty(fracoesImpressas);
            Assert.Contains("Erro: A sequência B está vazia.", saidaCompleta);
        }

        private static readonly Regex PadraoFracao = new Regex(@"^(-?\d+)/(-?\d+)$", RegexOptions.Compiled);

        private static bool EhIgual((int, int)[] esperado, List<(int, int)> obtido)
        {
            if (esperado.Length != obtido.Count) return false;
            for (int i = 0; i < esperado.Length; i++)
                if (esperado[i] != obtido[i]) return false;
            return true;
        }

        private static List<(int, int)> ExecutarMainECapturarFracoesImpressas(string entradaSimulada)
        {
            var (fracoes, _) = ExecutarMainECapturarTudo(entradaSimulada);
            return fracoes;
        }

        private static (List<(int, int)> Fracoes, string SaidaCompleta) ExecutarMainECapturarTudo(string entradaSimulada)
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
                var match = PadraoFracao.Match(linhaLimpa);
                if (match.Success)
                {
                    fracoesEncontradas.Add((int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)));
                }
            }

            return (fracoesEncontradas, saidaCompleta);
        }
    }
}
