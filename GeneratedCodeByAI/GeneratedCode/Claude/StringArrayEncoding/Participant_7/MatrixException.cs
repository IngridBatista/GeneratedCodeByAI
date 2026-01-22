namespace CLAUDE.STRING_ARRAY_ENCODING.JUNIOR.PARTICIPANT_7
{
    /// <summary>
    /// Exceção personalizada para operações inválidas na matriz
    /// </summary>
    public class MatrixException : Exception
    {
        public MatrixException() : base()
        {
        }

        public MatrixException(string message) : base(message)
        {
        }

        public MatrixException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}