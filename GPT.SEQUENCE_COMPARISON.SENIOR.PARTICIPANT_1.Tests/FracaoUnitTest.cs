namespace GPT.SEQUENCE_COMPARISON.SENIOR.PARTICIPANT_1.Tests
{
    /// <summary>
    /// Testes da Questão 2 para GPT - Participante 1.
    ///
    /// ATENÇÃO - NÃO CONFORMIDADE FUNCIONAL GRAVE:
    /// O enunciado pede que o programa "imprima todas as frações da sequência B
    /// cujo valor seja maior do que pelo menos metade dos números da sequência A".
    /// O método CompararSequencias() NÃO implementa esse filtro. Em vez disso,
    /// calcula e imprime a MÉDIA de A e a MÉDIA de B. Por isso, os casos
    /// compartilhados de Tarefa2TestData.CasosObrigatoriosFiltro() NÃO PODEM ser
    /// aplicados a este participante — não há saída de frações filtradas para
    /// comparar. Os testes abaixo cobrem apenas o que É funcional: a classe
    /// Fracao (lógica pura) e os métodos de leitura de A e B.
    /// </summary>
    public class FracaoUnitTest
    {
        [Fact]
        [Trait("Escopo", "NaoConformidadeFuncionalGrave")]
        public void CompararSequencias_NaoImplementaFiltroDoEnunciado()
        {
            Assert.Fail(
                "CompararSequencias() implementa comparação de médias, não o filtro " +
                "de 'fração > metade dos elementos de A' exigido pelo enunciado. " +
                "Requisito 3 do enunciado: NÃO ATENDIDO. Casos de Tarefa2TestData " +
                "não aplicáveis a esta submissão.");
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fracao")]
        public void Fracao_Construtor_ComDenominadorZero_DeveLancarArgumentException()
        {
            Action act = () => new Fracao(1, 0);
            Assert.Throws<ArgumentException>(act);
        }

        [Theory]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fracao")]
        [InlineData(3, 4, 1, 2, true)]
        [InlineData(1, 2, 3, 4, false)]
        [InlineData(1, 2, 2, 4, false)]
        public void Fracao_IsGreater_ComparaCorretamente(
            int numA, int denA, int numB, int denB, bool esperado)
        {
            var a = new Fracao(numA, denA);
            var b = new Fracao(numB, denB);
            Assert.Equal(esperado, a.IsGreater(b));
        }

        [Theory]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fracao")]
        [InlineData(1, 2, 3, 4, true)]
        [InlineData(3, 4, 1, 2, false)]
        [InlineData(1, 2, 2, 4, false)]
        public void Fracao_IsLesser_ComparaCorretamente(
            int numA, int denA, int numB, int denB, bool esperado)
        {
            var a = new Fracao(numA, denA);
            var b = new Fracao(numB, denB);
            Assert.Equal(esperado, a.IsLesser(b));
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Fracao")]
        public void Fracao_ComDenominadorNegativo_CalculaValorCorreto()
        {
            var fracaoDupNegativa = new Fracao(-1, -2);
            var meio = new Fracao(1, 2);

            Assert.False(fracaoDupNegativa.IsLesser(meio));
            Assert.False(fracaoDupNegativa.IsGreater(meio));
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "LeituraA")]
        public void LerSequenciaA_LeituraNormal_ParaNoZero()
        {
            var comparador = ExecutarLerSequenciaA("3.5\n2.1\n7.0\n0\n");
            Assert.Equal(new[] { 3.5, 2.1, 7.0 }, comparador.SequenciaA);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "LeituraA")]
        public void LerSequenciaA_PrimeiraEntradaZero_SequenciaVazia()
        {
            var comparador = ExecutarLerSequenciaA("0\n");
            Assert.Empty(comparador.SequenciaA);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "LeituraA")]
        public void LerSequenciaA_PermiteValoresNegativos_ApenasZeroEncerra()
        {
            var comparador = ExecutarLerSequenciaA("-5\n3\n0\n");
            Assert.Equal(new[] { -5.0, 3.0 }, comparador.SequenciaA);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "LeituraB")]
        public void LerSequenciaB_LeituraNormal_ParaNaFracaoNegativa()
        {
            var comparador = ExecutarLerSequenciaB("3/4\n5/2\n-1/1\n");

            Assert.Equal(2, comparador.SequenciaB.Count);
            Assert.Equal(3, comparador.SequenciaB[0].Numerador);
            Assert.Equal(4, comparador.SequenciaB[0].Denominador);
            Assert.Equal(5, comparador.SequenciaB[1].Numerador);
            Assert.Equal(2, comparador.SequenciaB[1].Denominador);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "LeituraB")]
        public void LerSequenciaB_PrimeiraFracaoNegativa_SequenciaVazia()
        {
            var comparador = ExecutarLerSequenciaB("-1/2\n");
            Assert.Empty(comparador.SequenciaB);
        }

        [Fact]
        [Trait("Escopo", "AmbiguidadeDeEspecificacao")]
        [Trait("Componente", "LeituraB")]
        public void LerSequenciaB_FracaoZero_NaoEhTratadaComoSentinela()
        {
            var comparador = ExecutarLerSequenciaB("0/1\n3/4\n-1/2\n");

            Assert.Equal(2, comparador.SequenciaB.Count);
            Assert.Equal(0, comparador.SequenciaB[0].Numerador);
            Assert.Equal(3, comparador.SequenciaB[1].Numerador);
        }

        private static ComparadorSequencias ExecutarLerSequenciaA(string entradaSimulada)
        {
            var comparador = new ComparadorSequencias();
            using var leitorEntrada = new StringReader(entradaSimulada);
            var saidaOriginal = Console.Out;
            var entradaOriginal = Console.In;
            try
            {
                Console.SetIn(leitorEntrada);
                Console.SetOut(TextWriter.Null);
                comparador.LerSequenciaA();
            }
            finally
            {
                Console.SetIn(entradaOriginal);
                Console.SetOut(saidaOriginal);
            }
            return comparador;
        }

        private static ComparadorSequencias ExecutarLerSequenciaB(string entradaSimulada)
        {
            var comparador = new ComparadorSequencias();
            using var leitorEntrada = new StringReader(entradaSimulada);
            var saidaOriginal = Console.Out;
            var entradaOriginal = Console.In;
            try
            {
                Console.SetIn(leitorEntrada);
                Console.SetOut(TextWriter.Null);
                comparador.LerSequenciaB();
            }
            finally
            {
                Console.SetIn(entradaOriginal);
                Console.SetOut(saidaOriginal);
            }
            return comparador;
        }
    }
}
