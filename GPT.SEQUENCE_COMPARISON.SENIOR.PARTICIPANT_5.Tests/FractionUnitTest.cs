using System.Text.RegularExpressions;
using Testes.Compartilhados.Tarefa2;

namespace GPT.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_5.Tests
{
    /// <summary>
    /// Testes da Questão 2 para GPT - Participante 5 (CompareSequence.Run()).
    ///
    /// DESTAQUES:
    /// - Fraction reduz automaticamente à forma canônica via MDC no construtor.
    /// - Implementa IComparable&lt;Fraction&gt; e adiciona IsEqual(), além de
    ///   IsLesser()/IsGreater().
    /// - Run() é público e estático, dispensando Main() e reflection para testes.
    ///
    /// ATENÇÃO - DIVERGÊNCIA REAL DE INTERPRETAÇÃO DO ENUNCIADO:
    /// O filtro usa "greaterCount > thresholdCount" (ESTRITAMENTE maior), enquanto
    /// os Participantes 2, 3 e 4 usam ">=" (ou Ceiling, equivalente). Isso só
    /// diverge no caso de empate exato (n par, contagem == n/2): este participante
    /// EXCLUI a fração nesse caso; os outros três INCLUEM. Não é um bug — é uma
    /// leitura diferente e defensável de "maior do que pelo menos metade". O caso
    /// compartilhado "ContagemParNoLimiarExato_DeveIncluir" É ESPERADO QUE FALHE
    /// aqui; isso é o dado, não um erro de configuração do teste.
    /// </summary>
    public class FractionUnitTest
    {
        // =====================================================================
        // Classe Fraction - lógica pura, 100% testável
        // =====================================================================

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        public void Fraction_Construtor_ComDenominadorZero_DeveLancarDivideByZeroException()
        {
            Action act = () => new Fraction(1, 0);
            Assert.Throws<DivideByZeroException>(act);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        public void Fraction_ReduzAFormaCanonica_ViaMdc()
        {
            // Fraction(4, 8) deveria ser reduzida internamente para 1/2.
            var fracao = new Fraction(4, 8);

            Assert.Equal(1, fracao.Numerator);
            Assert.Equal(2, fracao.Denominator);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        public void Fraction_ReduzAFormaCanonica_ComNumeradorNegativo()
        {
            var fracao = new Fraction(-4, 8);

            Assert.Equal(-1, fracao.Numerator);
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

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        public void Fraction_IsEqual_ComFracoesEquivalentesDeEscalasDiferentes_RetornaTrue()
        {
            // Graças à redução automática por MDC, 2/4 e 1/2 devem ser consideradas
            // iguais tanto pela representação interna quanto pelo CompareTo.
            var a = new Fraction(2, 4);
            var b = new Fraction(1, 2);

            Assert.True(a.IsEqual(b));
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        public void Fraction_CompareTo_ComArgumentoNulo_DeveLancarArgumentNullException()
        {
            var fracao = new Fraction(1, 2);
            Action act = () => fracao.CompareTo(null);
            Assert.Throws<ArgumentNullException>(act);
        }

        [Theory]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        [InlineData(-3, 4, true)]
        [InlineData(3, -4, true)]  // normaliza para Numerator=-3
        [InlineData(-3, -4, false)]
        [InlineData(0, 4, false)]
        public void Fraction_IsNegative_DetectaCorretamente(long num, long den, bool esperado)
        {
            var fracao = new Fraction(num, den);
            Assert.Equal(esperado, fracao.IsNegative());
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
                    ? " (DIVERGÊNCIA ESPERADA: este participante usa '>' estrito, não '>=' — ver nota da classe)"
                    : ""));
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        [Trait("Componente", "FiltroPrincipal")]
        public void Run_DocumentaDivergenciaDeInterpretacaoNoLimiteExato()
        {
            // A = [1,2,3,4] (n=4). Fração 3/1 (valor=3): elementos menores são {1,2} -> count=2.
            // threshold = 4/2.0 = 2.0. Critério "count > threshold": 2 > 2.0 -> FALSO -> EXCLUÍDA.
            // Participantes 2, 3 e 4 (critério ">="), incluiriam esta mesma fração.
            var entrada = "1\n2\n3\n4\n0\n3/1\n-1/1\n";

            var fracoesImpressas = ExecutarRunECapturarFracoesImpressas(entrada);

            Assert.Empty(fracoesImpressas); // comportamento real e documentado deste participante
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Run_SequenciaAVazia_ImprimeErro()
        {
            var (fracoesImpressas, saidaCompleta) = ExecutarRunECapturarTudo(Tarefa2TestData.EntradaSequenciaAVazia);

            Assert.Empty(fracoesImpressas);
            Assert.Contains("Erro: A sequência A está vazia. Encerrando execução.", saidaCompleta);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Run_SequenciaBVazia_ImprimeErro()
        {
            var (fracoesImpressas, saidaCompleta) = ExecutarRunECapturarTudo(Tarefa2TestData.EntradaSequenciaBVazia);

            Assert.Empty(fracoesImpressas);
            Assert.Contains("Erro: A sequência B está vazia. Encerrando execução.", saidaCompleta);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "FiltroPrincipal")]
        public void Run_NenhumaFracaoAtendeAoCriterio_ImprimeMensagemInformativa()
        {
            // Comportamento extra: diferente de outros participantes que simplesmente
            // não imprimem nada, este informa explicitamente que nenhuma fração qualificou.
            var (_, saidaCompleta) = ExecutarRunECapturarTudo("5\n6\n7\n0\n1/10\n-1/1\n");

            Assert.Contains("Nenhuma fração atende ao critério especificado.", saidaCompleta);
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
            using var leitorEntrada = new StringReader(entradaSimulada);
            using var escritorSaida = new StringWriter();
            var entradaOriginal = Console.In;
            var saidaOriginal = Console.Out;

            try
            {
                Console.SetIn(leitorEntrada);
                Console.SetOut(escritorSaida);
                CompareSequence.Run();
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
