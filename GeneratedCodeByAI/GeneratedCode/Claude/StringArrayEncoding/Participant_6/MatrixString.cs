namespace CLAUDE.STRING_ARRAY_ENCODING.PLENO.PARTICIPANT_6
{
    public class MatrixString
    {
        private string[,] m;

        // 1. Construtor
        public MatrixString(int rows, int columns, string value)
        {
            // Validação dos parâmetros
            if (rows <= 0 || columns <= 0)
            {
                throw new ArgumentException();
            }

            // Inicialização da matriz
            m = new string[rows, columns];

            // Preenchimento da matriz com o valor fornecido
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
            if (row < 0 || row >= m.GetLength(0) ||
                column < 0 || column >= m.GetLength(1))
            {
                throw new MatrixException();
            }

            // Atribuição do valor
            m[row, column] = value;
        }

        // 3. Método RowToString
        public string RowToString(int index, string separator)
        {
            // Validação do índice da linha
            if (index < 0 || index >= m.GetLength(0))
            {
                throw new MatrixException();
            }

            // Validação do separator
            if (separator == null)
            {
                throw new MatrixException();
            }

            // Construção da string com os elementos da linha
            string result = "";
            int columns = m.GetLength(1);

            for (int j = 0; j < columns; j++)
            {
                result += m[index, j];

                // Adiciona o separador apenas se não for o último elemento
                if (j < columns - 1)
                {
                    result += separator;
                }
            }

            return result;
        }
    }

    // Classe de exceção personalizada
    public class MatrixException : Exception
    {
        public MatrixException() : base()
        {
        }
    }
}