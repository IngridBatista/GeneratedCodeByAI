using System.Text.RegularExpressions;
using Testes.Compartilhados.Tarefa2;

namespace CLAUDE.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_5.Tests
{
    /// <summary>
    /// Testes da Questão 2 para Claude - Participante 5 (CompareSequence).
    ///
    /// Usa a MESMA fórmula de mediana estatisticamente correta do Participante 2
    /// (média dos dois valores centrais para n par). Espera-se o mesmo padrão:
    /// passa em todos os 7 casos compartilhados, mas ainda diverge no teste de
    /// duplicatas (mesma limitação estrutural de fundo).
    ///
    /// DIFERENÇA DE COMPORTAMENTO: PrintFractions() não imprime NENHUMA mensagem
    /// quando a lista de resultados está vazia (diferente de todos os demais
    /// participantes, que sempre imprimem algo como "Nenhuma fração atende...").
    ///
    /// Mensagens de erro em inglês, com o mesmo texto exato do DeepSeek
    /// Participante 5 ("Sequence A is empty."/"Sequence B is empty.") — possível
    /// coincidência de frase entre LLMs para esse tipo de mensagem.
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
        [InlineData(3, 4, 0.5, true)]
        [InlineData(1, 2, 0.75, false)]
        public void Fraction_IsGreater_ComparaCorretamente(int num, int den, double valor, bool esperado)
        {
            var fracao = new Fraction(num, den);
            Assert.Equal(esperado, fracao.IsGreater(valor));
        }

        [Theory]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        [InlineData(-1, 2, true)]
        [InlineData(1, -2, true)]
        [InlineData(1, 2, false)]
        public void Fraction_IsNegative_DetectaCorretamente(int num, int den, bool esperado)
        {
            var fracao = new Fraction(num, den);
            Assert.Equal(esperado, fracao.IsNegative());
        }

        [Theory]
        [MemberData(nameof(Tarefa2TestData.CasosObrigatoriosFiltro), MemberType = typeof(Tarefa2TestData))]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "FiltroPrincipal")]
        public void Execute_CasosCompartilhados(string cenario, string entradaConsole, (int Numerador, int Denominador)[] esperado)
        {
            // Assim como o Participante 2, a mediana correta faz este participante
            // passar em todos os 7 casos compartilhados.
            var fracoesImpressas = ExecutarECapturarFracoesImpressas(entradaConsole);

            Assert.True(
                EhIgual(esperado, fracoesImpressas),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", fracoesImpressas)}]");
        }

        [Fact]
        [Trait("Escopo", "NaoConformidadeGrave")]
        [Trait("Componente", "FiltroPrincipal")]
        public void Execute_ComDuplicatasEmA_MedianaCorretaAindaDivergeDoAlgoritmoCorreto()
        {
            // Mesmo caso do Participante 2: A=[1,1,3,3], mediana=(1+3)/2=2.0.
            // Fração 3/2 (valor=1.5). Critério: 1.5>2.0 -> FALSO -> EXCLUÍDA (incorreto).
            // Critério correto: count(A<1.5)={1,1}=2 >= 4/2.0=2.0 -> deveria ser INCLUÍDA.
            var entrada = "1\n1\n3\n3\n0\n3/2\n-1/1\n";

            var fracoesImpressas = ExecutarECapturarFracoesImpressas(entrada);

            Assert.Empty(fracoesImpressas); // comportamento real (incorreto)
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "FiltroPrincipal")]
        public void Execute_NenhumaFracaoAtendeAoCriterio_NaoImprimeNadaAlemDoEsperado()
        {
            // Diferente de todos os outros participantes: não há mensagem
            // explícita de "nenhuma fração encontrada" -- apenas ausência de
            // linhas de fração na saída.
            var (fracoesImpressas, _) = ExecutarECapturarTudo("5\n6\n7\n0\n1/10\n-1/1\n");
            Assert.Empty(fracoesImpressas);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Execute_SequenciaAVazia_ImprimeErro()
        {
            var (fracoesImpressas, saidaCompleta) = ExecutarECapturarTudo(Tarefa2TestData.EntradaSequenciaAVazia);

            Assert.Empty(fracoesImpressas);
            Assert.Contains("Error: Sequence A is empty.", saidaCompleta);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Execute_SequenciaBVazia_ImprimeErro()
        {
            var (fracoesImpressas, saidaCompleta) = ExecutarECapturarTudo(Tarefa2TestData.EntradaSequenciaBVazia);

            Assert.Empty(fracoesImpressas);
            Assert.Contains("Error: Sequence B is empty.", saidaCompleta);
        }

        private static readonly Regex PadraoFracao = new Regex(@"^(-?\d+)/(-?\d+)$", RegexOptions.Compiled);

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
            var comparador = new CompareSequence();
            using var leitorEntrada = new StringReader(entradaSimulada);
            using var escritorSaida = new StringWriter();
            var entradaOriginal = Console.In;
            var saidaOriginal = Console.Out;

            try
            {
                Console.SetIn(leitorEntrada);
                Console.SetOut(escritorSaida);
                comparador.Execute();
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
