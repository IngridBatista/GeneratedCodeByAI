using System;

namespace GPT.STRING_ARRAY_ENCODING.SENIOR.PARTICIPANT_4
{
    public class MatrixException : Exception
    {
        public MatrixException() : base()
        {
        }

        public MatrixException(string message) : base(message)
        {
        }

        public MatrixException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class MatrixString
    {
        private string[,] m;

        // 1. Construtor
        public MatrixString(int rows, int columns, string value)
        {
            // Validação de parâmetros: rows e columns devem ser naturais positivos (> 0)
            if (rows <= 0 || columns <= 0)
            {
                // Conforme solicitado, usar o construtor padrão de ArgumentException
                throw new ArgumentException();
            }

            m = new string[rows, columns];

            // Preenche toda a matriz com o valor informado
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
            // Verifica se os índices estão dentro dos limites da matriz
            // Documentação: GetLength(dimension) retorna o tamanho em cada dimensão
            // https://learn.microsoft.com/dotnet/api/system.array.getlength
            if (row < 0 || row >= m.GetLength(0) ||
                column < 0 || column >= m.GetLength(1))
            {
                // Conforme solicitado, usar o construtor padrão de MatrixException
                throw new MatrixException();
            }

            m[row, column] = value;
        }

        // 3. Método RowToString
        public string RowToString(int index, string separator)
        {
            // Verifica se separator é null
            if (separator == null)
            {
                throw new MatrixException();
            }

            // Verifica se o índice de linha é válido
            if (index < 0 || index >= m.GetLength(0))
            {
                throw new MatrixException();
            }

            int columns = m.GetLength(1);
            string[] values = new string[columns];

            // Copia os valores da linha para um array unidimensional
            for (int j = 0; j < columns; j++)
            {
                values[j] = m[index, j];
            }

            // Usa String.Join para concatenar com o separador
            // https://learn.microsoft.com/dotnet/api/system.string.join
            return string.Join(separator, values);
        }
    }
}