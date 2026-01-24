using System;

namespace GEMINI.STRING_ARRAY_ENCODING.SENIOR.PARTICIPANT_1
{
    /// <summary>
    /// Representa erros que ocorrem durante operações na classe MatrixString.
    /// É uma exceção personalizada para sinalizar problemas específicos da nossa matriz,
    /// como acesso a índices inválidos.
    /// </summary>
    public class MatrixException : Exception
    {
        /// <summary>
        /// Cria uma nova instância da exceção MatrixException.
        /// Usamos o construtor padrão da classe base 'Exception'.
        /// </summary>
        public MatrixException() : base()
        {
        }

        // Nota: Em um cenário real, também é uma boa prática adicionar construtores
        // que aceitam uma mensagem de erro para facilitar a depuração, como:
        // public MatrixException(string message) : base(message) { }
    }
}