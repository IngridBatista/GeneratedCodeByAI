using System;
using System.Linq;

namespace GEMINI.STRING_ARRAY_ENCODING.SENIOR.PARTICIPANT_4
{
    public class MatrixString
    {
        // O campo 'm' é declarado como 'readonly' para garantir que a instância da matriz,
        // uma vez criada no construtor, não possa ser substituída posteriormente.
        // As dimensões da matriz são imutáveis após a instanciação do objeto.
        private readonly string[,] m;

        /// <summary>
        /// Inicializa uma nova instância da classe MatrixString.
        /// </summary>
        /// <param name="rows">O número de linhas. Deve ser um inteiro positivo (> 0).</param>
        /// <param name="columns">O número de colunas. Deve ser um inteiro positivo (> 0).</param>
        /// <param name="value">O valor a ser atribuído a cada posição da matriz.</param>
        /// <exception cref="ArgumentException">Lançada se 'rows' ou 'columns' não forem positivos.</exception>
        public MatrixString(int rows, int columns, string value)
        {
            // 1. Validação do Construtor
            // Verifica se as dimensões são números naturais positivos maiores que zero.
            // Referência: https://learn.microsoft.com/dotnet/api/system.argumentexception
            if (rows <= 0 || columns <= 0)
            {
                throw new ArgumentException();
            }

            // Inicializa a matriz com as dimensões especificadas.
            this.m = new string[rows, columns];

            // Preenche cada posição da matriz com o valor fornecido.
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    this.m[i, j] = value;
                }
            }
        }

        /// <summary>
        /// Atribui um valor a uma posição específica da matriz.
        /// </summary>
        /// <param name="row">O índice da linha (base 0).</param>
        /// <param name="column">O índice da coluna (base 0).</param>
        /// <param name="value">O valor a ser atribuído.</param>
        /// <exception cref="MatrixException">Lançada se 'row' ou 'column' estiverem fora dos limites da matriz.</exception>
        public void Set(int row, int column, string value)
        {
            // 2. Validação do Método Set
            // Array.GetLength(0) retorna o número de linhas.
            // Array.GetLength(1) retorna o número de colunas.
            // Referência: https://learn.microsoft.com/dotnet/api/system.array.getlength
            if (row < 0 || row >= m.GetLength(0) || column < 0 || column >= m.GetLength(1))
            {
                throw new MatrixException();
            }

            m[row, column] = value;
        }

        /// <summary>
        /// Retorna uma string formada pela concatenação dos elementos de uma linha.
        /// </summary>
        /// <param name="index">O índice da linha a ser convertida (base 0).</param>
        /// <param name="separator">A string a ser usada como separador entre os elementos.</param>
        /// <returns>Uma string com os elementos da linha concatenados.</returns>
        /// <exception cref="MatrixException">Lançada se 'index' for inválido ou 'separator' for nulo.</exception>
        public string RowToString(int index, string separator)
        {
            // 3. Validação do Método RowToString
            if (index < 0 || index >= m.GetLength(0))
            {
                throw new MatrixException();
            }

            if (separator == null)
            {
                throw new MatrixException();
            }

            // Utiliza LINQ para extrair os elementos da linha especificada de forma concisa.
            // Enumerable.Range gera uma sequência de índices de coluna.
            // O Select projeta cada índice de coluna para o valor correspondente na matriz.
            // string.Join concatena os elementos resultantes com o separador.
            // Referência: https://learn.microsoft.com/dotnet/api/system.string.join
            // Referência: https://learn.microsoft.com/dotnet/api/system.linq.enumerable.select
            var rowElements = Enumerable.Range(0, m.GetLength(1))
                                        .Select(col => m[index, col]);

            return string.Join(separator, rowElements);
        }
    }
}