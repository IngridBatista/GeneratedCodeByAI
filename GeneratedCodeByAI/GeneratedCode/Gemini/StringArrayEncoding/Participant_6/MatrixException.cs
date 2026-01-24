using System;

namespace GEMINI.STRING_ARRAY_ENCODING.PLENO.PARTICIPANT_6
{
    // Exceção personalizada para erros específicos da nossa matriz.
    public class MatrixException : Exception
    {
        // Construtor padrão solicitado no exercício.
        public MatrixException() : base() { }

        // Construtores adicionais (boas práticas)
        public MatrixException(string message) : base(message) { }
        public MatrixException(string message, Exception inner) : base(message, inner) { }
    }
}