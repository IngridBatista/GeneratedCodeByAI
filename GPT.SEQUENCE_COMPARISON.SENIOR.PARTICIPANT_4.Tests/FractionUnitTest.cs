using System.Text.RegularExpressions;
using Testes.Compartilhados.Tarefa2;

namespace GPT.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_4.Tests
{
    /// <summary>
    /// Testes da Questão 2 para GPT - Participante 4 (CompareSequence).
    ///
    /// DESTAQUES DE CONFORMIDADE:
    /// - Primeiro participante a usar o nome de classe exato pedido no enunciado
    ///   ("CompareSequence").
    /// - Sequência A corretamente tipada como List&lt;double&gt;.
    /// - Fraction usa "long" (mais robusto que "int") e normaliza o sinal no
    ///   construtor (denominador sempre positivo), o que simplifica IsNegative().
    /// - IsLesser()/IsGreater() usam multiplicação cruzada (sem conversão para
    ///   double), evitando problemas de precisão, e validam argumento nulo.
    /// - Denominador zero lança DivideByZeroException (escolha de exceção
    ///   diferente de outros participantes que usam ArgumentException — ambas
    ///   são semanticamente defensáveis).
    ///
    /// Os métodos de leitura e o filtro estão organizados em métodos privados
    /// estáticos separados (ReadSequenceA, ReadSequenceB,
    /// PrintFractionsGreaterThanHalfOfA), mas não expostos publicamente — os
    /// testes de filtro continuam via Main() com redirecionamento de console,
    /// usando os casos compartilhados de Tarefa2TestData.
    /// </summary>
    public class FractionUnitTest
    {
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
        public void Fraction_ComDenominadorNegativo_NormalizaSinalParaODenominador()
        {
            // Fraction(3, -4) deveria normalizar para Numerator=-3, Denominator=4.
            var fracao = new Fraction(3, -4);

            Assert.Equal(-3, fracao.Numerator);
            Assert.Equal(4, fracao.Denominator);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        public void Fraction_ComAmbosNegativos_NormalizaParaValorPositivo()
        {
            // Fraction(-3, -4) deveria normalizar para Numerator=3, Denominator=4.
            var fracao = new Fraction(-3, -4);

            Assert.Equal(3, fracao.Numerator);
            Assert.Equal(4, fracao.Denominator);
        }

        [Theory]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        [InlineData(-3, 4, true)]
        [InlineData(3, -4, true)]  // normaliza para Numerator=-3, ainda negativa
        [InlineData(-3, -4, false)] // normaliza para positiva
        [InlineData(3, 4, false)]
        [InlineData(0, 4, false)]
        public void Fraction_IsNegative_DetectaCorretamenteAposNormalizacao(long num, long den, bool esperado)
        {
            var fracao = new Fraction(num, den);
            Assert.Equal(esperado, fracao.IsNegative());
        }

        [Theory]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        [InlineData(3, 4, 1, 2, true)]
        [InlineData(1, 2, 3, 4, false)]
        [InlineData(1, 2, 2, 4, false)]
        public void Fraction_IsGreater_ComparaCorretamentePorMultiplicacaoCruzada(
            long numA, long denA, long numB, long denB, bool esperado)
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
        [InlineData(1, 2, 2, 4, false)]
        public void Fraction_IsLesser_ComparaCorretamentePorMultiplicacaoCruzada(
            long numA, long denA, long numB, long denB, bool esperado)
        {
            var a = new Fraction(numA, denA);
            var b = new Fraction(numB, denB);
            Assert.Equal(esperado, a.IsLesser(b));
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        public void Fraction_IsGreater_ComArgumentoNulo_DeveLancarArgumentNullException()
        {
            var fracao = new Fraction(1, 2);
            Action act = () => fracao.IsGreater(null);
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fraction")]
        public void Fraction_IsLesser_ComArgumentoNulo_DeveLancarArgumentNullException()
        {
            var fracao = new Fraction(1, 2);
            Action act = () => fracao.IsLesser(null);
            Assert.Throws<ArgumentNullException>(act);
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
        public void Main_SequenciaAVazia_ImprimeErro()
        {
            var (fracoesImpressas, saidaCompleta) = ExecutarMainECapturarTudo(Tarefa2TestData.EntradaSequenciaAVazia);

            Assert.Empty(fracoesImpressas);
            Assert.Contains("Erro: A sequência A está vazia. Encerrando execução.", saidaCompleta);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Main_SequenciaBVazia_ImprimeErro()
        {
            var (fracoesImpressas, saidaCompleta) = ExecutarMainECapturarTudo(Tarefa2TestData.EntradaSequenciaBVazia);

            Assert.Empty(fracoesImpressas);
            Assert.Contains("Erro: A sequência B está vazia. Encerrando execução.", saidaCompleta);
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
                CompareSequence.Main(Array.Empty<string>());
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
