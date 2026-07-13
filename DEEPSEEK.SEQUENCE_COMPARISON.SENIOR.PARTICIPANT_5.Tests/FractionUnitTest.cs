using System.Text.RegularExpressions;
using Testes.Compartilhados.Tarefa2;

namespace DEEPSEEK.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_5.Tests
{
    /// <summary>
    /// Testes da Questão 2 para DeepSeek - Participante 5 (CompareSequence).
    ///
    /// ATENÇÃO - MESMO ALGORITMO DE MEDIANA ESTRUTURALMENTE INCORRETO DO
    /// PARTICIPANTE 2: CalculateThreshold ordena A, pega o elemento no índice
    /// Count/2, e compara cada fração de B contra esse ÚNICO valor de limiar,
    /// em vez de contar quantos elementos de A cada fração individualmente
    /// supera. Coincide com o algoritmo correto em casos sem duplicatas (exceto
    /// no limite exato de empate), mas diverge de forma mais severa com valores
    /// duplicados em A -- ver teste "Execute_ComDuplicatasEmA_AlgoritmoDeMedianaDivergeDoCorreto".
    ///
    /// Também em inglês (segundo caso, junto com o Participante 4), e usa a
    /// mesma frase exata "No fractions meet the criteria." para ausência de
    /// resultados -- possível padrão de fala do DeepSeek nesse tipo de mensagem.
    ///
    /// Sentinela de B usa fraction.Value &lt; 0 (valor computado, sem bug),
    /// diferente do Participante 2 (que verificava a string bruta).
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
        [MemberData(nameof(Tarefa2TestData.CasosObrigatoriosFiltro), MemberType = typeof(Tarefa2TestData))]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "FiltroPrincipal")]
        public void Execute_CasosCompartilhados(string cenario, string entradaConsole, (int Numerador, int Denominador)[] esperado)
        {
            var fracoesImpressas = ExecutarECapturarFracoesImpressas(entradaConsole);

            Assert.True(
                EhIgual(esperado, fracoesImpressas),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", fracoesImpressas)}]" +
                (cenario == "ContagemParNoLimiarExato_DeveIncluir"
                    ? " (DIVERGÊNCIA ESPERADA: algoritmo de mediana, mesmo problema do Participante 2 — ver nota da classe)"
                    : ""));
        }

        [Fact]
        [Trait("Escopo", "NaoConformidadeGrave")]
        [Trait("Componente", "FiltroPrincipal")]
        public void Execute_ComDuplicatasEmA_AlgoritmoDeMedianaDivergeDoCorreto()
        {
            // A = [1,1,3,3] (n=4). Fração 3/2 (valor=1.5).
            // Critério CORRETO: elementos de A estritamente menores que 1.5 são {1,1} -> count=2.
            // threshold = 4/2.0 = 2.0. 2 >= 2.0 -> TRUE -> deveria ser INCLUÍDA.
            //
            // Algoritmo desta implementação: sortedA=[1,1,3,3], middleIndex=4/2=2,
            // sortedA[2]=3 (limiar). Critério: 1.5 > 3 -> FALSO -> EXCLUÍDA (incorretamente).
            var entrada = "1\n1\n3\n3\n0\n3/2\n-1/1\n";

            var fracoesImpressas = ExecutarECapturarFracoesImpressas(entrada);

            Assert.Empty(fracoesImpressas); // comportamento real (incorreto) desta implementação
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "FiltroPrincipal")]
        public void Execute_NenhumaFracaoAtendeAoCriterio_ImprimeMensagemInformativa()
        {
            var (_, saidaCompleta) = ExecutarECapturarTudo("5\n6\n7\n0\n1/10\n-1/1\n");
            Assert.Contains("No fractions meet the criteria.", saidaCompleta);
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
            using var leitorEntrada = new StringReader(entradaSimulada);
            using var escritorSaida = new StringWriter();
            var entradaOriginal = Console.In;
            var saidaOriginal = Console.Out;

            try
            {
                Console.SetIn(leitorEntrada);
                Console.SetOut(escritorSaida);
                CompareSequence.Execute();
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
