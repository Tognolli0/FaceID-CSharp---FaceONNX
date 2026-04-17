# FaceID CSharp FaceONNX

Prova de conceito de reconhecimento facial offline em C#, com comparacao de embeddings gerados localmente sem dependencia de servicos em nuvem.

## Stack

- .NET 8
- FaceONNX
- ONNX Runtime
- System.Drawing.Common

## Objetivo

Comparar duas imagens de rosto e indicar se pertencem ou nao a mesma pessoa com processamento totalmente local.

## Diferenciais

- Execucao offline
- Sem custo por chamada em nuvem
- Foco em privacidade
- Threshold ajustavel para cenarios reais

## Estrutura

- `Program.cs`: fluxo principal de leitura e comparacao
- `Images/`: imagens de exemplo
- `FaceRecognitionDemo.csproj`: projeto principal

## Como rodar

1. Instale o .NET 8 SDK.
2. Restaure os pacotes do projeto.
3. Ajuste ou substitua as imagens da pasta `Images`.
4. Execute a aplicacao via `dotnet run`.
5. Opcionalmente, rode `dotnet run -- <imagem1> <imagem2> <threshold>` para testar outros arquivos e limiares.

## Resultado esperado

O programa calcula a distancia entre embeddings faciais e informa se as imagens representam a mesma pessoa dentro do threshold definido.
