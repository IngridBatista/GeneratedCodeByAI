using System;

namespace CLAUDE.STRING_ARRAY_ENCODING.SENIOR.PARTICIPANT_1
{
    public class MatrixException : Exception
    {
        // Construtor padrão (sem parâmetros)
        public MatrixException() : base()
        {
            // Chama o construtor base da classe Exception
            // Isso cria uma exceção genérica sem mensagem específica
        }

        // Construtor com mensagem (opcional, mas útil para debugging)
        public MatrixException(string message) : base(message)
        {
        }

        // Construtor com mensagem e exceção interna (opcional)
        public MatrixException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}