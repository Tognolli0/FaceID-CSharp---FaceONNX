using FaceONNX;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.Versioning;

namespace FaceRecognitionDemo
{
    [SupportedOSPlatform("windows")]
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Demo de Reconhecimento Facial Profissional ---");

            string baseDir = AppContext.BaseDirectory;
            string path1 = args.Length > 0 ? args[0] : Path.Combine(baseDir, "Images", "pessoa1.jpg");
            string path2 = args.Length > 1 ? args[1] : Path.Combine(baseDir, "Images", "pessoa2.jpg");
            double threshold = args.Length > 2 && double.TryParse(args[2], out var parsedThreshold)
                ? parsedThreshold
                : 4d;

            if (!File.Exists(path1) || !File.Exists(path2))
            {
                Console.WriteLine("ERRO: Fotos nao encontradas na pasta Images.");
                Console.WriteLine("Uso opcional: FaceRecognitionDemo <imagem1> <imagem2> <threshold>");
                return;
            }

            try
            {
                using var faceDetector = new FaceDetector();
                using var faceEmbedder = new FaceEmbedder();

                using var bmp1 = LoadBitmapSafe(path1);
                using var bmp2 = LoadBitmapSafe(path2);

                var results1 = faceDetector.Forward(bmp1);
                var results2 = faceDetector.Forward(bmp2);

                if (results1.Length == 0 || results2.Length == 0)
                {
                    Console.WriteLine("AVISO: Rosto nao detectado em uma das fotos.");
                    return;
                }

                using var face1 = CropAndResize(bmp1, results1[0].Box, 112, 112);
                using var face2 = CropAndResize(bmp2, results2[0].Box, 112, 112);

                var emb1 = faceEmbedder.Forward(face1);
                var emb2 = faceEmbedder.Forward(face2);
                double distance = CalculateEuclideanDistance(emb1, emb2);

                Console.WriteLine($"\nDistancia calculada: {distance:F4}");
                Console.WriteLine($"Threshold utilizado: {threshold:F2}");

                if (distance < threshold)
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

        static Bitmap LoadBitmapSafe(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);
            using var ms = new MemoryStream(bytes);
            return new Bitmap(ms);
        }

        static Bitmap CropAndResize(Bitmap source, Rectangle section, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            using Graphics g = Graphics.FromImage(bmp);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(source, new Rectangle(0, 0, width, height), section, GraphicsUnit.Pixel);
            return bmp;
        }

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
