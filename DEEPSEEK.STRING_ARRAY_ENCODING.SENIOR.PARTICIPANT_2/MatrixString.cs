using System;

namespace DEEPSEEK.STRING_ARRAY_ENCODING.SENIOR.PARTICIPANT_2
{
    public class MatrixString
    {
        private string[,] m;

        // Exceção personalizada para operações da matriz
        public class MatrixException : Exception
        {
            public MatrixException(string message) : base(message) { }
        }

        // Construtor
        public MatrixString(int rows, int columns, string value)
        {
            // Validação dos parâmetros
            if (rows <= 0)
                throw new ArgumentException("O número de linhas deve ser positivo.", nameof(rows));

            if (columns <= 0)
                throw new ArgumentException("O número de colunas deve ser positivo.", nameof(columns));

            // Inicializa a matriz
            m = new string[rows, columns];

            // Preenche a matriz com o valor fornecido
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    m[i, j] = value;
                }
            }
        }

        // Método para definir um valor em uma posição específica
        public void Set(int row, int column, string value)
        {
            // Validação dos índices
            if (row < 0 || row >= m.GetLength(0))
                throw new MatrixException($"Índice de linha {row} está fora dos limites. A matriz tem {m.GetLength(0)} linhas.");

            if (column < 0 || column >= m.GetLength(1))
                throw new MatrixException($"Índice de coluna {column} está fora dos limites. A matriz tem {m.GetLength(1)} colunas.");

            // Atribui o valor
            m[row, column] = value;
        }

        // Método para converter uma linha em string com separador
        public string RowToString(int index, string separator)
        {
            // Validação do separador
            if (separator == null)
                throw new MatrixException("O separador não pode ser nulo.");

            // Validação do índice
            if (index < 0 || index >= m.GetLength(0))
                throw new MatrixException($"Índice {index} está fora dos limites. A matriz tem {m.GetLength(0)} linhas.");

            // Concatena os elementos da linha
            string[] rowElements = new string[m.GetLength(1)];
            for (int j = 0; j < m.GetLength(1); j++)
            {
                rowElements[j] = m[index, j];
            }

            // Retorna a string concatenada
            return string.Join(separator, rowElements);
        }

        // Propriedade para obter o número de linhas (opcional, útil para testes)
        public int Rows => m.GetLength(0);

        // Propriedade para obter o número de colunas (opcional, útil para testes)
        public int Columns => m.GetLength(1);

        // Método para obter um valor específico (opcional, útil para testes)
        public string Get(int row, int column)
        {
            if (row < 0 || row >= m.GetLength(0))
                throw new MatrixException($"Índice de linha {row} está fora dos limites.");

            if (column < 0 || column >= m.GetLength(1))
                throw new MatrixException($"Índice de coluna {column} está fora dos limites.");

            return m[row, column];
        }
    }
}