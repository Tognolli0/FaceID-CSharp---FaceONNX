🤖 FaceID Demo - Reconhecimento Facial Offline
Este projeto é uma prova de conceito (PoC) de um sistema de biometria facial desenvolvido em C# (.NET 8.0). O sistema é capaz de identificar se duas imagens pertencem à mesma pessoa utilizando inteligência artificial de ponta, rodando de forma 100% local.

🌟 Diferenciais Técnicos
Privacidade e Custo Zero: Diferente de soluções em nuvem (Azure/AWS), este motor processa tudo localmente usando ONNX Runtime, garantindo que os dados biométricos não saiam da máquina.

Calibração de Threshold: Implementação de um limiar de confiança ajustado (4.0) para lidar com variações reais de iluminação, ângulo e ruído de câmera, reduzindo falsos negativos.

Arquitetura Robusta: Carregamento de imagens via buffer de memória para evitar conflitos de leitura em ambientes de sincronização (como OneDrive).

🛠️ Tecnologias Utilizadas
FaceONNX: Engine de Deep Learning para detecção e extração de características faciais.

Microsoft.ML.OnnxRuntime: Runtime para execução de modelos de IA com alta performance.

System.Drawing.Common: Manipulação e normalização de bitmaps.

📊 Como o Sistema Toma Decisões
O sistema transforma o rosto humano em um vetor matemático de 512 dimensões. A comparação é feita através do cálculo da Distância Euclidiana:

Distância Calculada,Veredito,Confiança
0.0 - 0.5,Mesma Pessoa,Identidade Exata
0.6 - 3.9,Mesma Pessoa,Reconhecimento com variação de ambiente
> 4.0,Pessoas Diferentes,Acesso Negado / Intruso
