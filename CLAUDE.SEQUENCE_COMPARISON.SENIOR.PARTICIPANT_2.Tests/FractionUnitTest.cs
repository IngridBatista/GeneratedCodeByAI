using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Text.RegularExpressions;
using Testes.Compartilhados.Tarefa2;

namespace CLAUDE.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_2.Tests
{
    /// <summary>
    /// Testes da Questão 2 para Claude - Participante 2.
    ///
    /// ALGORITMO: usa a MEDIANA ESTATÍSTICA CORRETA (média dos dois valores
    /// centrais para n par), diferente da versão "crua" (apenas sortedA[Count/2])
    /// vista em 3 participantes do DeepSeek. Isso faz com que este participante
    /// PASSE em todos os 7 casos compartilhados de Tarefa2TestData, incluindo o
    /// caso de empate exato que quebrava as versões do DeepSeek.
    ///
    /// PORÉM: usar qualquer forma de mediana como limiar único ainda NÃO é
    /// estruturalmente equivalente ao algoritmo pedido (contar quantos elementos
    /// de A cada fração individualmente supera). O teste
    /// "Main_ComDuplicatasEmA_MedianaCorretaAindaDivergeDoAlgoritmoCorreto"
    /// demonstra isso com um caso específico onde mesmo a mediana matematicamente
    /// correta produz um resultado diferente do exigido pelo enunciado.
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
        [MemberData(nameof(Tarefa2TestData.CasosObrigatoriosFiltro), MemberType = typeof(Tarefa2TestData))]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "FiltroPrincipal")]
        public void Main_CasosCompartilhados(string cenario, string entradaConsole, (int Numerador, int Denominador)[] esperado)
        {
            // Diferente do DeepSeek (índice cru), a mediana estatística correta
            // deste participante passa em TODOS os 7 casos compartilhados,
            // incluindo o de empate exato.
            var fracoesImpressas = ExecutarMainECapturarFracoesImpressas(entradaConsole);

            Assert.True(
                EhIgual(esperado, fracoesImpressas),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", fracoesImpressas)}]");
        }

        [Fact]
        [Trait("Escopo", "NaoConformidadeGrave")]
        [Trait("Componente", "FiltroPrincipal")]
        public void Main_ComDuplicatasEmA_MedianaCorretaAindaDivergeDoAlgoritmoCorreto()
        {
            // A = [1,1,3,3] (n=4). Mediana estatística correta = (1+3)/2 = 2.0.
            // Fração 3/2 (valor=1.5). Critério deste participante: 1.5 > 2.0 -> FALSO -> EXCLUÍDA.
            //
            // Critério CORRETO do enunciado: elementos de A estritamente menores
            // que 1.5 são {1,1} -> count=2. threshold=4/2.0=2.0. 2>=2.0 -> TRUE ->
            // deveria ser INCLUÍDA.
            //
            // Isso prova que mesmo com a mediana matematicamente correta (que
            // resolve o caso de empate simples), o uso de qualquer limiar único
            // baseado em mediana ainda não é equivalente ao algoritmo de
            // contagem exigido pelo enunciado quando A tem distribuição
            // assimétrica com duplicatas.
            var entrada = "1\n1\n3\n3\n0\n3/2\n-1/1\n";

            var fracoesImpressas = ExecutarMainECapturarFracoesImpressas(entrada);

            Assert.Empty(fracoesImpressas); // comportamento real (incorreto) desta implementação
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
            Assert.Contains("Erro: Ambas as sequências devem conter pelo menos um elemento.", saidaCompleta);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Main_SequenciaBVazia_ImprimeErro()
        {
            var (fracoesImpressas, saidaCompleta) = ExecutarMainECapturarTudo(Tarefa2TestData.EntradaSequenciaBVazia);

            Assert.Empty(fracoesImpressas);
            Assert.Contains("Erro: Ambas as sequências devem conter pelo menos um elemento.", saidaCompleta);
        }

        private static readonly Regex PadraoFracao = new Regex(@"^(-?\d+)/(-?\d+)", RegexOptions.Compiled);

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
