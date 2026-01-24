using System;

namespace GPT.STRING_ARRAY_ENCODING.PLENO.PARTICIPANT_6
{
    public class MatrixException : Exception
    {
        public MatrixException() : base() { }
    }

    public class MatrixString
    {
        private string[,] m;

        // 1. Construtor
        public MatrixString(int rows, int columns, string value)
        {
            if (rows <= 0 || columns <= 0)
            {
                throw new ArgumentException();
            }

            m = new string[rows, columns];

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
            if (row < 0 || row >= m.GetLength(0) ||
                column < 0 || column >= m.GetLength(1))
            {
                throw new MatrixException();
            }

            m[row, column] = value;
        }

        // 3. Método RowToString
        public string RowToString(int index, string separator)
        {
            if (separator == null)
            {
                throw new MatrixException();
            }

            if (index < 0 || index >= m.GetLength(0))
            {
                throw new MatrixException();
            }

            int columns = m.GetLength(1);
            if (columns == 0)
            {
                return string.Empty;
            }

            // Monta a string da linha
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int j = 0; j < columns; j++)
            {
                if (j > 0)
                {
                    sb.Append(separator);
                }
                sb.Append(m[index, j]);
            }

            return sb.ToString();
        }
    }
}