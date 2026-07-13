using System.Text.RegularExpressions;
using Testes.Compartilhados.Tarefa2;

namespace CLAUDE.SEQUENCE_COMPARISON.PLENO.PARTICIPANT_6.Tests
{
    /// <summary>
    /// Testes da Questão 2 para Claude - Participante 6, nível Pleno (CompareSequence).
    ///
    /// ACHADO DE QUALIDADE DE CÓDIGO: ProcessAndPrint() calcula "medianValue"
    /// (usando sortedA[halfIndex], a mesma abordagem de índice cru vista em
    /// vários participantes do DeepSeek) mas NUNCA USA essa variável no filtro
    /// real. O filtro de fato usa "countLesser >= sequenceA.Count / 2.0" — o
    /// algoritmo CORRETO de contagem, não o de mediana. Parece código residual
    /// de uma abordagem abandonada, sem impacto funcional (a variável morta não
    /// afeta o resultado), mas é um sinal de qualidade de código a registrar.
    ///
    /// Por usar o algoritmo correto de contagem, este é o primeiro participante
    /// da "família de mediana" (Claude/DeepSeek) a passar no teste de duplicatas
    /// que quebrou todos os anteriores que usavam mediana como limiar.
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
        [InlineData(3, 4, 1, 2, true)]
        [InlineData(1, 2, 3, 4, false)]
        public void Fraction_IsGreater_ComOutraFracao_ComparaCorretamente(int numA, int denA, int numB, int denB, bool esperado)
        {
            var a = new Fraction(numA, denA);
            var b = new Fraction(numB, denB);
            Assert.Equal(esperado, a.IsGreater(b));
        }

        [Theory]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        [InlineData(3, 4, 0.5, true)]
        [InlineData(1, 2, 0.75, false)]
        public void Fraction_IsGreater_ComDouble_ComparaCorretamente(int num, int den, double valor, bool esperado)
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
            var fracoesImpressas = ExecutarECapturarFracoesImpressas(entradaConsole);

            Assert.True(
                EhIgual(esperado, fracoesImpressas),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", fracoesImpressas)}]");
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "FiltroPrincipal")]
        public void Execute_ComDuplicatasEmA_AlgoritmoCorretoIncluiCorretamente()
        {
            // A = [1,1,3,3] (n=4). Fração 3/2 (valor=1.5).
            // countLesser = elementos de A < 1.5 = {1,1} = 2. threshold = 4/2.0 = 2.0.
            // 2 >= 2.0 -> TRUE -> INCLUÍDA corretamente.
            //
            // Diferente de TODOS os participantes anteriores que usavam mediana
            // como limiar único (DeepSeek P2/P5/P7, Claude P2/P5), que excluíam
            // incorretamente este mesmo caso -- aqui o algoritmo de contagem
            // correto resolve isso sem problemas.
            var entrada = "1\n1\n3\n3\n0\n3/2\n-1/1\n";

            var fracoesImpressas = ExecutarECapturarFracoesImpressas(entrada);

            Assert.Single(fracoesImpressas);
            Assert.Equal((3, 2), fracoesImpressas[0]);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "FiltroPrincipal")]
        public void Execute_NenhumaFracaoAtendeAoCriterio_ImprimeMensagemInformativa()
        {
            var (_, saidaCompleta) = ExecutarECapturarTudo("5\n6\n7\n0\n1/10\n-1/1\n");
            Assert.Contains("Nenhuma fração atende ao critério.", saidaCompleta);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Execute_SequenciaAVazia_ImprimeErro()
        {
            var (fracoesImpressas, saidaCompleta) = ExecutarECapturarTudo(Tarefa2TestData.EntradaSequenciaAVazia);

            Assert.Empty(fracoesImpressas);
            Assert.Contains("Erro: A sequência A está vazia. Não é possível continuar.", saidaCompleta);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Execute_SequenciaBVazia_ImprimeErro()
        {
            var (fracoesImpressas, saidaCompleta) = ExecutarECapturarTudo(Tarefa2TestData.EntradaSequenciaBVazia);

            Assert.Empty(fracoesImpressas);
            Assert.Contains("Erro: A sequência B está vazia. Não é possível continuar.", saidaCompleta);
        }

        private static readonly Regex PadraoFracao = new Regex(@"^(-?\d+)/(-?\d+)", RegexOptions.Compiled);

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
