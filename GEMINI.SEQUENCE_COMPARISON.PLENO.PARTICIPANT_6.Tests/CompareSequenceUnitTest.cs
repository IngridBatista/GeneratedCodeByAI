using System.Text.RegularExpressions;
using Testes.Compartilhados.Tarefa2;

namespace GEMINI.SEQUENCE_COMPARISON.PLENO.PARTICIPANT_6.Tests
{
    /// <summary>
    /// Testes da Questão 2 para Gemini - Participante 6, nível Pleno (CompareSequence).
    ///
    /// DESTAQUES:
    /// - Fraction reduz automaticamente à forma canônica via MDC e normaliza o sinal.
    /// - Fraction.Parse() estático, com FormatException para formato inválido e
    ///   ArgumentException para denominador zero — separação clara de responsabilidades.
    /// - Implementa IComparable&lt;Fraction&gt; além de IsLesser()/IsGreater().
    /// </summary>
    public class CompareSequenceUnitTest
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
        [InlineData(1, 2, 2, 4, false)] // equivalentes (2/4 reduz para 1/2) -> não é maior
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

        [Theory]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        [InlineData(1, 2, 3, 4, -1)]
        [InlineData(3, 4, 1, 2, 1)]
        [InlineData(1, 2, 2, 4, 0)]
        public void Fraction_CompareTo_ComparaCorretamente(long numA, long denA, long numB, long denB, int esperado)
        {
            var a = new Fraction(numA, denA);
            var b = new Fraction(numB, denB);
            Assert.Equal(esperado, a.CompareTo(b));
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        public void Fraction_Parse_ComFormatoValido_CriaFracaoCorreta()
        {
            var fracao = Fraction.Parse("3/4");
            Assert.Equal(3, fracao.Numerator);
            Assert.Equal(4, fracao.Denominator);
        }

        [Theory]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("3/4/5")]
        [InlineData("abc/4")]
        public void Fraction_Parse_ComFormatoInvalido_DeveLancarFormatException(string entrada)
        {
            Action act = () => Fraction.Parse(entrada);
            Assert.Throws<FormatException>(act);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        public void Fraction_Parse_ComDenominadorZero_DeveLancarArgumentException()
        {
            // O erro de denominador zero vem do construtor, propagado através do Parse.
            Action act = () => Fraction.Parse("3/0");
            Assert.Throws<ArgumentException>(act);
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
        [Trait("Componente", "FiltroPrincipal")]
        public void Run_NenhumaFracaoAtendeAoCriterio_ImprimeMensagemInformativa()
        {
            var (_, saidaCompleta) = ExecutarRunECapturarTudo("5\n6\n7\n0\n1/10\n-1/1\n");
            Assert.Contains("Nenhuma fração da sequência B atendeu ao critério.", saidaCompleta);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Run_SequenciaAVazia_ImprimeErro()
        {
            var (fracoesImpressas, saidaCompleta) = ExecutarRunECapturarTudo(Tarefa2TestData.EntradaSequenciaAVazia);

            Assert.Empty(fracoesImpressas);
            Assert.Contains("Erro: A sequência A está vazia. A execução será interrompida.", saidaCompleta);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Run_SequenciaBVazia_ImprimeErro()
        {
            var (fracoesImpressas, saidaCompleta) = ExecutarRunECapturarTudo(Tarefa2TestData.EntradaSequenciaBVazia);

            Assert.Empty(fracoesImpressas);
            Assert.Contains("Erro: A sequência B está vazia. A execução será interrompida.", saidaCompleta);
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
            var (fracoes, _) = ExecutarRunECapturarTudo(entradaSimulada);
            return fracoes;
        }

        private static (List<(int, int)> Fracoes, string SaidaCompleta) ExecutarRunECapturarTudo(string entradaSimulada)
        {
            var comparador = new CompareSequence();
            using var leitorEntrada = new StringReader(entradaSimulada);
            using var escritorSaida = new StringWriter();
            var entradaOriginal = Console.In;
            var saidaOriginal = Console.Out;

            try
            {
                Console.SetIn(leitorEntrada);
                Console.SetOut(escritorSaida);
                comparador.Run();
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
