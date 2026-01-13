using FaceONNX;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace FaceRecognitionDemo
{
    [SupportedOSPlatform("windows")]
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Demo de Reconhecimento Facial Profissional ---");

            // Caminhos das imagens
            string baseDir = AppContext.BaseDirectory;
            string path1 = Path.Combine(baseDir, "Images", "pessoa1.jpg");
            string path2 = Path.Combine(baseDir, "Images", "pessoa2.jpg");

            if (!File.Exists(path1) || !File.Exists(path2))
            {
                Console.WriteLine("ERRO: Fotos não encontradas na pasta Images.");
                return;
            }

            try
            {
                // Inicializa os modelos de IA
                using var faceDetector = new FaceDetector();
                using var faceEmbedder = new FaceEmbedder();

                // Carrega as imagens de forma segura para não travar o arquivo
                using var bmp1 = LoadBitmapSafe(path1);
                using var bmp2 = LoadBitmapSafe(path2);

                // 1. Detecção de rostos
                var results1 = faceDetector.Forward(bmp1);
                var results2 = faceDetector.Forward(bmp2);

                if (results1.Length == 0 || results2.Length == 0)
                {
                    Console.WriteLine("AVISO: Rosto não detectado em uma das fotos.");
                    return;
                }

                // 2. Recorte e Redimensionamento (Normalização)
                // Redimensionamos para 112x112 que é o padrão de muitas redes neurais de face
                using var face1 = CropAndResize(bmp1, results1[0].Box, 112, 112);
                using var face2 = CropAndResize(bmp2, results2[0].Box, 112, 112);

                // 3. Extração de Características (Embeddings)
                var emb1 = faceEmbedder.Forward(face1);
                var emb2 = faceEmbedder.Forward(face2);

                // 4. Cálculo da Distância Euclidiana
                double distance = CalculateEuclideanDistance(emb1, emb2);

                Console.WriteLine($"\nDistância calculada: {distance:F4}");
                
                // AJUSTE: Threshold de 0.65 é ideal para a maioria dos cenários
                if (distance < 4)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("RESULTADO: MESMA PESSOA!");
}
else
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("RESULTADO: PESSOAS DIFERENTES.");
}
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }

            Console.WriteLine("\nPressione qualquer tecla para sair.");
            Console.ReadKey();
        }

        // Carrega a imagem sem bloquear o arquivo no disco (importante para OneDrive)
        static Bitmap LoadBitmapSafe(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);
            using (var ms = new MemoryStream(bytes))
            {
                return new Bitmap(ms);
            }
        }

        // Recorta o rosto e redimensiona para um tamanho padrão (Normalização)
        static Bitmap CropAndResize(Bitmap source, Rectangle section, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(source, new Rectangle(0, 0, width, height), section, GraphicsUnit.Pixel);
            }
            return bmp;
        }

        // Algoritmo de comparação matemática
        static double CalculateEuclideanDistance(float[] v1, float[] v2)
        {
            double sum = 0;
            for (int i = 0; i < v1.Length; i++)
            {
                double diff = v1[i] - v2[i];
                sum += diff * diff;
            }
            return Math.Sqrt(sum);
        }
    }
}