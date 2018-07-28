using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace CardboardCameraVRPhotoExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("ファイルパスを渡すと変換します");
                return;
            }

            if (args.All(a => !a.EndsWith(".vr.jpg")))
            {
                Console.WriteLine(".vr.jpg にしか対応していません");
                return;
            }

            for (var i = 0; i < args.Length; i++)
            {
                var filePath = args[i];
                var prefix = $"[{i.ToString("000")}/{args.Length.ToString("000")}] ";

                if (!filePath.EndsWith(".vr.jpg"))
                {
                    Console.WriteLine(prefix + "非対応: " + filePath);
                    continue;
                }

                try
                {
                    var metadata = MetadataReader.Read(filePath);
                    var combinedImagePath = filePath.Replace(".vr.jpg", ".joined.jpg");

                    using (var leftStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    using (var rightStream = new MemoryStream(metadata.RightImageBytes, false))
                    {
                        SaveCombinedImage(leftStream, rightStream, combinedImagePath);
                    }

                    var csvPath = filePath.Replace(".vr.jpg", ".csv");
                    SaveViewCSV(metadata.Pitch, metadata.Roll, csvPath);
                    Console.WriteLine(prefix + "完了: " + filePath);
                }
                catch
                {
                    Console.WriteLine(prefix + "エラー: " + filePath);
                }
            }
        }

        static void SaveCombinedImage(Stream left, Stream right, string destination)
        {
            using (var leftImage = Image.FromStream(left))
            using (var rightImage = Image.FromStream(right))
            using (var combinedImage = new Bitmap(leftImage.Width + rightImage.Width, leftImage.Height))
            using (var graphics = Graphics.FromImage(combinedImage))
            {
                graphics.DrawImage(leftImage, 0, 0);
                graphics.DrawImage(rightImage, leftImage.Width, 0);

                combinedImage.Save(destination);
            }
        }

        static void SaveViewCSV(float pitch, float roll, string destination)
        {
            File.WriteAllText(destination, $"{pitch},{roll}");
        }
    }
}
