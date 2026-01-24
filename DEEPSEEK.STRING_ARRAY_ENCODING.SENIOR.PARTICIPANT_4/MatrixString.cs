using System;

namespace DEEPSEEK.STRING_ARRAY_ENCODING.SENIOR.PARTICIPANT_4
{
    public class MatrixString
    {
        private string[,] m;

        // 1. Construtor
        public MatrixString(int rows, int columns, string value)
        {
            // Validação dos parâmetros conforme especificado
            if (rows <= 0)
                throw new ArgumentException("rows must be a positive natural number greater than zero");

            if (columns <= 0)
                throw new ArgumentException("columns must be a positive natural number greater than zero");

            // Inicializa a matriz com as dimensões especificadas
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

        // 2. Método Set
        public void Set(int row, int column, string value)
        {
            // Validação dos índices
            if (row < 0 || row >= m.GetLength(0) || column < 0 || column >= m.GetLength(1))
                throw new MatrixException("Row or column index is out of bounds");

            m[row, column] = value;
        }

        // 3. Método RowToString
        public string RowToString(int index, string separator)
        {
            // Validação do separador
            if (separator == null)
                throw new MatrixException("Separator cannot be null");

            // Validação do índice da linha
            if (index < 0 || index >= m.GetLength(0))
                throw new MatrixException("Row index is out of bounds");

            // Concatena os elementos da linha
            string result = "";
            int columns = m.GetLength(1);

            for (int j = 0; j < columns; j++)
            {
                result += m[index, j];

                // Adiciona separador apenas entre elementos (não após o último)
                if (j < columns - 1)
                    result += separator;
            }

            return result;
        }

        // Método auxiliar para visualização (não solicitado, mas útil para testes)
        public int Rows => m.GetLength(0);
        public int Columns => m.GetLength(1);
    }

    // Exceção personalizada conforme especificado
    public class MatrixException : Exception
    {
        public MatrixException() : base() { }
        public MatrixException(string message) : base(message) { }
        public MatrixException(string message, Exception innerException) : base(message, innerException) { }
    }
}