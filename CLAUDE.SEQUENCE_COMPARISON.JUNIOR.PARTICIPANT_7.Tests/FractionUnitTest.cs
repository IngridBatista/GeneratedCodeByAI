using Testes.Compartilhados.Tarefa2;

namespace CLAUDE.SEQUENCE_COMPARISON.JUNIOR.PARTICIPANT_7.Tests
{
    /// <summary>
    /// Testes da Questão 2 para Claude - Participante 7, nível Junior (CompareSequence).
    ///
    /// DESTAQUE ARQUITETURAL: injeção de dependência completa (IInputReader,
    /// ISequenceAnalyzer, IOutputWriter), princípios SOLID bem aplicados. É o
    /// PRIMEIRO participante (entre ~31 analisados em 4 LLMs) cuja lógica central
    /// pode ser testada como unidade pura, sem qualquer simulação de console —
    /// SequenceAnalyzer.FilterFractionsGreaterThanMedian() é chamado diretamente
    /// com List&lt;double&gt;/List&lt;Fraction&gt; construídos em memória.
    ///
    /// O NOME DO MÉTODO ("FilterFractionsGreaterThanMedian") confirma
    /// explicitamente a conceitualização equivocada de "pelo menos metade" como
    /// "maior que a mediana" — mesma família de erro dos Participantes 2, 5
    /// (Claude) e 2, 5, 7 (DeepSeek). Usa a mediana matematicamente correta
    /// (média dos dois centrais para n par), então passa nos 7 casos
    /// compartilhados, mas ainda diverge no teste de duplicatas.
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

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "CompareSequence")]
        public void Construtor_ComInputReaderNulo_DeveLancarArgumentNullException()
        {
            Action act = () => new CompareSequence(null, new SequenceAnalyzer(), new ConsoleOutputWriter());
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "CompareSequence")]
        public void Construtor_ComAnalyzerNulo_DeveLancarArgumentNullException()
        {
            Action act = () => new CompareSequence(new ConsoleInputReader(), null, new ConsoleOutputWriter());
            Assert.Throws<ArgumentNullException>(act);
        }

        [Theory]
        [MemberData(nameof(Tarefa2TestData.CasosObrigatoriosFiltro), MemberType = typeof(Tarefa2TestData))]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "FiltroPrincipal")]
        public void FilterFractionsGreaterThanMedian_CasosCompartilhados(
            string cenario, string entradaConsole, (int Numerador, int Denominador)[] esperado)
        {
            var (sequenciaA, sequenciaB) = ConverterEntradaParaListas(entradaConsole);
            var analyzer = new SequenceAnalyzer();

            var resultado = analyzer.FilterFractionsGreaterThanMedian(sequenciaA, sequenciaB);

            var obtido = new List<(int, int)>();
            foreach (var f in resultado) obtido.Add((f.Numerator, f.Denominator));

            Assert.True(
                EhIgual(esperado, obtido),
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", obtido)}]");
        }

        [Fact]
        [Trait("Escopo", "NaoConformidadeGrave")]
        [Trait("Componente", "FiltroPrincipal")]
        public void FilterFractionsGreaterThanMedian_ComDuplicatasEmA_AindaDivergeDoAlgoritmoCorreto()
        {
            // A = [1,1,3,3]. Mediana correta = (1+3)/2 = 2.0. Fração 3/2 (=1.5).
            // 1.5 > 2.0 -> FALSO -> EXCLUÍDA (incorreto; deveria incluir, pois
            // supera 2 de 4 elementos, exatamente metade).
            var sequenciaA = new List<double> { 1, 1, 3, 3 };
            var sequenciaB = new List<Fraction> { new Fraction(3, 2) };
            var analyzer = new SequenceAnalyzer();

            var resultado = analyzer.FilterFractionsGreaterThanMedian(sequenciaA, sequenciaB);

            Assert.Empty(resultado); // comportamento real (incorreto) desta implementação
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void FilterFractionsGreaterThanMedian_SequenciaAVazia_DeveLancarInvalidOperationException()
        {
            var analyzer = new SequenceAnalyzer();
            Action act = () => analyzer.FilterFractionsGreaterThanMedian(new List<double>(), new List<Fraction> { new Fraction(1, 2) });

            var ex = Assert.Throws<InvalidOperationException>(act);
            Assert.Equal("Sequência A está vazia.", ex.Message);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void FilterFractionsGreaterThanMedian_SequenciaBVazia_DeveLancarInvalidOperationException()
        {
            var analyzer = new SequenceAnalyzer();
            Action act = () => analyzer.FilterFractionsGreaterThanMedian(new List<double> { 1, 2 }, new List<Fraction>());

            var ex = Assert.Throws<InvalidOperationException>(act);
            Assert.Equal("Sequência B está vazia.", ex.Message);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Execute_SequenciaAVazia_ImprimeErroViaOutputWriter()
        {
            var saida = ExecutarFluxoCompletoECapturarSaida(Tarefa2TestData.EntradaSequenciaAVazia);
            Assert.Contains("ERRO: Sequência A está vazia.", saida);
        }

        private static bool EhIgual((int, int)[] esperado, List<(int, int)> obtido)
        {
            if (esperado.Length != obtido.Count) return false;
            for (int i = 0; i < esperado.Length; i++)
                if (esperado[i] != obtido[i]) return false;
            return true;
        }

        private static (List<double>, List<Fraction>) ConverterEntradaParaListas(string entradaConsole)
        {
            var linhas = entradaConsole.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var sequenciaA = new List<double>();
            var sequenciaB = new List<Fraction>();
            int i = 0;

            while (i < linhas.Length)
            {
                var linha = linhas[i].Trim();
                i++;
                if (double.TryParse(linha, out double valor))
                {
                    if (valor == 0) break;
                    sequenciaA.Add(valor);
                }
            }

            while (i < linhas.Length)
            {
                var linha = linhas[i].Trim();
                i++;
                var partes = linha.Split('/');
                var numerador = int.Parse(partes[0]);
                var denominador = int.Parse(partes[1]);
                var fracao = new Fraction(numerador, denominador);
                if (fracao.IsNegative()) break;
                sequenciaB.Add(fracao);
            }

            return (sequenciaA, sequenciaB);
        }

        private static string ExecutarFluxoCompletoECapturarSaida(string entradaSimulada)
        {
            using var leitorEntrada = new StringReader(entradaSimulada);
            using var escritorSaida = new StringWriter();
            var entradaOriginal = Console.In;
            var saidaOriginal = Console.Out;

            try
            {
                Console.SetIn(leitorEntrada);
                Console.SetOut(escritorSaida);

                var comparador = new CompareSequence(
                    new ConsoleInputReader(),
                    new SequenceAnalyzer(),
                    new ConsoleOutputWriter());
                comparador.Execute();
            }
            finally
            {
                Console.SetIn(entradaOriginal);
                Console.SetOut(saidaOriginal);
            }

            return escritorSaida.ToString();
        }
    }
}
