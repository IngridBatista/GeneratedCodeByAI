using System;
using System.Collections.Generic;
using System.Text;

namespace Testes.Compartilhados.Tarefa2
{
    /// <summary>
    /// Fonte única de casos de teste para a Questão 2 (CompareSequence), focada no
    /// requisito 3 do enunciado: imprimir as frações de B cujo valor seja maior do
    /// que pelo menos metade dos números de A.
    ///
    /// Estratégia: os casos são expressos como STRINGS DE ENTRADA DE CONSOLE
    /// (simulando exatamente o que um usuário digitaria), não como listas de
    /// double/Fraction em memória. Isso permite reutilizar os mesmos casos entre
    /// participantes diferentes, independentemente de como cada um estrutura
    /// internamente os dados (List&lt;double&gt;, List&lt;int&gt;, métodos estáticos
    /// vs. instância, lógica em Main() vs. em método separado) — todos leem do
    /// Console no mesmo formato de texto.
    ///
    /// Cada entrada já inclui os terminadores necessários:
    ///   - "0" para encerrar a sequência A
    ///   - uma fração negativa (ex: "-1/1") para encerrar a sequência B
    ///
    /// Usa apenas valores INTEIROS em A (mesmo que o enunciado exija double),
    /// para que os casos sejam comparáveis mesmo entre implementações com bugs
    /// de tipo (ex: GPT Participante 3, que só aceita int em A). O comportamento
    /// específico com valores decimais deve ser testado à parte, por participante.
    /// </summary>
    public static class Tarefa2TestData
    {
        public static IEnumerable<object[]> CasosObrigatoriosFiltro()
        {
            yield return new object[]
            {
                "CasoNormal_ElementoClaramenteMaiorEMenor",
                "1\n2\n3\n4\n5\n0\n10/1\n1/2\n-1/1\n",
                new (int Numerador, int Denominador)[] { (10, 1) }
            };

            yield return new object[]
            {
                "ContagemImparNoLimiar_DeveExcluir",
                // A = [1,2,3,4,5] (n=5). Fração 3/1 (valor=3): elementos estritamente
                // menores que 3 são {1,2} -> greaterCount=2. Critério correto: 2 >= 2.5 -> FALSO.
                // Se houvesse bug de divisão inteira (5/2=2), o critério "2 >= 2" seria
                // erroneamente TRUE. Este caso detecta esse bug clássico.
                "1\n2\n3\n4\n5\n0\n3/1\n-1/1\n",
                new (int, int)[] { }
            };

            yield return new object[]
            {
                "ContagemParNoLimiarExato_DeveIncluir",
                // A = [1,2,3,4] (n=4). Fração 3/1 (valor=3): greaterCount=2. Critério: 2 >= 2.0 -> TRUE.
                "1\n2\n3\n4\n0\n3/1\n-1/1\n",
                new (int, int)[] { (3, 1) }
            };

            yield return new object[]
            {
                "NenhumaFracaoQualifica",
                "5\n6\n7\n0\n1/10\n-1/1\n",
                new (int, int)[] { }
            };

            yield return new object[]
            {
                "TodasAsFracoesQualificam",
                "1\n2\n0\n100/1\n50/1\n-1/1\n",
                new (int Numerador, int Denominador)[] { (100, 1), (50, 1) }
            };

            yield return new object[]
            {
                "FracaoZeroNaoEhTratadaComoSentinela",
                // 0/1 tem valor 0 (não negativo) -> deve ser lida normalmente como parte de B,
                // não deve encerrar a leitura. Se a implementação tratar erroneamente 0 como
                // sentinela, a fração 5/1 nunca seria lida, e o resultado ficaria vazio.
                "1\n2\n3\n0\n0/1\n5/1\n-1/1\n",
                new (int, int)[] { (5, 1) }
            };

            yield return new object[]
            {
                "EmpateNaoContaComoMaior_ComparacaoDeveSerEstrita",
                // A = [3,3,3,3,3] (n=5). Fração 3/1 (valor=3): nenhum elemento de A é
                // ESTRITAMENTE menor que 3 (todos iguais) -> greaterCount=0. 0 >= 2.5 -> FALSO.
                "3\n3\n3\n3\n3\n0\n3/1\n-1/1\n",
                new (int, int)[] { }
            };
        }

        // ---------------------------------------------------------------
        // Cenários de sequência vazia (requisito 4). O texto da mensagem de
        // erro varia por participante — cada teste específico valida sua
        // própria mensagem; aqui só compartilhamos a ENTRADA simulada.
        // ---------------------------------------------------------------

        public const string EntradaSequenciaAVazia = "0\n-1/1\n";
        public const string EntradaSequenciaBVazia = "1\n2\n0\n-1/1\n";
        public const string EntradaAmbasSequenciasVazias = "0\n-1/1\n";
    }
}
