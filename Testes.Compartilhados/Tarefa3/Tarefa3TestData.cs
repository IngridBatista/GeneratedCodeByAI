using System;
using System.Collections.Generic;
using System.Text;

namespace Testes.Compartilhados.Tarefa3
{

    /// <summary>
    /// Fonte única de casos de teste para a Questão 3 (MatrixString), baseada
    /// estritamente no enunciado oficial:
    ///
    ///   1. Construtor: MatrixString(int rows, int columns, string value) —
    ///      inicializa uma matriz rows x columns com "value" em cada posição.
    ///      Lança ArgumentException (construtor padrão, sem mensagem) se rows
    ///      ou columns não forem números naturais positivos (> 0).
    ///
    ///   2. Set(int row, int column, string value) — atribui "value" à posição
    ///      [row, column]. Lança MatrixException (construtor padrão) se row ou
    ///      column estiverem fora dos limites da matriz.
    ///
    ///   3. RowToString(int index, string separator) — concatena os elementos
    ///      da linha "index", separados por "separator". Lança MatrixException
    ///      se index não for uma linha válida OU se separator for null.
    ///
    /// IMPORTANTE: como o enunciado pede explicitamente o "construtor padrão"
    /// das exceções (sem mensagem customizada), os testes verificam apenas o
    /// TIPO da exceção lançada — nunca o texto da mensagem, que seria genérico
    /// e não é parte do contrato da tarefa.
    ///
    /// Assinaturas são exatas (assim como na Questão 1), então os testes podem
    /// instanciar e chamar os métodos diretamente, sem simulação de console.
    /// </summary>
    public static class Tarefa3TestData
    {
        // ---------------------------------------------------------------
        // Construtor — casos válidos (deve construir sem lançar exceção)
        // (rows, columns, value)
        // ---------------------------------------------------------------
        public static IEnumerable<object[]> CasosConstrutorValido()
        {
            yield return new object[] { 1, 1, "x" };
            yield return new object[] { 3, 3, "0" };
            yield return new object[] { 2, 5, "abc" };
            yield return new object[] { 5, 2, "" };       // value vazio é permitido (enunciado não proíbe)
            yield return new object[] { 1, 10, "a" };
            yield return new object[] { 10, 1, "b" };
        }

        // ---------------------------------------------------------------
        // Construtor — casos inválidos (deve lançar ArgumentException)
        // (rows, columns)
        // ---------------------------------------------------------------
        public static IEnumerable<object[]> CasosConstrutorInvalido()
        {
            yield return new object[] { 0, 5 };    // rows = 0 (não é positivo)
            yield return new object[] { 5, 0 };    // columns = 0
            yield return new object[] { 0, 0 };    // ambos zero
            yield return new object[] { -1, 5 };   // rows negativo
            yield return new object[] { 5, -1 };   // columns negativo
            yield return new object[] { -3, -3 };  // ambos negativos
        }

        // ---------------------------------------------------------------
        // RowToString — matriz recém-construída (sem chamadas a Set), casos válidos
        // (rows, columns, value, rowIndex, separator, esperado)
        // ---------------------------------------------------------------
        public static IEnumerable<object[]> CasosRowToStringAposConstrucao()
        {
            yield return new object[] { 3, 3, "0", 0, ",", "0,0,0" };
            yield return new object[] { 2, 4, "x", 1, "-", "x-x-x-x" };
            yield return new object[] { 1, 1, "solo", 0, ";", "solo" };
            yield return new object[] { 3, 3, "0", 0, "", "000" };       // separador vazio
            yield return new object[] { 2, 3, "ab", 0, "|", "ab|ab|ab" };
        }

        // ---------------------------------------------------------------
        // RowToString — índice inválido (deve lançar MatrixException)
        // (rows, columns, value, indiceInvalido)
        // ---------------------------------------------------------------
        public static IEnumerable<object[]> CasosRowToStringIndiceInvalido()
        {
            yield return new object[] { 3, 3, "0", -1 };   // negativo
            yield return new object[] { 3, 3, "0", 3 };    // igual ao count (fora, já que índices vão de 0 a 2)
            yield return new object[] { 3, 3, "0", 100 };  // muito acima do limite
            yield return new object[] { 1, 1, "x", 1 };    // única linha é índice 0; 1 é inválido
        }

        // ---------------------------------------------------------------
        // Set — índices válidos, seguido de RowToString para confirmar a alteração
        // (rows, columns, valorInicial, setRow, setColumn, novoValor, rowParaVerificar, separator, esperado)
        // ---------------------------------------------------------------
        public static IEnumerable<object[]> CasosSetValidoERowToString()
        {
            yield return new object[] { 3, 3, "0", 1, 1, "9", 1, ",", "0,9,0" };
            yield return new object[] { 2, 2, "a", 0, 0, "z", 0, "", "za" };
            yield return new object[] { 3, 1, "x", 2, 0, "y", 2, "-", "y" };
            yield return new object[] { 1, 4, "0", 0, 3, "9", 0, ",", "0,0,0,9" };
        }

        // ---------------------------------------------------------------
        // Set — índices inválidos (deve lançar MatrixException)
        // (rows, columns, invalidRow, invalidColumn)
        // ---------------------------------------------------------------
        public static IEnumerable<object[]> CasosSetIndiceInvalido()
        {
            yield return new object[] { 3, 3, -1, 0 };   // row negativo
            yield return new object[] { 3, 3, 0, -1 };   // column negativo
            yield return new object[] { 3, 3, 3, 0 };    // row igual ao count (fora)
            yield return new object[] { 3, 3, 0, 3 };    // column igual ao count (fora)
            yield return new object[] { 3, 3, 5, 5 };    // ambos muito fora
        }
    }
}
