using System;
using System.Linq;

namespace GEMINI.STRING_ARRAY_ENCODING.SENIOR.PARTICIPANT_3
{
    /// <summary>
    /// Exceção personalizada para erros relacionados a operações na MatrixString.
    /// </summary>
    public class MatrixException : Exception
    {
        public MatrixException(string message) : base(message) { }
    }

    public class MatrixString
    {
        private string[,] m;

        /// <summary>
        /// Cria uma matriz de strings com o tamanho especificado e a preenche com um valor inicial.
        /// </summary>
        /// <param name="rows">O número de linhas.</param>
        /// <param name="cols">O número de colunas.</param>
        /// <param name="val">O valor para preencher toda a matriz.</param>
        /// <exception cref="ArgumentException">Lançada se rows ou cols forem menores ou iguais a zero.</exception>
        public MatrixString(int rows, int cols, string val)
        {
            if (rows <= 0 || cols <= 0)
            {
                throw new ArgumentException("As dimensões da matriz devem ser positivas.");
            }

            this.m = new string[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    this.m[i, j] = val;
                }
            }
        }

        /// <summary>
        /// Atribui um valor a uma posição específica na matriz.
        /// </summary>
        /// <param name="r">O índice da linha (baseado em zero).</param>
        /// <param name="c">O índice da coluna (baseado em zero).</param>
        /// <param name="val">O valor a ser atribuído.</param>
        /// <exception cref="MatrixException">Lançada se os índices r ou c estiverem fora dos limites da matriz.</exception>
        public void Set(int r, int c, string val)
        {
            if (r < 0 || r >= m.GetLength(0) || c < 0 || c >= m.GetLength(1))
            {
                throw new MatrixException("Índice de linha ou coluna fora dos limites da matriz.");
            }
            this.m[r, c] = val;
        }

        /// <summary>
        /// Retorna uma string representando uma linha da matriz, com os elementos separados por um delimitador.
        /// </summary>
        /// <param name="index">O índice da linha a ser convertida.</param>
        /// <param name="separator">A string para usar como separador entre os elementos.</param>
        /// <returns>Uma string concatenada dos elementos da linha.</returns>
        /// <exception cref="MatrixException">Lançada se o índice for inválido ou se o separador for nulo.</exception>
        public string RowToString(int index, string separator)
        {
            if (index < 0 || index >= m.GetLength(0))
            {
                throw new MatrixException("Índice de linha inválido.");
            }
            if (separator == null)
            {
                throw new MatrixException("O separador não pode ser nulo.");
            }

            // Cria uma sequência de índices de coluna (de 0 até o número de colunas - 1)
            var columnIndices = Enumerable.Range(0, m.GetLength(1));

            // Para cada índice de coluna, seleciona o elemento correspondente na linha 'index'
            var rowElements = columnIndices.Select(col => m[index, col]);

            // Junta os elementos da linha em uma única string, usando o separador
            return string.Join(separator, rowElements);
        }
    }
}