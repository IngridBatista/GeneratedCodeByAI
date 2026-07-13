using System.Text.RegularExpressions;
using Testes.Compartilhados.Tarefa2;

namespace GEMINI.SEQUENCE_COMPARISON.JUNIOR.PARTICIPANT_7.Tests
{
    /// <summary>
    /// Testes da Questão 2 para Gemini - Participante 7, nível Junior (CompareSequence).
    ///
    /// DESTAQUES:
    /// - API mais completa do conjunto Gemini: IsGreater/IsLesser tanto para
    ///   Fraction quanto para double; implementa IComparable&lt;Fraction&gt; E
    ///   IComparable&lt;double&gt; simultaneamente.
    /// - CompareTo(null) retorna 1 em vez de lançar exceção — convenção mais
    ///   idiomática do .NET para IComparable (qualquer instância é "maior" que null).
    ///
    /// ATENÇÃO - DIVERGÊNCIA REAL NO LIMIAR (parecida com GPT Participante 5,
    /// mas por causa dupla):
    /// FindMatchingFractions usa "int requiredCount = sequenceA.Count / 2"
    /// (DIVISÃO INTEIRA) combinado com "countGreaterThan > requiredCount"
    /// (ESTRITAMENTE maior). Para n ÍMPAR, essas duas escolhas se cancelam e o
    /// resultado coincide com a interpretação majoritária (>=  n/2.0). Mas para
    /// n PAR, a combinação cria um critério mais rígido que o pretendido no
    /// caso de empate exato — o caso compartilhado
    /// "ContagemParNoLimiarExato_DeveIncluir" É ESPERADO QUE FALHE aqui.
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
        public void Fraction_IsGreater_ComOutraFracao_ComparaCorretamente(long numA, long denA, long numB, long denB, bool esperado)
        {
            var a = new Fraction((int)numA, (int)denA);
            var b = new Fraction((int)numB, (int)denB);
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

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        public void Fraction_CompareTo_ComNulo_RetornaUm_ConvencaoIdiomatica()
        {
            var fracao = new Fraction(1, 2);
            // Convenção idiomática de IComparable: instância é sempre "maior" que null.
            Assert.Equal(1, fracao.CompareTo((Fraction)null));
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
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", fracoesImpressas)}]" +
                (cenario == "ContagemParNoLimiarExato_DeveIncluir"
                    ? " (DIVERGÊNCIA ESPERADA: divisão inteira + '>' estrito tornam o critério mais rígido para n par — ver nota da classe)"
                    : ""));
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        [Trait("Componente", "FiltroPrincipal")]
        public void Run_DocumentaDivergenciaNoLimiteExatoParaNPar()
        {
            // A = [1,2,3,4] (n=4). Fração 3/1 (valor=3): elementos menores são {1,2} -> count=2.
            // requiredCount = 4/2 = 2 (int). Critério "count > requiredCount": 2 > 2 -> FALSO -> EXCLUÍDA.
            // Participantes com critério ">=" (float) incluiriam esta mesma fração.
            var entrada = "1\n2\n3\n4\n0\n3/1\n-1/1\n";

            var fracoesImpressas = ExecutarRunECapturarFracoesImpressas(entrada);

            Assert.Empty(fracoesImpressas); // comportamento real e documentado deste participante
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
            Assert.Contains("Erro: Ambas as sequências A e B devem conter pelo menos um elemento.", saidaCompleta);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Run_SequenciaBVazia_ImprimeErro()
        {
            var (fracoesImpressas, saidaCompleta) = ExecutarRunECapturarTudo(Tarefa2TestData.EntradaSequenciaBVazia);

            Assert.Empty(fracoesImpressas);
            Assert.Contains("Erro: Ambas as sequências A e B devem conter pelo menos um elemento.", saidaCompleta);
        }

        private static readonly Regex PadraoFracao = new Regex(@"^-\s(-?\d+)/(-?\d+)$", RegexOptions.Compiled);

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
