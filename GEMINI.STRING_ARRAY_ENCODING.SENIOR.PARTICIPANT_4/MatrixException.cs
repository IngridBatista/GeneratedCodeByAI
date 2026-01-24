using System;
using System.Runtime.Serialization;

namespace GEMINI.STRING_ARRAY_ENCODING.SENIOR.PARTICIPANT_4
{
    // A classe deve herdar de System.Exception para ser tratada como uma exceção padrão do .NET.
    // A implementação dos construtores padrão é uma prática recomendada.
    // Referência: https://learn.microsoft.com/dotnet/standard/exceptions/how-to-create-user-defined-exceptions
    [Serializable]
    public class MatrixException : Exception
    {
        // Construtor padrão, solicitado no problema.
        public MatrixException() { }

        // Construtor que aceita uma mensagem de erro.
        public MatrixException(string message) : base(message) { }

        // Construtor para exceções aninhadas (inner exceptions).
        public MatrixException(string message, Exception inner) : base(message, inner) { }

        // Construtor necessário para serialização da exceção.
        protected MatrixException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}