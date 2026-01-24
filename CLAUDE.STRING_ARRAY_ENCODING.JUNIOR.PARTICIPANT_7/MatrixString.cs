namespace CLAUDE.STRING_ARRAY_ENCODING.JUNIOR.PARTICIPANT_7
{
    /// <summary>
    /// Representa uma matriz bidimensional de strings com operações básicas
    /// </summary>
    public class MatrixString
    {
        private string[,] m;

        /// <summary>
        /// Inicializa uma nova instância da matriz com dimensões e valor padrão
        /// </summary>
        /// <param name="rows">Número de linhas (deve ser maior que zero)</param>
        /// <param name="columns">Número de colunas (deve ser maior que zero)</param>
        /// <param name="value">Valor inicial para todas as posições</param>
        /// <exception cref="ArgumentException">
        /// Lançada quando rows ou columns não são valores positivos maiores que zero
        /// </exception>
        public MatrixString(int rows, int columns, string value)
        {
            if (rows <= 0)
            {
                throw new ArgumentException();
            }

            if (columns <= 0)
            {
                throw new ArgumentException();
            }

            m = new string[rows, columns];
            InitializeMatrix(value);
        }

        /// <summary>
        /// Inicializa todas as posições da matriz com o valor especificado
        /// </summary>
        private void InitializeMatrix(string value)
        {
            for (int i = 0; i < m.GetLength(0); i++)
            {
                for (int j = 0; j < m.GetLength(1); j++)
                {
                    m[i, j] = value;
                }
            }
        }

        /// <summary>
        /// Define o valor de uma posição específica da matriz
        /// </summary>
        /// <param name="row">Índice da linha</param>
        /// <param name="column">Índice da coluna</param>
        /// <param name="value">Valor a ser atribuído</param>
        /// <exception cref="MatrixException">
        /// Lançada quando os índices estão fora dos limites da matriz
        /// </exception>
        public void Set(int row, int column, string value)
        {
            if (!IsValidRow(row))
            {
                throw new MatrixException();
            }

            if (!IsValidColumn(column))
            {
                throw new MatrixException();
            }

            m[row, column] = value;
        }

        /// <summary>
        /// Retorna uma string com os elementos de uma linha concatenados
        /// </summary>
        /// <param name="index">Índice da linha</param>
        /// <param name="separator">Separador entre os elementos</param>
        /// <returns>String concatenada com os elementos da linha</returns>
        /// <exception cref="MatrixException">
        /// Lançada quando o índice é inválido ou separator é null
        /// </exception>
        public string RowToString(int index, string separator)
        {
            if (!IsValidRow(index))
            {
                throw new MatrixException();
            }

            if (separator == null)
            {
                throw new MatrixException();
            }

            return BuildRowString(index, separator);
        }

        /// <summary>
        /// Constrói a string concatenada de uma linha
        /// </summary>
        private string BuildRowString(int rowIndex, string separator)
        {
            var elements = new List<string>();
            int columnCount = m.GetLength(1);

            for (int j = 0; j < columnCount; j++)
            {
                elements.Add(m[rowIndex, j]);
            }

            return string.Join(separator, elements);
        }

        /// <summary>
        /// Verifica se o índice da linha é válido
        /// </summary>
        private bool IsValidRow(int row)
        {
            return row >= 0 && row < m.GetLength(0);
        }

        /// <summary>
        /// Verifica se o índice da coluna é válido
        /// </summary>
        private bool IsValidColumn(int column)
        {
            return column >= 0 && column < m.GetLength(1);
        }
    }
}