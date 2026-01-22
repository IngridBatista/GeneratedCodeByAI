using System;

namespace CLAUDE.STRING_ARRAY_ENCODING.SENIOR.PARTICIPANT_1
{
    public class MatrixString
    {
        // Matriz privada para armazenar as strings
        private string[,] m;

        /// <summary>
        /// Construtor que cria uma matriz rows × columns preenchida com value
        /// </summary>
        public MatrixString(int rows, int columns, string value)
        {
            // Validação: rows e columns devem ser maiores que zero
            if (rows <= 0 || columns <= 0)
            {
                throw new ArgumentException();
            }

            // Cria a matriz com as dimensões especificadas
            m = new string[rows, columns];

            // Preenche todas as posições com o valor fornecido
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
        public void Set(int row, int column, string value)
        {
            // Verifica se os índices estão dentro dos limites da matriz
            // GetLength(0) retorna o número de linhas
            // GetLength(1) retorna o número de colunas
            if (row < 0 || row >= m.GetLength(0) ||
                column < 0 || column >= m.GetLength(1))
            {
                throw new MatrixException();
            }

            // Atribui o valor à posição especificada
            m[row, column] = value;
        }

        /// <summary>
        /// Retorna uma string com todos os elementos de uma linha, separados por separator
        /// </summary>
        public string RowToString(int index, string separator)
        {
            // Validação: index deve estar dentro dos limites
            if (index < 0 || index >= m.GetLength(0))
            {
                throw new MatrixException();
            }

            // Validação: separator não pode ser null
            if (separator == null)
            {
                throw new MatrixException();
            }

            // Constrói a string com os elementos da linha
            string result = "";
            int columns = m.GetLength(1);

            for (int j = 0; j < columns; j++)
            {
                result += m[index, j];

                // Adiciona o separador entre os elementos (mas não após o último)
                if (j < columns - 1)
                {
                    result += separator;
                }
            }

            return result;
        }
    }
}