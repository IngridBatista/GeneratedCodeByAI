using Testes.Compartilhados.Tarefa3;

namespace GPT.STRING_ARRAY_ENCODING.SENIOR.PARTICIPANT_5.Tests
{
    /// <summary>
    /// Testes da Questão 3 para GPT - Participante 5 (MatrixString/MatrixException).
    ///
    /// DESTAQUE POSITIVO: MatrixException É usada corretamente com o construtor
    /// padrão em Set() e RowToString() ("throw new MatrixException();").
    ///
    /// ATENÇÃO - ERRO SUTIL NO CONSTRUTOR:
    /// "throw new ArgumentException(paramName);" passa "rows"/"columns" como a
    /// MENSAGEM da exceção (via ArgumentException(string message)), não como
    /// ParamName. O construtor ArgumentException(string) NÃO define a
    /// propriedade ParamName -- apenas Message. Isso significa que
    /// ex.Message == "rows" (texto literal) e ex.ParamName == null, um
    /// resultado provavelmente não intencional (parece confundir a intenção de
    /// "informar qual parâmetro" com "usar o nome do parâmetro como mensagem").
    /// Também diverge da instrução de usar o construtor padrão para
    /// ArgumentException.
    /// </summary>
    public class MatrixExceptionUnitTest
    {
        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "MatrixException")]
        public void MatrixException_ConstrutorPadrao_CriaExcecaoDoTipoCorreto()
        {
            var excecao = new MatrixException();
            Assert.IsAssignableFrom<Exception>(excecao);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "Set")]
        public void Set_UsaConstrutorPadraoDeMatrixException_Corretamente()
        {
            var matriz = new MatrixString(2, 2, "x");
            Action act = () => matriz.Set(-1, 0, "y");
            var ex = Assert.Throws<MatrixException>(act);

            Assert.Equal(new MatrixException().Message, ex.Message);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "RowToString")]
        public void RowToString_UsaConstrutorPadraoDeMatrixException_Corretamente()
        {
            var matriz = new MatrixString(2, 2, "x");
            Action act = () => matriz.RowToString(5, ",");
            var ex = Assert.Throws<MatrixException>(act);

            Assert.Equal(new MatrixException().Message, ex.Message);
        }

        [Fact]
        [Trait("Escopo", "NaoConformidadeDeConstrutorDeExcecao")]
        [Trait("Componente", "Construtor")]
        public void Construtor_ComRowsInvalido_UsaNomeDoParametroComoMensagemNaoComoParamName()
        {
            Action act = () => new MatrixString(0, 5, "x");
            var ex = Assert.Throws<ArgumentException>(act);

            // ArgumentException(string message) só define Message, não ParamName.
            Assert.Equal("rows", ex.Message);
            Assert.Null(ex.ParamName); // provavelmente não intencional
        }

        [Fact]
        [Trait("Escopo", "NaoConformidadeDeConstrutorDeExcecao")]
        [Trait("Componente", "Construtor")]
        public void Construtor_ComColumnsInvalido_UsaNomeDoParametroComoMensagemNaoComoParamName()
        {
            Action act = () => new MatrixString(5, 0, "x");
            var ex = Assert.Throws<ArgumentException>(act);

            Assert.Equal("columns", ex.Message);
            Assert.Null(ex.ParamName);
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
