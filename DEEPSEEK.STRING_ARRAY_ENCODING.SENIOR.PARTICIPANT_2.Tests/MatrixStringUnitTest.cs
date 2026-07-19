using Testes.Compartilhados.Tarefa3;

namespace DEEPSEEK.STRING_ARRAY_ENCODING.SENIOR.PARTICIPANT_2.Tests
{
    /// <summary>
    /// Testes da Questão 3 para DeepSeek - Participante 2 (MatrixString/MatrixString.MatrixException).
    ///
    /// ATENÇÃO - ESTRUTURA DIFERENTE: MatrixException é uma CLASSE ANINHADA
    /// dentro de MatrixString (não uma classe de nível superior no namespace,
    /// como em todos os demais ~20 participantes analisados). O nome totalmente
    /// qualificado é MatrixString.MatrixException.
    ///
    /// ATENÇÃO - NÃO CONFORMIDADE GRAVE (mesmo padrão do GPT/Gemini Participante 3):
    /// MatrixException só possui o construtor com mensagem. Não existe
    /// construtor padrão -- "new MatrixString.MatrixException()" nem compilaria.
    ///
    /// ArgumentException também usa mensagem customizada + paramName, divergindo
    /// da instrução do enunciado.
    ///
    /// Extras (não pedidos, mas úteis): Rows/Columns como propriedades, e um
    /// método Get(row, column) para leitura direta de célula.
    /// </summary>
    public class MatrixStringUnitTest
    {
        [Fact]
        [Trait("Escopo", "NaoConformidadeGrave")]
        [Trait("Componente", "MatrixException")]
        public void MatrixException_NaoPossuiConstrutorPadrao()
        {
            var construtorPadrao = typeof(MatrixString.MatrixException).GetConstructor(Type.EmptyTypes);
            Assert.Null(construtorPadrao);
        }

        [Fact]
        [Trait("Escopo", "NaoConformidadeGrave")]
        [Trait("Componente", "MatrixException")]
        public void MatrixException_SoPossuiConstrutorComMensagem()
        {
            var construtorComMensagem = typeof(MatrixString.MatrixException).GetConstructor(new[] { typeof(string) });
            Assert.NotNull(construtorComMensagem);
        }

        [Fact]
        [Trait("Escopo", "Extra")]
        [Trait("Componente", "MatrixException")]
        public void MatrixException_EhClasseAninhadaDentroDeMatrixString()
        {
            var tipoExcecao = typeof(MatrixString.MatrixException);
            Assert.Equal(typeof(MatrixString), tipoExcecao.DeclaringType);
        }

        [Fact]
        [Trait("Escopo", "NaoConformidadeDeConstrutorDeExcecao")]
        [Trait("Componente", "Construtor")]
        public void Construtor_NaoUsaConstrutorPadraoDeArgumentException()
        {
            Action act = () => new MatrixString(0, 5, "x");
            var ex = Assert.Throws<ArgumentException>(act);
            Assert.StartsWith("O número de linhas deve ser positivo.", ex.Message);
            Assert.Equal("rows", ex.ParamName);
        }

        [Fact]
        [Trait("Escopo", "Extra")]
        [Trait("Componente", "Construtor")]
        public void Construtor_ExpoeRowsEColumnsComoPropriedades()
        {
            var matriz = new MatrixString(3, 5, "x");
            Assert.Equal(3, matriz.Rows);
            Assert.Equal(5, matriz.Columns);
        }

        [Fact]
        [Trait("Escopo", "Extra")]
        [Trait("Componente", "Get")]
        public void Get_RetornaValorDaCelulaCorretamente()
        {
            var matriz = new MatrixString(2, 2, "x");
            matriz.Set(1, 1, "y");
            Assert.Equal("y", matriz.Get(1, 1));
            Assert.Equal("x", matriz.Get(0, 0));
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
            Assert.Throws<MatrixString.MatrixException>(act);
        }

        [Fact]
        [Trait("Escopo", "Obrigatorio")]
        [Trait("Componente", "RowToString")]
        public void RowToString_ComSeparatorNulo_DeveLancarMatrixException()
        {
            var matriz = new MatrixString(2, 2, "x");
            Action act = () => matriz.RowToString(0, null);
            Assert.Throws<MatrixString.MatrixException>(act);
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
            Assert.Throws<MatrixString.MatrixException>(act);
        }
    }
}
