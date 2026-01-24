using System;
using System.Linq;
using System.Text;

namespace GEMINI.STRING_ARRAY_ENCODING.JUNIOR.PARTICIPANT_7
{
    /// <summary>
    /// Representa uma matriz bidimensional de strings com operações básicas.
    /// </summary>
    public class MatrixString
    {
        // O campo privado que armazena a matriz.
        // O uso de 'readonly' garante que a referência à matriz não possa ser alterada após a construção,
        // promovendo a imutabilidade da estrutura da matriz (mas não de seus valores internos).
        private readonly string[,] m;

        /// <summary>
        /// Inicializa uma nova instância da classe MatrixString, preenchendo-a com um valor padrão.
        /// </summary>
        /// <param name="rows">O número de linhas da matriz. Deve ser um valor maior que zero.</param>
        /// <param name="columns">O número de colunas da matriz. Deve ser um valor maior que zero.</param>
        /// <param name="value">O valor a ser atribuído a cada posição da matriz.</param>
        /// <exception cref="ArgumentException">Lançada se 'rows' ou 'columns' for menor ou igual a zero.</exception>
        public MatrixString(int rows, int columns, string value)
        {
            // Princípio "Fail-Fast": Validar os parâmetros de entrada antes de qualquer outra operação.
            if (rows <= 0 || columns <= 0)
            {
                // Lança a exceção padrão para argumentos inválidos, conforme solicitado.
                throw new ArgumentException("As dimensões da matriz devem ser números naturais positivos maiores que zero.");
            }

            // Inicializa a matriz com as dimensões especificadas.
            m = new string[rows, columns];

            // Preenche cada célula da matriz com o valor fornecido.
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    m[i, j] = value;
                }
            }
        }

        /// <summary>
        /// Atribui um novo valor a uma posição específica da matriz.
        /// </summary>
        /// <param name="row">O índice da linha (baseado em zero).</param>
        /// <param name="column">O índice da coluna (baseado em zero).</param>
        /// <param name="value">O novo valor a ser atribuído.</param>
        /// <exception cref="MatrixException">Lançada se 'row' ou 'column' estiverem fora dos limites da matriz.</exception>
        public void Set(int row, int column, string value)
        {
            // Validação dos limites da matriz.
            // m.GetLength(0) retorna o número de linhas.
            // m.GetLength(1) retorna o número de colunas.
            if (row < 0 || row >= m.GetLength(0) || column < 0 || column >= m.GetLength(1))
            {
                // Lança a exceção personalizada, conforme solicitado.
                throw new MatrixException();
            }

            m[row, column] = value;
        }

        /// <summary>
        /// Retorna uma string formada pela concatenação dos elementos de uma linha específica,
        /// separados por um delimitador.
        /// </summary>
        /// <param name="index">O índice da linha a ser convertida para string.</param>
        /// <param name="separator">A string a ser usada como separador entre os elementos.</param>
        /// <returns>Uma string representando a linha da matriz.</returns>
        /// <exception cref="MatrixException">Lançada se 'index' for um índice de linha inválido ou se 'separator' for nulo.</exception>
        public string RowToString(int index, string separator)
        {
            // Validação do índice da linha e do separador.
            if (index < 0 || index >= m.GetLength(0))
            {
                throw new MatrixException();
            }

            if (separator == null)
            {
                throw new MatrixException();
            }

            // Uma abordagem moderna e limpa usando LINQ para extrair a linha
            // e string.Join para concatenar os elementos.
            // Enumerable.Range gera uma sequência de índices de coluna (0, 1, 2, ...).
            // .Select projeta cada índice de coluna para o valor correspondente na matriz.
            var rowElements = Enumerable.Range(0, m.GetLength(1))
                                        .Select(col => m[index, col]);

            return string.Join(separator, rowElements);
        }
    }
}