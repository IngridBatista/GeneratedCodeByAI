using System;

namespace DEEPSEEK.STRING_ARRAY_ENCODING.PLENO.PARTICIPANT_6
{
    // Exceção personalizada para a matriz
    public class MatrixException : Exception
    {
        public MatrixException() : base() { }
        public MatrixException(string message) : base(message) { }
        public MatrixException(string message, Exception inner) : base(message, inner) { }
    }

    public class MatrixString
    {
        private string[,] m;

        // 1. Construtor
        public MatrixString(int rows, int columns, string value)
        {
            // Validação dos parâmetros
            if (rows <= 0 || columns <= 0)
            {
                throw new ArgumentException("Rows and columns must be positive integers greater than zero.");
            }

            // Inicializa a matriz com o valor especificado
            m = new string[rows, columns];

            // Preenche toda a matriz com o valor fornecido
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
            {
                throw new MatrixException("Row or column index is out of bounds.");
            }

            m[row, column] = value;
        }

        // 3. Método RowToString
        public string RowToString(int index, string separator)
        {
            // Validação do separador
            if (separator == null)
            {
                throw new MatrixException("Separator cannot be null.");
            }

            // Validação do índice
            if (index < 0 || index >= m.GetLength(0))
            {
                throw new MatrixException("Row index is out of bounds.");
            }

            // Concatena os elementos da linha
            string result = "";
            int columns = m.GetLength(1);

            for (int j = 0; j < columns; j++)
            {
                result += m[index, j];

                // Adiciona separador apenas entre elementos (não no final)
                if (j < columns - 1)
                {
                    result += separator;
                }
            }

            return result;
        }

        // Método auxiliar para visualização (opcional)
        public void PrintMatrix()
        {
            int rows = m.GetLength(0);
            int columns = m.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Console.Write(m[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}