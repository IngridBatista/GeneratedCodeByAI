using System;
using System.Text; // Precisamos disso para o StringBuilder

namespace GEMINI.STRING_ARRAY_ENCODING.SENIOR.PARTICIPANT_1
{

    /// <summary>
    /// Representa e manipula uma matriz (array 2D) de strings de forma segura,
    /// com validação de entradas e tratamento de erros.
    /// </summary>
    public class MatrixString
    {
        // O 'coração' da nossa classe: um array 2D privado.
        // 'private' significa que só o código DENTRO desta classe pode acessá-lo diretamente.
        // Isso garante que ninguém de fora possa bagunçar nossa matriz.
        private readonly string[,] m;

        // Propriedades públicas para obter as dimensões. O 'private set'
        // permite que apenas a própria classe defina esses valores (no construtor).
        public int Rows { get; private set; }
        public int Columns { get; private set; }

        /// <summary>
        /// Construtor: Cria e inicializa a matriz com um valor padrão.
        /// </summary>
        /// <param name="rows">O número de linhas. Deve ser maior que zero.</param>
        /// <param name="columns">O número de colunas. Deve ser maior que zero.</param>
        /// <param name="value">O valor inicial para todas as posições da matriz.</param>
        public MatrixString(int rows, int columns, string value)
        {
            // 1. Validação (Regra de Segurança):
            // Antes de fazer qualquer coisa, verificamos se as dimensões são válidas.
            // Uma matriz não pode ter 0 ou um número negativo de linhas/colunas.
            if (rows <= 0 || columns <= 0)
            {
                // Lançamos ArgumentException, que é a exceção padrão do .NET para
                // indicar que um argumento passado para um método é inválido.
                throw new ArgumentException("As dimensões da matriz devem ser positivas.");
            }

            // 2. Inicialização:
            // Se a validação passou, guardamos as dimensões e criamos o array.
            this.Rows = rows;
            this.Columns = columns;
            this.m = new string[rows, columns];

            // 3. Preenchimento:
            // Agora, usamos dois laços 'for' aninhados para percorrer cada
            // posição (célula) da matriz e preenchê-la com o valor inicial.
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    m[i, j] = value;
                }
            }
        }

        /// <summary>
        /// Define um valor em uma posição específica da matriz.
        /// </summary>
        /// <param name="row">O índice da linha (base 0).</param>
        /// <param name="column">O índice da coluna (base 0).</param>
        /// <param name="value">O valor a ser atribuído.</param>
        public void Set(int row, int column, string value)
        {
            // Validação de limites: Verificamos se a linha e a coluna estão dentro
            // do intervalo válido. Os índices em C# vão de 0 a (tamanho - 1).
            bool isRowInvalid = row < 0 || row >= this.Rows;
            bool isColumnInvalid = column < 0 || column >= this.Columns;

            if (isRowInvalid || isColumnInvalid)
            {
                // Se o índice estiver fora dos limites, lançamos nossa exceção personalizada.
                // Isso sinaliza claramente que o erro é uma tentativa de acesso
                // a uma posição inexistente na nossa matriz.
                throw new MatrixException();
            }

            // Se a validação passou, a operação é segura.
            m[row, column] = value;
        }

        /// <summary>
        /// Retorna uma string com todos os elementos de uma linha, unidos por um separador.
        /// </summary>
        /// <param name="index">O índice da linha a ser convertida.</param>
        /// <param name="separator">A string que separará os elementos.</param>
        /// <returns>Uma string formatada com os elementos da linha.</returns>
        public string RowToString(int index, string separator)
        {
            // 1. Validação dos argumentos:
            if (index < 0 || index >= this.Rows)
            {
                // O índice da linha é inválido.
                throw new MatrixException();
            }

            if (separator == null)
            {
                // O separador não pode ser nulo.
                throw new MatrixException();
            }

            // 2. Lógica de Construção da String:
            // Usar 'StringBuilder' é muito mais eficiente do que concatenar strings
            // com o operador '+' dentro de um laço, pois evita criar múltiplos
            // objetos de string na memória.
            StringBuilder sb = new StringBuilder();

            // Percorremos cada coluna da linha especificada ('index').
            for (int j = 0; j < this.Columns; j++)
            {
                // Adicionamos o elemento da matriz ao nosso construtor de string.
                sb.Append(m[index, j]);

                // Para evitar um separador sobrando no final, só o adicionamos
                // se este NÃO for o último elemento da linha.
                if (j < this.Columns - 1)
                {
                    sb.Append(separator);
                }
            }

            // Convertemos o conteúdo do StringBuilder para uma string final e a retornamos.
            return sb.ToString();
        }
    }
}