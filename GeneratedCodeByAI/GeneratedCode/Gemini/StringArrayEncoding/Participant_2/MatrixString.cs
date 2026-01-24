using System;
using System.Text; // Usado para uma implementação alternativa de RowToString

namespace GEMINI.STRING_ARRAY_ENCODING.SENIOR.PARTICIPANT_2
{

    /// <summary>
    /// Representa uma matriz bidimensional de strings.
    /// </summary>
    public class MatrixString
    {
        /// <summary>
        /// A matriz interna que armazena os dados.
        /// É privada para encapsular o estado da classe.
        /// </summary>
        private readonly string[,] m;

        /// <summary>
        /// Construtor que inicializa a matriz com um tamanho e valor padrão.
        /// </summary>
        /// <param name="rows">O número de linhas da matriz.</param>
        /// <param name="columns">O número de colunas da matriz.</param>
        /// <param name="value">O valor inicial para preencher todas as células.</param>
        /// <exception cref="ArgumentException">Lançada se 'rows' ou 'columns' não forem positivos.</exception>
        public MatrixString(int rows, int columns, string value)
        {
            // Validação dos parâmetros do construtor
            if (rows <= 0)
            {
                throw new ArgumentException("O número de linhas deve ser um valor positivo.", nameof(rows));
            }
            if (columns <= 0)
            {
                throw new ArgumentException("O número de colunas deve ser um valor positivo.", nameof(columns));
            }

            // Inicialização e preenchimento da matriz
            m = new string[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    m[i, j] = value;
                }
            }
        }

        /// <summary>
        /// Atribui um valor a uma célula específica da matriz.
        /// </summary>
        /// <param name="row">O índice da linha (base 0).</param>
        /// <param name="column">O índice da coluna (base 0).</param>
        /// <param name="value">O valor a ser atribuído.</param>
        /// <exception cref="MatrixException">Lançada se os índices de linha ou coluna estiverem fora dos limites.</exception>
        public void Set(int row, int column, string value)
        {
            int numRows = m.GetLength(0);
            int numCols = m.GetLength(1);

            // Validação dos índices
            if (row < 0 || row >= numRows || column < 0 || column >= numCols)
            {
                throw new MatrixException($"Índices ({row}, {column}) fora dos limites da matriz ({numRows}x{numCols}).");
            }

            m[row, column] = value;
        }

        /// <summary>
        /// Concatena todos os elementos de uma linha em uma única string, usando um separador.
        /// </summary>
        /// <param name="index">O índice da linha a ser convertida para string.</param>
        /// <param name="separator">A string para usar como separador entre os elementos.</param>
        /// <returns>Uma string representando a linha concatenada.</returns>
        /// <exception cref="MatrixException">Lançada se o índice da linha for inválido ou se o separador for nulo.</exception>
        public string RowToString(int index, string separator)
        {
            int numRows = m.GetLength(0);
            int numCols = m.GetLength(1);

            // Validação do índice da linha
            if (index < 0 || index >= numRows)
            {
                throw new MatrixException($"Índice de linha {index} é inválido. Deve estar entre 0 e {numRows - 1}.");
            }

            // Validação do separador
            if (separator == null)
            {
                throw new MatrixException("O separador não pode ser nulo.");
            }

            // Extrai os elementos da linha para um array temporário
            string[] rowElements = new string[numCols];
            for (int j = 0; j < numCols; j++)
            {
                rowElements[j] = m[index, j];
            }

            // Usa string.Join para uma concatenação limpa e eficiente
            return string.Join(separator, rowElements);
        }
    }
}