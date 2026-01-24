using System;

namespace GEMINI.STRING_ARRAY_ENCODING.SENIOR.PARTICIPANT_5
{
    /// <summary>
    /// Representa erros que ocorrem durante operações em uma MatrixString, 
    /// como acesso a índices inválidos.
    /// </summary>
    public class MatrixException : Exception
    {
        /// <summary>
        /// Inicializa uma nova instância da classe MatrixException.
        /// </summary>
        public MatrixException()
            : base("Ocorreu um erro na operação da matriz. Verifique os índices fornecidos.")
        {
        }

        /// <summary>
        /// Inicializa uma nova instância da classe MatrixException com uma mensagem de erro específica.
        /// </summary>
        /// <param name="message">A mensagem que descreve o erro.</param>
        public MatrixException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Inicializa uma nova instância da classe MatrixException com uma mensagem de erro específica 
        /// e uma referência à exceção interna que é a causa desta exceção.
        /// </summary>
        /// <param name="message">A mensagem de erro que explica o motivo da exceção.</param>
        /// <param name="innerException">A exceção que é a causa da exceção atual.</param>
        public MatrixException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}