using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Text.RegularExpressions;
using Testes.Compartilhados.Tarefa2;

namespace DEEPSEEK.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_2.Tests
{
    /// <summary>
    /// Testes da Questão 2 para DeepSeek - Participante 2.
    ///
    /// ATENÇÃO - ALGORITMO DE FILTRO ESTRUTURALMENTE INCORRETO (mais grave que
    /// uma simples ambiguidade de limiar):
    /// Em vez de contar, para cada fração de B, quantos elementos de A ela supera,
    /// esta implementação ordena A, pega o elemento no índice Count/2 (uma
    /// "mediana") e compara cada fração de B contra esse ÚNICO valor de limiar.
    /// Isso coincide com o algoritmo correto em casos sem duplicatas (inclusive
    /// nos 7 casos compartilhados, exceto no de empate exato), mas diverge de
    /// forma mais severa com valores duplicados em A — ver teste específico
    /// "Main_ComDuplicatasEmA_AlgoritmoDeMedianaDivergeDoCorreto".
    ///
    /// ATENÇÃO - BUGS DE SENTINELA (verifica a STRING BRUTA, não o valor calculado):
    /// "3/-4" (valor real -0.75, negativa) não começa com "-", passa despercebida
    /// e é adicionada normalmente. "-3/-4" (valor real +0.75, positiva) começa
    /// com "-", é erroneamente tratada como sentinela.
    ///
    /// ATENÇÃO - TESTABILIDADE: sequenceA/sequenceB são variáveis locais em Main(),
    /// sem exposição alguma. Main() termina com Console.ReadKey() FORA do
    /// try/catch, que lança InvalidOperationException com entrada redirecionada —
    /// o harness de teste precisa capturar essa exceção esperada.
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
        public void Fraction_ToDecimal_CalculaValorCorreto(int num, int den, double esperado)
        {
            var fracao = new Fraction(num, den);
            Assert.Equal(esperado, fracao.ToDecimal(), precision: 10);
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
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", fracoesImpressas)}]" +
                (cenario == "ContagemParNoLimiarExato_DeveIncluir"
                    ? " (DIVERGÊNCIA ESPERADA: algoritmo de mediana, mesmo caso de empate exato — ver nota da classe)"
                    : ""));
        }

        [Fact]
        [Trait("Escopo", "NaoConformidadeGrave")]
        [Trait("Componente", "FiltroPrincipal")]
        public void Main_ComDuplicatasEmA_AlgoritmoDeMedianaDivergeDoCorreto()
        {
            // A = [1,1,3,3] (n=4). Fração 3/2 (valor=1.5).
            // Critério CORRETO: elementos de A estritamente menores que 1.5 são {1,1} -> count=2.
            // threshold = 4/2.0 = 2.0. 2 >= 2.0 -> TRUE -> deveria ser INCLUÍDA.
            //
            // Algoritmo desta implementação: sortedA=[1,1,3,3], halfIndex=4/2=2,
            // sortedA[2]=3 (limiar). Critério: 1.5 > 3 -> FALSO -> EXCLUÍDA
            // (incorretamente).
            //
            // Isso demonstra que o algoritmo não é apenas uma leitura alternativa
            // do "pelo menos metade" (como nos casos de empate exato vistos em
            // outros participantes) — é uma aproximação estruturalmente diferente
            // que pode divergir mesmo fora do limite exato, sempre que A tiver
            // distribuição de valores não-uniforme com duplicatas.
            var entrada = "1\n1\n3\n3\n0\n3/2\n-1/1\n";

            var fracoesImpressas = ExecutarMainECapturarFracoesImpressas(entrada);

            // Comportamento real (incorreto) desta implementação:
            Assert.Empty(fracoesImpressas);
        }

        [Fact]
        [Trait("Escopo", "NaoConformidadeGrave")]
        [Trait("Componente", "LeituraB")]
        public void Main_FracaoComDenominadorNegativo_NaoEhDetectadaComoSentinela()
        {
            // "3/-4" tem valor real -0.75 (negativa), mas a string não começa
            // com "-", então não dispara o sentinela. É adicionada normalmente
            // a B. Confirmamos indiretamente via a contagem impressa de elementos de B.
            var saida = ExecutarMainECapturarTudo("5\n0\n3/-4\n-1/1\n");

            Assert.Contains("Sequência B tem 1 elementos.", saida);
        }

        [Fact]
        [Trait("Escopo", "NaoConformidadeGrave")]
        [Trait("Componente", "LeituraB")]
        public void Main_FracaoComAmbosNegativos_EhErroneamenteTratadaComoSentinela()
        {
            // "-3/-4" tem valor real +0.75 (positiva), mas a string começa com
            // "-", então é (erroneamente) tratada como sentinela e a leitura para
            // sem adicionar nada a B, resultando em sequência B vazia.
            var saida = ExecutarMainECapturarTudo("5\n0\n-3/-4\n");

            Assert.Contains("Erro: Ambas as sequências devem conter pelo menos um elemento.", saida);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Main_SequenciaAVazia_ImprimeErro()
        {
            var saida = ExecutarMainECapturarTudo(Tarefa2TestData.EntradaSequenciaAVazia);
            Assert.Contains("Erro: Ambas as sequências devem conter pelo menos um elemento.", saida);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Main_SequenciaBVazia_ImprimeErro()
        {
            var saida = ExecutarMainECapturarTudo(Tarefa2TestData.EntradaSequenciaBVazia);
            Assert.Contains("Erro: Ambas as sequências devem conter pelo menos um elemento.", saida);
        }

        private static readonly Regex PadraoFracao = new Regex(@"(-?\d+)/(-?\d+)", RegexOptions.Compiled);

        private static bool EhIgual((int, int)[] esperado, System.Collections.Generic.List<(int, int)> obtido)
        {
            if (esperado.Length != obtido.Count) return false;
            for (int i = 0; i < esperado.Length; i++)
                if (esperado[i] != obtido[i]) return false;
            return true;
        }

        private static System.Collections.Generic.List<(int, int)> ExecutarMainECapturarFracoesImpressas(string entradaSimulada)
        {
            var saidaCompleta = ExecutarMainECapturarTudo(entradaSimulada);
            var fracoesEncontradas = new System.Collections.Generic.List<(int, int)>();

            foreach (var linha in saidaCompleta.Split('\n'))
            {
                var linhaLimpa = linha.Trim().TrimEnd('\r');
                // Frações do resultado final são impressas como "Fraçao: 3/4 = 0.7500".
                if (!linhaLimpa.StartsWith("Fraçao:")) continue;

                var match = PadraoFracao.Match(linhaLimpa);
                if (match.Success)
                {
                    fracoesEncontradas.Add((int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)));
                }
            }

            return fracoesEncontradas;
        }

        private static string ExecutarMainECapturarTudo(string entradaSimulada)
        {
            using var leitorEntrada = new StringReader(entradaSimulada);
            using var escritorSaida = new StringWriter();
            var entradaOriginal = Console.In;
            var saidaOriginal = Console.Out;

            try
            {
                Console.SetIn(leitorEntrada);
                Console.SetOut(escritorSaida);
                try
                {
                    Program.Main(Array.Empty<string>());
                }
                catch (InvalidOperationException)
                {
                    // Esperado: Console.ReadKey() falha com entrada redirecionada.
                    // A saída relevante já foi escrita antes deste ponto.
                }
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
