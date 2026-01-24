using System;

namespace DEEPSEEK.STRING_ARRAY_ENCODING.JUNIOR.PARTICIPANT_7
{
    // Exceção personalizada para erros específicos da matriz
    public class MatrixException : Exception
    {
        public MatrixException() : base() { }
        public MatrixException(string message) : base(message) { }
        public MatrixException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    public class MatrixString
    {
        private readonly string[,] _matrix;

        // Propriedades para acesso controlado às dimensões
        public int Rows => _matrix.GetLength(0);
        public int Columns => _matrix.GetLength(1);

        /// <summary>
        /// Construtor que inicializa a matriz com um valor padrão em todas as posições
        /// </summary>
        /// <param name="rows">Número de linhas (deve ser > 0)</param>
        /// <param name="columns">Número de colunas (deve ser > 0)</param>
        /// <param name="value">Valor inicial para todas as células</param>
        /// <exception cref="ArgumentException">Lançada quando rows ou columns são inválidos</exception>
        public MatrixString(int rows, int columns, string value)
        {
            // Validação dos parâmetros
            if (rows <= 0)
                throw new ArgumentException("O número de linhas deve ser maior que zero.", nameof(rows));

            if (columns <= 0)
                throw new ArgumentException("O número de colunas deve ser maior que zero.", nameof(columns));

            // Inicialização da matriz
            _matrix = new string[rows, columns];

            // Preenchimento com o valor padrão
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    _matrix[i, j] = value;
                }
            }
        }

        /// <summary>
        /// Define o valor de uma célula específica na matriz
        /// </summary>
        /// <param name="row">Índice da linha (base 0)</param>
        /// <param name="column">Índice da coluna (base 0)</param>
        /// <param name="value">Valor a ser atribuído</param>
        /// <exception cref="MatrixException">Lançada quando os índices estão fora dos limites</exception>
        public void Set(int row, int column, string value)
        {
            ValidateRowIndex(row);
            ValidateColumnIndex(column);

            _matrix[row, column] = value;
        }

        /// <summary>
        /// Concatena os elementos de uma linha específica usando um separador
        /// </summary>
        /// <param name="index">Índice da linha a ser concatenada</param>
        /// <param name="separator">Separador entre os elementos (não pode ser null)</param>
        /// <returns>String concatenada com os elementos da linha</returns>
        /// <exception cref="MatrixException">Lançada quando o índice é inválido ou separator é null</exception>
        public string RowToString(int index, string separator)
        {
            ValidateRowIndex(index);

            if (separator == null)
                throw new MatrixException("O separador não pode ser nulo.");

            // Cria um array temporário para a linha específica
            var rowElements = new string[Columns];

            for (int col = 0; col < Columns; col++)
            {
                rowElements[col] = _matrix[index, col];
            }

            // Concatena usando o separador especificado
            return string.Join(separator, rowElements);
        }

        // Métodos auxiliares para validação
        private void ValidateRowIndex(int row)
        {
            if (row < 0 || row >= Rows)
                throw new MatrixException($"Índice de linha {row} está fora dos limites. A matriz tem {Rows} linhas.");
        }

        private void ValidateColumnIndex(int column)
        {
            if (column < 0 || column >= Columns)
                throw new MatrixException($"Índice de coluna {column} está fora dos limites. A matriz tem {Columns} colunas.");
        }

        // Método adicional útil para debug/visualização
        public override string ToString()
        {
            var result = new System.Text.StringBuilder();

            for (int i = 0; i < Rows; i++)
            {
                result.AppendLine(RowToString(i, ", "));
            }

            return result.ToString();
        }
    }

    // Exemplo de uso
    public class Program
    {
        public static void Main()
        {
            try
            {
                // Cria uma matriz 3x4 com valor padrão "empty"
                var matrix = new MatrixString(3, 4, "empty");

                // Define alguns valores específicos
                matrix.Set(0, 0, "A1");
                matrix.Set(0, 1, "A2");
                matrix.Set(1, 2, "B3");
                matrix.Set(2, 3, "C4");

                // Testa RowToString
                Console.WriteLine("Linha 0: " + matrix.RowToString(0, " | "));
                Console.WriteLine("Linha 1: " + matrix.RowToString(1, " - "));
                Console.WriteLine("Linha 2: " + matrix.RowToString(2, ", "));

                Console.WriteLine("\nMatriz completa:");
                Console.WriteLine(matrix.ToString());

                // Testa exceções
                try
                {
                    matrix.Set(5, 0, "erro");
                }
                catch (MatrixException ex)
                {
                    Console.WriteLine($"\nExceção esperada: {ex.Message}");
                }

                try
                {
                    matrix.RowToString(1, null);
                }
                catch (MatrixException ex)
                {
                    Console.WriteLine($"Exceção esperada: {ex.Message}");
                }

                // Testa construtor com parâmetros inválidos
                try
                {
                    var invalidMatrix = new MatrixString(0, 5, "test");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Exceção esperada: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
            }
        }
    }
}