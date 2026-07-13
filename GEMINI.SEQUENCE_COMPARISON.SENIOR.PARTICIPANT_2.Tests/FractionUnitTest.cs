using System.Text.RegularExpressions;
using Testes.Compartilhados.Tarefa2;

namespace GEMINI.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_2.Tests
{
    /// <summary>
    /// Testes da Questão 2 para Gemini - Participante 2.
    /// Implementa corretamente o requisito 3, com ">=" e divisão em ponto
    /// flutuante (mesma interpretação majoritária vista no GPT). Verificação de
    /// denominador zero feita ANTES de instanciar Fraction (evita depender de
    /// exceção para fluxo de controle normal).
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
        public void Fraction_NaoImplementaIsLesserOuIsGreaterOuIsNegative()
        {
            var tipo = typeof(Fraction);
            Assert.Null(tipo.GetMethod("IsLesser"));
            Assert.Null(tipo.GetMethod("IsGreater"));
            Assert.Null(tipo.GetMethod("IsNegative"));
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
            Assert.Contains("Nenhuma fração em B atendeu ao critério especificado.", saidaCompleta);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Main_SequenciaAVazia_ImprimeErro()
        {
            var (fracoesImpressas, saidaCompleta) = ExecutarMainECapturarTudo(Tarefa2TestData.EntradaSequenciaAVazia);

            Assert.Empty(fracoesImpressas);
            Assert.Contains("Erro: Ambas as sequências devem conter pelo menos um elemento para a comparação.", saidaCompleta);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Main_SequenciaBVazia_ImprimeErro()
        {
            var (fracoesImpressas, saidaCompleta) = ExecutarMainECapturarTudo(Tarefa2TestData.EntradaSequenciaBVazia);

            Assert.Empty(fracoesImpressas);
            Assert.Contains("Erro: Ambas as sequências devem conter pelo menos um elemento para a comparação.", saidaCompleta);
        }

        private static readonly Regex PadraoFracao = new Regex(@"(-?\d+)/(-?\d+)", RegexOptions.Compiled);

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
                // Só considera linhas que começam com "- " (prefixo usado para
                // frações impressas neste participante), evitando falsos positivos
                // em linhas de cabeçalho/instrução.
                if (!linhaLimpa.StartsWith("- ")) continue;

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
