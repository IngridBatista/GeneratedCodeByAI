using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Text.RegularExpressions;
using Testes.Compartilhados.Tarefa2;

namespace GPT.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_2.Tests
{
    /// <summary>
    /// Testes da Questão 2 para GPT - Participante 2.
    /// Implementa corretamente o requisito 3 (filtro), usando divisão em ponto
    /// flutuante (Count / 2.0). Os casos obrigatórios de filtro vêm de
    /// Tarefa2TestData.CasosObrigatoriosFiltro(), compartilhados entre participantes.
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
        [InlineData(3, 4, 0.75)]
        [InlineData(1, 2, 0.5)]
        [InlineData(-1, 2, -0.5)]
        public void Fraction_ToDecimal_CalculaValorCorreto(int num, int den, double esperado)
        {
            var fracao = new Fraction(num, den);
            Assert.Equal(esperado, fracao.ToDecimal(), precision: 10);
        }

        [Fact]
        [Trait("Escopo", "NaoConformidadeDeAssinatura")]
        public void Fraction_NaoImplementaIsLesserOuIsGreater()
        {
            var tipo = typeof(Fraction);
            Assert.Null(tipo.GetMethod("IsLesser"));
            Assert.Null(tipo.GetMethod("IsGreater"));
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
        [Trait("Componente", "SequenciaVazia")]
        public void Main_SequenciaAVazia_ImprimeErroENenhumaFracao()
        {
            var (fracoesImpressas, saidaCompleta) = ExecutarMainECapturarTudo(Tarefa2TestData.EntradaSequenciaAVazia);

            Assert.Empty(fracoesImpressas);
            Assert.Contains("Erro: Pelo menos uma das sequências está vazia.", saidaCompleta);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Main_SequenciaBVazia_ImprimeErroENenhumaFracao()
        {
            var (fracoesImpressas, saidaCompleta) = ExecutarMainECapturarTudo(Tarefa2TestData.EntradaSequenciaBVazia);

            Assert.Empty(fracoesImpressas);
            Assert.Contains("Erro: Pelo menos uma das sequências está vazia.", saidaCompleta);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Main_AmbasSequenciasVazias_ImprimeErroENenhumaFracao()
        {
            var (fracoesImpressas, saidaCompleta) = ExecutarMainECapturarTudo(Tarefa2TestData.EntradaAmbasSequenciasVazias);

            Assert.Empty(fracoesImpressas);
            Assert.Contains("Erro: Pelo menos uma das sequências está vazia.", saidaCompleta);
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
                Program.Main(Array.Empty<string>());
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
