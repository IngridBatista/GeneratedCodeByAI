using System;
using System.Collections.Generic;
using System.Linq;

namespace GEMINI.STRING_ARRAY_ENCODING.SENIOR.PARTICIPANT_5
{
    /// <summary>
    /// Representa uma matriz bidimensional de strings.
    /// </summary>
    public class MatrixString
    {
        // O campo é 'readonly' porque a instância da matriz em si não será trocada
        // após a inicialização no construtor. Apenas seu conteúdo será alterado.
        // O prefixo '_' é uma convenção comum para campos privados.
        private readonly string[,] _matrix;

        /// <summary>
        /// Obtém o número de linhas na matriz.
        /// </summary>
        public int Rows => _matrix.GetLength(0);

        /// <summary>
        /// Obtém o número de colunas na matriz.
        /// </summary>
        public int Columns => _matrix.GetLength(1);

        /// <summary>
        /// Inicializa uma nova instância da classe MatrixString, preenchendo todas as posições com um valor padrão.
        /// </summary>
        /// <param name="rows">O número de linhas da matriz. Deve ser um valor positivo.</param>
        /// <param name="columns">O número de colunas da matriz. Deve ser um valor positivo.</param>
        /// <param name="value">O valor a ser atribuído a cada elemento da matriz.</param>
        /// <exception cref="ArgumentException">Lançada se 'rows' ou 'columns' não forem maiores que zero.</exception>
        public MatrixString(int rows, int columns, string value)
        {
            if (rows <= 0 || columns <= 0)
            {
                // Lançando a exceção com uma mensagem clara, o que é mais útil do que o construtor padrão.
                throw new ArgumentException("As dimensões da matriz (linhas e colunas) devem ser maiores que zero.");
            }

            _matrix = new string[rows, columns];

            // A responsabilidade de preencher a matriz foi delegada a um método privado.
            FillMatrix(value);
        }

        /// <summary>
        /// Atribui um valor a uma posição específica na matriz.
        /// </summary>
        /// <param name="row">O índice da linha (baseado em zero).</param>
        /// <param name="column">O índice da coluna (baseado em zero).</param>
        /// <param name="value">O valor a ser atribuído.</param>
        /// <exception cref="MatrixException">Lançada se 'row' ou 'column' estiverem fora dos limites da matriz.</exception>
        public void Set(int row, int column, string value)
        {
            ValidateIndices(row, column);
            _matrix[row, column] = value;
        }

        /// <summary>
        /// Retorna uma string formada pela concatenação dos elementos de uma linha específica,
        /// separados por um delimitador.
        /// </summary>
        /// <param name="rowIndex">O índice da linha a ser convertida para string.</param>
        /// <param name="separator">A string para usar como separador entre os elementos.</param>
        /// <returns>Uma string concatenada dos elementos da linha.</returns>
        /// <exception cref="MatrixException">Lançada se 'rowIndex' for inválido ou se 'separator' for nulo.</exception>
        public string RowToString(int rowIndex, string separator)
        {
            if (separator is null)
            {
                throw new MatrixException("O separador não pode ser nulo.");
            }

            ValidateRowIndex(rowIndex);

            // Delega a obtenção dos elementos da linha para um método auxiliar,
            // mantendo este método focado apenas na formatação.
            IEnumerable<string> rowElements = GetRowElements(rowIndex);

            return string.Join(separator, rowElements);
        }

        // ========================================================================
        // MÉTODOS PRIVADOS (AUXILIARES)
        // ========================================================================

        /// <summary>
        /// Preenche toda a matriz com um valor especificado.
        /// </summary>
        private void FillMatrix(string value)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    _matrix[i, j] = value;
                }
            }
        }

        /// <summary>
        /// Valida se os índices de linha e coluna estão dentro dos limites da matriz.
        /// </summary>
        private void ValidateIndices(int row, int column)
        {
            ValidateRowIndex(row);
            ValidateColumnIndex(column);
        }

        /// <summary>
        /// Valida se o índice da linha está dentro dos limites da matriz.
        /// </summary>
        private void ValidateRowIndex(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= Rows)
            {
                throw new MatrixException($"O índice da linha ({rowIndex}) está fora dos limites válidos [0-{Rows - 1}].");
            }
        }

        /// <summary>
        /// Valida se o índice da coluna está dentro dos limites da matriz.
        /// </summary>
        private void ValidateColumnIndex(int columnIndex)
        {
            if (columnIndex < 0 || columnIndex >= Columns)
            {
                throw new MatrixException($"O índice da coluna ({columnIndex}) está fora dos limites válidos [0-{Columns - 1}].");
            }
        }

        /// <summary>
        /// Retorna um enumerador para os elementos de uma linha específica.
        /// O uso de 'yield return' é eficiente, pois evita a criação de uma coleção intermediária em memória.
        /// </summary>
        private IEnumerable<string> GetRowElements(int rowIndex)
        {
            for (int j = 0; j < Columns; j++)
            {
                yield return _matrix[rowIndex, j];
            }
        }
    }
}