using Testes.Compartilhados.Tarefa3;

namespace DEEPSEEK.STRING_ARRAY_ENCODING.PLENO.PARTICIPANT_6.Tests
{
    /// <summary>
    /// Testes da Questão 3 para DeepSeek - Participante 6, nível Pleno (MatrixString/MatrixException).
    ///
    /// NOTA: quase idêntico ao Participante 4 (Senior) -- mesmas mensagens de
    /// exceção ("Rows and columns must be positive integers greater than
    /// zero.", "Row or column index is out of bounds.", "Separator cannot be
    /// null."), mesma concatenação ingênua de strings em RowToString. Só troca
    /// as propriedades Rows/Columns por um método PrintMatrix() que escreve no
    /// console. Vale registrar essa similaridade quase literal entre
    /// participantes de níveis diferentes do mesmo LLM.
    ///
    /// MatrixException tem os 3 construtores, mas não é usada com o parameterless.
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
        [Trait("Componente", "Set")]
        public void Set_NaoUsaConstrutorPadraoDeMatrixException()
        {
            var matriz = new MatrixString(2, 2, "x");
            Action act = () => matriz.Set(-1, 0, "y");
            var ex = Assert.Throws<MatrixException>(act);
            Assert.Equal("Row or column index is out of bounds.", ex.Message);
        }

        [Fact]
        [Trait("Escopo", "NaoConformidadeDeConstrutorDeExcecao")]
        [Trait("Componente", "Construtor")]
        public void Construtor_NaoUsaConstrutorPadraoDeArgumentException()
        {
            Action act = () => new MatrixString(0, 5, "x");
            var ex = Assert.Throws<ArgumentException>(act);
            Assert.Equal("Rows and columns must be positive integers greater than zero.", ex.Message);
        }

        [Fact]
        [Trait("Escopo", "Extra")]
        [Trait("Componente", "PrintMatrix")]
        public void PrintMatrix_ImprimeConteudoDaMatrizNoConsole()
        {
            var matriz = new MatrixString(2, 2, "x");
            matriz.Set(0, 1, "y");

            var saidaOriginal = Console.Out;
            using var escritorSaida = new StringWriter();

            try
            {
                Console.SetOut(escritorSaida);
                matriz.PrintMatrix();
            }
            finally
            {
                Console.SetOut(saidaOriginal);
            }

            var saida = escritorSaida.ToString();
            Assert.Contains("x", saida);
            Assert.Contains("y", saida);
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
