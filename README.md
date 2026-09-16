# GeneratedCodeByAI

Repositório de dados brutos de um Trabalho de Conclusão de Curso (TCC) que avalia a qualidade do código C# gerado por diferentes LLMs. Ele armazena o **código-fonte gerado automaticamente** por quatro modelos de linguagem — **GPT**, **Claude**, **Gemini** e **DeepSeek** — a partir de prompts de programação submetidos por participantes reais, produzido pela API [AIConnection](https://github.com/IngridBatista/AIConnection).

## O que este repositório contém

Cada solução de programação gerada por um modelo, para um determinado exercício e um determinado participante, fica isolada em seu próprio projeto .NET (`.csproj`), permitindo compilar, analisar e testar cada saída de forma independente, sem interferência entre os experimentos.

A raiz do repositório reúne:

- **84 projetos** (pastas), um para cada combinação de **modelo × questão × participante**
- Um arquivo de solução `GeneratedCodeByAI.slnx`, que referencia todos os 84 projetos e permite abrir o conjunto completo de uma vez no Visual Studio
- 104 arquivos `.cs` no total, onde a maioria dos projetos contém uma única classe de solução, mas alguns modelos geraram classes auxiliares adicionais (ex.: exceções customizadas)

## Convenção de nomes

Cada pasta de projeto segue o padrão:

```
{MODELO}.{QUESTAO}.{SENIORIDADE}.PARTICIPANT_{N}
```

Exemplo: `CLAUDE.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_1` → solução gerada pelo Claude, para o exercício "ArrayDifference", a partir do prompt de um participante sênior (Participant 1).

O namespace e o nome da classe dentro do `.cs` seguem a mesma convenção do diretório, garantindo rastreabilidade total entre pasta, projeto e código.

### Modelos

| Modelo | Prefixo da pasta |
|---|---|
| GPT | `GPT.*` |
| Claude | `CLAUDE.*` |
| Gemini | `GEMINI.*` |
| DeepSeek | `DEEPSEEK.*` |

### Questões (exercícios de programação)

- **ArrayDifference** — obtenção dos elementos exclusivos entre dois arrays
- **SequenceComparison** — comparação de sequências
- **StringArrayEncoding** — codificação de arrays de strings (matriz de strings)

### Participantes e senioridade

7 participantes, cujos prompts variam em senioridade: 5 sêniores (Participant 1 a 5), 1 pleno (Participant 6) e 1 júnior (Participant 7).

## Estrutura de um projeto individual

```
CLAUDE.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_1/
├── CLAUDE.ARRAY_DIFFERENCE.SENIOR.PARTICIPANT_1.csproj   # net10.0, Nullable/ImplicitUsings habilitados
└── ClaudeArrayDifferenceSeniorParticipant1.cs            # classe com a solução gerada pelo modelo
```

Todos os projetos usam o mesmo `TargetFramework` (`net10.0`) e as mesmas configurações (`Nullable`/`ImplicitUsings` habilitados), o que padroniza a compilação e viabiliza a comparação justa entre as soluções.

## Como este repositório é usado no TCC

O código aqui armazenado é a matéria-prima da fase de avaliação do estudo, que aplica:

- **CodeBLEU** — similaridade estrutural/sintática entre cada solução gerada e a solução de referência (especialista)
- **SonarQube** — análise estática de qualidade e code smells
- **Testes de comportamento (xUnit)** — verificação funcional das soluções

## Origem dos dados

O código deste repositório não foi escrito manualmente: cada arquivo `.cs` é a extração direta da resposta de um LLM, obtida via API do projeto [AIConnection](https://github.com/IngridBatista/AIConnection) e salva sem alterações de lógica (apenas formatação/indentação via Roslyn).
