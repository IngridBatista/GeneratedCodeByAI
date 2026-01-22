using System;
using System.Text;

namespace MatrixLibrary
{
    /// <summary>
    /// Exceção personalizada para operações inválidas na matriz
    /// </summary>
    public class MatrixException : Exception
    {
        public MatrixException() : base() { }

        public MatrixException(string message) : base(message) { }

        public MatrixException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    /// <summary>
    /// Classe que representa uma matriz de strings
    /// </summary>
    public class MatrixString
    {
        private string[,] m;

        /// <summary>
        /// Construtor que inicializa a matriz com dimensões e valor especificados
        /// </summary>
        /// <param name="rows">Número de linhas</param>
        /// <param name="columns">Número de colunas</param>
        /// <param name="value">Valor inicial para preencher a matriz</param>
        /// <exception cref="ArgumentException">Lançada quando rows ou columns não são positivos</exception>
        public MatrixString(int rows, int columns, string value)
        {
            if (rows <= 0)
            {
                throw new ArgumentException("O número de linhas deve ser positivo.", nameof(rows));
            }

            if (columns <= 0)
            {
                throw new ArgumentException("O número de colunas deve ser positivo.", nameof(columns));
            }

            m = new string[rows, columns];

            // Preenche a matriz com o valor especificado
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    m[i, j] = value;
                }
            }
        }

        /// <summary>
        /// Define o valor de uma célula específica da matriz
        /// </summary>
        /// <param name="row">Índice da linha</param>
        /// <param name="column">Índice da coluna</param>
        /// <param name="value">Valor a ser atribuído</param>
        /// <exception cref="MatrixException">Lançada quando os índices estão fora dos limites</exception>
        public void Set(int row, int column, string value)
        {
            if (row < 0 || row >= m.GetLength(0))
            {
                throw new MatrixException($"Índice de linha {row} está fora dos limites. Deve estar entre 0 e {m.GetLength(0) - 1}.");
            }

            if (column < 0 || column >= m.GetLength(1))
            {
                throw new MatrixException($"Índice de coluna {column} está fora dos limites. Deve estar entre 0 e {m.GetLength(1) - 1}.");
            }

            m[row, column] = value;
        }

        /// <summary>
        /// Retorna uma linha da matriz como string concatenada
        /// </summary>
        /// <param name="index">Índice da linha</param>
        /// <param name="separator">Separador entre os elementos</param>
        /// <returns>String com os elementos da linha separados pelo separador</returns>
        /// <exception cref="MatrixException">Lançada quando o índice é inválido ou separator é null</exception>
        public string RowToString(int index, string separator)
        {
            if (separator == null)
            {
                throw new MatrixException("O separador não pode ser null.");
            }

            if (index < 0 || index >= m.GetLength(0))
            {
                throw new MatrixException($"Índice de linha {index} está fora dos limites. Deve estar entre 0 e {m.GetLength(0) - 1}.");
            }

            StringBuilder result = new StringBuilder();
            int columns = m.GetLength(1);

            for (int j = 0; j < columns; j++)
            {
                result.Append(m[index, j]);

                // Adiciona o separador apenas se não for o último elemento
                if (j < columns - 1)
                {
                    result.Append(separator);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Propriedade para obter o número de linhas
        /// </summary>
        public int Rows => m.GetLength(0);

        /// <summary>
        /// Propriedade para obter o número de colunas
        /// </summary>
        public int Columns => m.GetLength(1);
    }
}