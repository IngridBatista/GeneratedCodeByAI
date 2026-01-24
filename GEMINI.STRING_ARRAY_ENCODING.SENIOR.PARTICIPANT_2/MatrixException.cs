using System;

namespace GEMINI.STRING_ARRAY_ENCODING.SENIOR.PARTICIPANT_2
{
    /// <summary>
    /// Representa erros que ocorrem durante operações na classe MatrixString.
    /// </summary>
    public class MatrixException : Exception
    {
        public MatrixException() { }

        public MatrixException(string message) : base(message) { }

        public MatrixException(string message, Exception inner) : base(message, inner) { }
    }
}