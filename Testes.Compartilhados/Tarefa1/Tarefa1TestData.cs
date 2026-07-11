using System;
using System.Collections.Generic;
using System.Text;

namespace Testes.Compartilhados.Tarefa1
{
    /// <summary>
    /// Fonte única de casos de teste para a Questão 1 (Difference), baseada estritamente
    /// no enunciado oficial:
    ///
    ///   "O método deve retornar um array de inteiros contendo todos e somente os
    ///    elementos do array a que não aparecem no array b. Você deve assumir que o
    ///    método sempre será chamado com argumentos não nulos."
    ///
    /// Por isso, esta classe NÃO inclui casos de argumento nulo — o enunciado isenta
    /// explicitamente a implementação de tratar esse cenário. Testes de robustez com
    /// nulo (opcionais, fora do contrato) devem ficar em uma classe separada,
    /// marcados com [Trait("Escopo", "ForaDoContrato")] e não contados na taxa de
    /// aprovação oficial.
    /// </summary>
    public static class Tarefa1TestData
    {
        public static IEnumerable<object[]> CasosObrigatorios()
        {
            yield return new object[]
            {
                "CasoNormal_ElementosComunsEUnicos",
                new[] { 1, 3, 5, 7, 9 },
                new[] { 1, 5, 7 },
                new[] { 3, 9 }
            };

            yield return new object[]
            {
                "PrimeiroArrayVazio",
                new int[] { },
                new[] { 1, 2, 3 },
                new int[] { }
            };

            yield return new object[]
            {
                "SegundoArrayVazio",
                new[] { 1, 2, 3 },
                new int[] { },
                new[] { 1, 2, 3 }
            };

            yield return new object[]
            {
                "AmbosArraysVazios",
                new int[] { },
                new int[] { },
                new int[] { }
            };

            yield return new object[]
            {
                "TodosElementosDeAEmB",
                new[] { 1, 2, 3 },
                new[] { 1, 2, 3 },
                new int[] { }
            };

            yield return new object[]
            {
                "NenhumElementoDeAEmB",
                new[] { 1, 2, 3 },
                new[] { 4, 5, 6 },
                new[] { 1, 2, 3 }
            };

            yield return new object[]
            {
                "ComNumerosNegativos",
                new[] { -1, -2, -3 },
                new[] { -2, -3, -4 },
                new[] { -1 }
            };

            yield return new object[]
            {
                "BComElementosQueNaoExistemEmA",
                new[] { 1, 2, 3 },
                new[] { 9, 10 },
                new[] { 1, 2, 3 }
            };

            yield return new object[]
            {
                "PreservaOrdemOriginalDeA",
                new[] { 5, 3, 1, 4, 2 },
                new[] { 3, 4 },
                new[] { 5, 1, 2 }
            };
        }

        // ---------------------------------------------------------------
        // Caso 10 — AMBÍGUO: o enunciado não define o comportamento esperado
        // para duplicatas em `a`. Não entra em CasosObrigatorios() porque não
        // há gabarito único; deve ser tratado como categoria de análise à
        // parte (quantas implementações preservam vs. deduplicam), e não
        // como critério de aprovação/reprovação.
        // ---------------------------------------------------------------
        public static readonly int[] DuplicatasEntradaA = { 1, 1, 2, 2 };
        public static readonly int[] DuplicatasEntradaB = { 2, 3, 4 };
        public static readonly int[] DuplicatasEsperadoPreservandoOcorrencias = { 1, 1 };
        public static readonly int[] DuplicatasEsperadoDeduplicado = { 1 };
    }
}
