using System;
using System.Linq; // Necessário para o método RowToString (opção mais elegante)
using System.Text;  // Alternativa para RowToString

namespace GEMINI.STRING_ARRAY_ENCODING.PLENO.PARTICIPANT_6
{
    public class MatrixString
    {
        private string[,] m;

        /// <summary>
        /// 1. Construtor
        /// Inicializa a matriz com um tamanho e valor padrão.
        /// </summary>
        /// <param name="rows">Número de linhas (deve ser > 0).</param>
        /// <param name="columns">Número de colunas (deve ser > 0).</param>
        /// <param name="value">Valor a ser atribuído a cada posição.</param>
        /// <exception cref="ArgumentException">Lançada se rows ou columns for menor ou igual a zero.</exception>
        public MatrixString(int rows, int columns, string value)
        {
            // Validação das dimensões da matriz
            if (rows <= 0 || columns <= 0)
            {
                throw new ArgumentException();
            }

            // Inicialização do array bidimensional
            this.m = new string[rows, columns];

            // Preenchimento da matriz com o valor inicial
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    m[i, j] = value;
                }
            }
        }

        /// <summary>
        /// 2. Método Set
        /// Atribui um valor a uma posição específica da matriz.
        /// </summary>
        /// <param name="row">Índice da linha.</param>
        /// <param name="column">Índice da coluna.</param>
        /// <param name="value">Valor a ser atribuído.</param>
        /// <exception cref="MatrixException">Lançada se os índices estiverem fora dos limites da matriz.</exception>
        public void Set(int row, int column, string value)
        {
            // Validação dos índices
            // m.GetLength(0) retorna o número de linhas
            // m.GetLength(1) retorna o número de colunas
            if (row < 0 || row >= m.GetLength(0) || column < 0 || column >= m.GetLength(1))
            {
                throw new MatrixException();
            }

            // Atribuição do valor
            m[row, column] = value;
        }

        /// <summary>
        /// 3. Método RowToString
        /// Concatena todos os elementos de uma linha em uma única string, usando um separador.
        /// </summary>
        /// <param name="index">O índice da linha a ser convertida.</param>
        /// <param name="separator">A string para separar os elementos.</param>
        /// <returns>Uma string com os elementos da linha concatenados.</returns>
        /// <exception cref="MatrixException">Lançada se o índice da linha for inválido ou o separador for nulo.</exception>
        public string RowToString(int index, string separator)
        {
            // Validação do índice e do separador
            if (index < 0 || index >= m.GetLength(0) || separator == null)
            {
                throw new MatrixException();
            }

            // Opção 1: Usando LINQ e string.Join (mais moderno e conciso)
            // Enumerable.Range cria uma sequência de números (0, 1, 2, ...) para as colunas.
            // O .Select extrai o valor de cada coluna na linha 'index'.
            // string.Join concatena todos os valores usando o separador.
            var rowElements = Enumerable.Range(0, m.GetLength(1))
                                        .Select(col => m[index, col]);
            return string.Join(separator, rowElements);

            /*
            // Opção 2: Usando um loop e StringBuilder (mais tradicional e eficiente para versões antigas do .NET)
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < m.GetLength(1); j++)
            {
                sb.Append(m[index, j]);
                if (j < m.GetLength(1) - 1) // Adiciona o separador, exceto para o último elemento
                {
                    sb.Append(separator);
                }
            }
            return sb.ToString();
            */
        }
    }
}