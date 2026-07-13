using System.Text.RegularExpressions;
using Testes.Compartilhados.Tarefa2;

namespace CLAUDE.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_4.Tests
{
    /// <summary>
    /// Testes da Questão 2 para Claude - Participante 4 (conforme namespace
    /// PARTICIPANT_4 no código-fonte; mencionado como "participante 3" na
    /// entrega — confirmar numeração antes de consolidar a tabela final).
    ///
    /// Implementação inteiramente em INGLÊS (terceiro caso, junto com DeepSeek
    /// Participantes 4 e 5).
    ///
    /// ATENÇÃO - DIVERGÊNCIA DE LIMIAR NA DIREÇÃO OPOSTA a tudo visto até agora:
    /// "int halfCount = sequenceA.Count / 2;" (divisão INTEIRA) combinado com
    /// "countGreater >= halfCount" (NÃO estrito). Para n PAR, isso funciona
    /// perfeitamente (sem perda de precisão na truncagem). Mas para n ÍMPAR, a
    /// truncagem torna o critério PERMISSIVO DEMAIS -- aceita uma contagem que
    /// deveria ser insuficiente. Isso é o oposto dos bugs vistos em GPT
    /// Participante 5 e Gemini Participante 7 (que eram RESTRITIVOS demais para
    /// n par). Aqui, "ContagemImparNoLimiar_DeveExcluir" é esperado que FALHE
    /// (inclui quando deveria excluir), enquanto "ContagemParNoLimiarExato_DeveIncluir"
    /// passa normalmente (sem divergência).
    /// </summary>
    public class FractionUnitTets
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
        [InlineData(-3, 4, true)]
        [InlineData(3, -4, true)]
        [InlineData(3, 4, false)]
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
                $"Falhou no cenário '{cenario}'. Esperado: [{string.Join(",", esperado)}], Obtido: [{string.Join(",", fracoesImpressas)}]" +
                (cenario == "ContagemImparNoLimiar_DeveExcluir"
                    ? " (DIVERGÊNCIA ESPERADA: divisão inteira do limiar torna o critério permissivo demais para n ímpar — ver nota da classe)"
                    : ""));
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        [Trait("Componente", "FiltroPrincipal")]
        public void Execute_DocumentaDivergenciaPermissivaParaNImpar()
        {
            // A = [1,2,3,4,5] (n=5, ímpar). Fração 3/1 (valor=3): elementos de A
            // estritamente menores que 3 são {1,2} -> countGreater=2.
            // halfCount = 5/2 = 2 (int, truncado). Critério: 2 >= 2 -> TRUE -> INCLUÍDA.
            // Critério correto (float): 2 >= 2.5 -> FALSO -> deveria ser EXCLUÍDA.
            var entrada = "1\n2\n3\n4\n5\n0\n3/1\n-1/1\n";

            var fracoesImpressas = ExecutarECapturarFracoesImpressas(entrada);

            // Comportamento real (permissivo demais) desta implementação:
            Assert.Single(fracoesImpressas);
            Assert.Equal((3, 1), fracoesImpressas[0]);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Execute_SequenciaAVazia_ImprimeErro()
        {
            var (fracoesImpressas, saidaCompleta) = ExecutarECapturarTudo(Tarefa2TestData.EntradaSequenciaAVazia);

            Assert.Empty(fracoesImpressas);
            Assert.Contains("Error: One or both sequences are empty.", saidaCompleta);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "SequenciaVazia")]
        public void Execute_SequenciaBVazia_ImprimeErro()
        {
            var (fracoesImpressas, saidaCompleta) = ExecutarECapturarTudo(Tarefa2TestData.EntradaSequenciaBVazia);

            Assert.Empty(fracoesImpressas);
            Assert.Contains("Error: One or both sequences are empty.", saidaCompleta);
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
