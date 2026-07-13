using System.Text.RegularExpressions;
using Testes.Compartilhados.Tarefa2;

namespace DEEPSEEK.SEQUENCE_COMPARISON.JUNIOR.PARTICIPANT_7.Tests
{
    /// <summary>
    /// Testes da Questão 2 para DeepSeek - Participante 7, nível Junior (CompareSequence).
    ///
    /// ATENÇÃO - TERCEIRA OCORRÊNCIA DO MESMO ALGORITMO DE MEDIANA INCORRETO
    /// (idêntico aos Participantes 2 e 5): ordena A, pega o elemento no índice
    /// Count/2, e compara cada fração contra esse único limiar. Reforça a
    /// hipótese de que essa é uma tendência sistemática do DeepSeek para esse
    /// tipo de problema.
    ///
    /// NOTA TÉCNICA: ToString() omite o denominador quando é 1 (ex: "10" em vez
    /// de "10/1") — formato de saída diferente de todos os demais participantes,
    /// exigindo ajuste no parser de saída dos testes.
    ///
    /// NOTA TÉCNICA: usa .Select() (LINQ) sem "using System.Linq;" explícito —
    /// pode ser erro de compilação dependendo de ImplicitUsings no projeto.
    ///
    /// NOTA: IsGreater(null)/IsLesser(null) retornam false diretamente, mas
    /// CompareTo(null) retorna 1 (convenção IComparable) — inconsistência
    /// interna sutil, já que CompareTo(null) > 0 logicamente seria true.
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
        public void Fraction_Simplifica_ViaMdc()
        {
            var fracao = new Fraction(4, 8);
            Assert.Equal(1, fracao.Numerator);
            Assert.Equal(2, fracao.Denominator);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        public void Fraction_ComNumeradorZero_SimplificaParaZeroSobreUm()
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
        public void Fraction_IsGreater_ComparaCorretamente(int numA, int denA, int numB, int denB, bool esperado)
        {
            var a = new Fraction(numA, denA);
            var b = new Fraction(numB, denB);
            Assert.Equal(esperado, a.IsGreater(b));
        }

        [Fact]
        [Trait("Escopo", "Robustez")]
        [Trait("Componente", "Fraction")]
        public void Fraction_IsGreater_ComNulo_RetornaFalse_MesmoCompareToRetornandoPositivo()
        {
            // Inconsistência sutil: CompareTo(null) retorna 1 (convenção IComparable
            // padrão), mas IsGreater(null) intercepta o caso e retorna false
            // diretamente, sem delegar para CompareTo.
            var fracao = new Fraction(1, 2);

            Assert.False(fracao.IsGreater(null));
            Assert.Equal(1, fracao.CompareTo(null));
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        public void Fraction_ToString_OmiteDenominadorQuandoIgualAUm()
        {
            var fracaoInteira = new Fraction(10, 1);
            var fracaoNormal = new Fraction(3, 4);

            Assert.Equal("10", fracaoInteira.ToString());
            Assert.Equal("3/4", fracaoNormal.ToString());
        }

        [Theory]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        [InlineData("3/4", true)]
        [InlineData("invalido", false)]
        [InlineData("3/0", false)]
        public void Fraction_TryParse_ComportaSeCorretamente(string entrada, bool esperado)
        {
            var resultado = Fraction.TryParse(entrada, out _);
            Assert.Equal(esperado, resultado);
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
                    ? " (DIVERGÊNCIA ESPERADA: mesmo algoritmo de mediana dos Participantes 2 e 5 — ver nota da classe)"
                    : ""));
        }

        [Fact]
        [Trait("Escopo", "NaoConformidadeGrave")]
        [Trait("Componente", "FiltroPrincipal")]
        public void Execute_ComDuplicatasEmA_AlgoritmoDeMedianaDivergeDoCorreto()
        {
            // Mesmo caso de divergência severa demonstrado nos Participantes 2 e 5:
            // A = [1,1,3,3], fração 3/2 (=1.5) deveria ser INCLUÍDA (supera 2 de 4,
            // exatamente metade), mas o algoritmo de mediana a EXCLUI incorretamente.
            var entrada = "1\n1\n3\n3\n0\n3/2\n-1/1\n";

            var fracoesImpressas = ExecutarECapturarFracoesImpressas(entrada);

            Assert.Empty(fracoesImpressas); // comportamento real (incorreto)
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
            Assert.Contains("Erro: Uma ou ambas as sequências estão vazias.", saidaCompleta);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Execute_SequenciaBVazia_ImprimeErro()
        {
            var (fracoesImpressas, saidaCompleta) = ExecutarECapturarTudo(Tarefa2TestData.EntradaSequenciaBVazia);

            Assert.Empty(fracoesImpressas);
            Assert.Contains("Erro: Uma ou ambas as sequências estão vazias.", saidaCompleta);
        }

        private static readonly Regex PadraoFracao = new Regex(@"^(-?\d+)(?:/(-?\d+))?\s*=", RegexOptions.Compiled);

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

            var linhas = saidaCompleta.Split('\n');
            bool dentroDaSecaoDeResultados = false;

            foreach (var linhaBruta in linhas)
            {
                var linha = linhaBruta.Trim().TrimEnd('\r');

                if (linha.StartsWith("Frações da sequência B maiores que"))
                {
                    dentroDaSecaoDeResultados = true;
                    continue;
                }

                // Para de capturar ao chegar na seção de debug (evita capturar o
                // dump completo de todos os elementos de B, não só os filtrados).
                if (linha.StartsWith("---"))
                {
                    dentroDaSecaoDeResultados = false;
                    continue;
                }

                if (!dentroDaSecaoDeResultados) continue;

                var match = PadraoFracao.Match(linha);
                if (match.Success)
                {
                    var numerador = int.Parse(match.Groups[1].Value);
                    var denominador = match.Groups[2].Success ? int.Parse(match.Groups[2].Value) : 1;
                    fracoesEncontradas.Add((numerador, denominador));
                }
            }

            return (fracoesEncontradas, saidaCompleta);
        }
    }
}
