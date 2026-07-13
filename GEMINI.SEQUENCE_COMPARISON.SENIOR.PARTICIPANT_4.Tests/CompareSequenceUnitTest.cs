using System.Text.RegularExpressions;
using Testes.Compartilhados.Tarefa2;

namespace GEMINI.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_4.Tests
{
    /// <summary>
    /// Testes da Questão 2 para Gemini - Participante 4 (CompareSequence).
    ///
    /// DESTAQUES:
    /// - Nome de classe conforme o enunciado.
    /// - Sequência A corretamente tipada como double.
    /// - Fraction normaliza o sinal no construtor (denominador sempre positivo),
    ///   evitando o bug de sentinela visto no Participante 3.
    ///
    /// NOTA DE DESIGN:
    /// IsLesser(double)/IsGreater(double) comparam a fração contra um double
    /// diretamente, não contra outra Fraction. Prático para este problema
    /// específico (evita converter A em frações), mas diverge da sugestão mais
    /// genérica do enunciado de comparar frações entre si.
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
        public void Fraction_ComDenominadorNegativo_NormalizaSinal()
        {
            var fracao = new Fraction(3, -4);

            Assert.Equal(-3, fracao.Numerator);
            Assert.Equal(4, fracao.Denominator);
        }

        [Theory]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        [InlineData(3, 4, 0.5, true)]   // 0.75 > 0.5
        [InlineData(1, 2, 0.75, false)] // 0.5 > 0.75 -> false
        public void Fraction_IsGreater_ComparaComDoubleCorretamente(long num, long den, double valor, bool esperado)
        {
            var fracao = new Fraction(num, den);
            Assert.Equal(esperado, fracao.IsGreater(valor));
        }

        [Theory]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        [InlineData(1, 2, 0.75, true)]  // 0.5 < 0.75
        [InlineData(3, 4, 0.5, false)]  // 0.75 < 0.5 -> false
        public void Fraction_IsLesser_ComparaComDoubleCorretamente(long num, long den, double valor, bool esperado)
        {
            var fracao = new Fraction(num, den);
            Assert.Equal(esperado, fracao.IsLesser(valor));
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        public void Fraction_ComDenominadorNegativo_IsGreater_ComparaCorretamenteAposNormalizacao()
        {
            // Fraction(3, -4) normaliza para -3/4 = -0.75 (negativa).
            // -0.75 não deve ser maior que 0.5.
            var fracao = new Fraction(3, -4);
            Assert.False(fracao.IsGreater(0.5));
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
            Assert.Contains("Nenhuma fração encontrada que satisfaça a condição.", saidaCompleta);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Run_SequenciaAVazia_ImprimeErro()
        {
            var (fracoesImpressas, saidaCompleta) = ExecutarRunECapturarTudo(Tarefa2TestData.EntradaSequenciaAVazia);

            Assert.Empty(fracoesImpressas);
            Assert.Contains("Erro: A sequência A não pode ser vazia.", saidaCompleta);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Run_SequenciaBVazia_ImprimeErro()
        {
            var (fracoesImpressas, saidaCompleta) = ExecutarRunECapturarTudo(Tarefa2TestData.EntradaSequenciaBVazia);

            Assert.Empty(fracoesImpressas);
            Assert.Contains("Erro: A sequência B não pode ser vazia.", saidaCompleta);
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
