using Testes.Compartilhados.Tarefa3;

namespace GPT.STRING_ARRAY_ENCODING.SENIOR.PARTICIPANT_2.Tests
{
    /// <summary>
    /// Testes da Questão 3 para GPT - Participante 2 (MatrixString/MatrixException).
    ///
    /// ATENÇÃO - NÃO CONFORMIDADE COM INSTRUÇÃO EXPLÍCITA DO ENUNCIADO:
    /// O enunciado pede claramente "use o construtor padrão" tanto de
    /// ArgumentException quanto de MatrixException (sem mensagem customizada).
    /// Esta implementação usa ArgumentException(message, paramName) e
    /// MatrixException(message) em todos os pontos de lançamento, nunca os
    /// construtores parameterless. O TIPO da exceção continua correto (os
    /// testes de tipo ainda passam), mas é uma divergência clara e
    /// documentável de uma instrução literal do enunciado.
    ///
    /// Ponto positivo: MatrixException implementa os 3 construtores
    /// convencionais de Exception (parameterless, message, message+inner),
    /// API mais completa que o Participante 1 — só não é usada da forma pedida.
    /// </summary>
    public class MatrixExceptionUnitTest
    {
        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "MatrixException")]
        public void MatrixException_ConstrutorPadrao_ExisteECriaExcecaoDoTipoCorreto()
        {
            var excecao = new MatrixException();
            Assert.IsAssignableFrom<Exception>(excecao);
        }

        [Fact]
        [Trait("Escopo", "NaoConformidadeDeConstrutorDeExcecao")]
        [Trait("Componente", "Construtor")]
        public void Construtor_NaoUsaConstrutorPadraoDeArgumentException_UsaMensagemCustomizada()
        {
            // O enunciado pede explicitamente o construtor padrão (sem mensagem).
            // Esta implementação usa ArgumentException(message, paramName).
            Action act = () => new MatrixString(0, 5, "x");
            var ex = Assert.Throws<ArgumentException>(act);

            // ex.Message inclui "(Parameter 'rows')" anexado automaticamente pelo
            // .NET quando paramName é informado; verificamos apenas o prefixo customizado.
            Assert.StartsWith("rows deve ser positivo.", ex.Message);
            Assert.Equal("rows", ex.ParamName);
        }

        [Fact]
        [Trait("Escopo", "NaoConformidadeDeConstrutorDeExcecao")]
        [Trait("Componente", "Set")]
        public void Set_NaoUsaConstrutorPadraoDeMatrixException_UsaMensagemCustomizada()
        {
            var matriz = new MatrixString(2, 2, "x");
            Action act = () => matriz.Set(-1, 0, "y");
            var ex = Assert.Throws<MatrixException>(act);

            Assert.Equal("Índices fora dos limites da matriz.", ex.Message);
        }

        [Theory]
        [MemberData(nameof(Tarefa3TestData.CasosConstrutorValido), MemberType = typeof(Tarefa3TestData))]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Construtor")]
        public void Construtor_ComParametrosValidos_NaoLancaExcecao(int rows, int columns, string value)
        {
            var excecao = Record.Exception(() => new MatrixString(rows, columns, value));
            Assert.Null(excecao);
        }

        [Theory]
        [MemberData(nameof(Tarefa3TestData.CasosConstrutorInvalido), MemberType = typeof(Tarefa3TestData))]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Construtor")]
        public void Construtor_ComParametrosInvalidos_DeveLancarArgumentException(int rows, int columns)
        {
            Action act = () => new MatrixString(rows, columns, "x");
            Assert.Throws<ArgumentException>(act);
        }

        [Theory]
        [MemberData(nameof(Tarefa3TestData.CasosRowToStringAposConstrucao), MemberType = typeof(Tarefa3TestData))]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "RowToString")]
        public void RowToString_AposConstrucao_RetornaConcatenacaoCorreta(
            int rows, int columns, string value, int rowIndex, string separator, string esperado)
        {
            var matriz = new MatrixString(rows, columns, value);
            var resultado = matriz.RowToString(rowIndex, separator);
            Assert.Equal(esperado, resultado);
        }

        [Theory]
        [MemberData(nameof(Tarefa3TestData.CasosRowToStringIndiceInvalido), MemberType = typeof(Tarefa3TestData))]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "RowToString")]
        public void RowToString_ComIndiceInvalido_DeveLancarMatrixException(
            int rows, int columns, string value, int indiceInvalido)
        {
            var matriz = new MatrixString(rows, columns, value);
            Action act = () => matriz.RowToString(indiceInvalido, ",");
            Assert.Throws<MatrixException>(act);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "RowToString")]
        public void RowToString_ComSeparatorNulo_DeveLancarMatrixException()
        {
            var matriz = new MatrixString(2, 2, "x");
            Action act = () => matriz.RowToString(0, null);
            Assert.Throws<MatrixException>(act);
        }

        [Theory]
        [MemberData(nameof(Tarefa3TestData.CasosSetValidoERowToString), MemberType = typeof(Tarefa3TestData))]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Set")]
        public void Set_ComIndicesValidos_AlteraCelulaCorretamente(
            int rows, int columns, string valorInicial, int setRow, int setColumn,
            string novoValor, int rowParaVerificar, string separator, string esperado)
        {
            var matriz = new MatrixString(rows, columns, valorInicial);
            matriz.Set(setRow, setColumn, novoValor);

            var resultado = matriz.RowToString(rowParaVerificar, separator);
            Assert.Equal(esperado, resultado);
        }

        [Theory]
        [MemberData(nameof(Tarefa3TestData.CasosSetIndiceInvalido), MemberType = typeof(Tarefa3TestData))]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Set")]
        public void Set_ComIndicesInvalidos_DeveLancarMatrixException(
            int rows, int columns, int invalidRow, int invalidColumn)
        {
            var matriz = new MatrixString(rows, columns, "x");
            Action act = () => matriz.Set(invalidRow, invalidColumn, "novo");
            Assert.Throws<MatrixException>(act);
        }
    }
}
