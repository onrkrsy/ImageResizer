using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace ImageResizer
{
    class Program
    {
        static void Main(string[] args)
        {
            ///uploads/1/news
            //string path = @"C:\Users\OnurKARASOY\Desktop\denemeler\images2"; //bu dizinin içinde bulunan tüm görselleri küçültür. subfolderlar dahil
            int maxWidth = 1920;
            int maxHeight = 1200;
            int maxFileSize = 400 * 1024; // 400Kb 

            do
            {
                Console.Write("Lütfen görsellerin yeniden boyutlandırılmasını istediğiniz bir dizin (path) girin: ");
                string path = Console.ReadLine();

                if (Directory.Exists(path))
                {
                    Console.Write($"\"{path}\" bu klasörde resize işlemi başlatılsın mı? (Y/N): ");
                    string response = Console.ReadLine();

                    if (response.Trim().ToUpper() == "Y")
                    {
                        ResizeImages(path, maxWidth, maxHeight, maxFileSize);
                    }
                    else
                    {
                        Console.WriteLine("Resize işlemi başlatılmadı.");
                    }
                }
                else
                {
                    Console.WriteLine("Geçersiz bir dizin girdiniz veya dizin bulunamadı.");
                }
                Console.Write("Yeni bir işlem başlatmak ister misiniz? (Y/N): ");
            }
            while (Console.ReadLine().Trim().ToUpper() == "Y");

        }

        static void ResizeImages(string path, int maxWidth, int maxHeight, int maxFileSize)
        {
            int totalFiles = 0;
            int currentFile = 1;
            totalFiles = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Length;
            foreach (string file in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
            {
                try
                {
                    string ext = Path.GetExtension(file).ToLower();
                    if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".bmp")
                    {
                        Console.WriteLine($"Resizing file {currentFile} of {totalFiles}");
                        currentFile++;
                        byte[] bytes;
                        using (Image image = Image.FromFile(file))
                        {
                            int width = image.Width;
                            int height = image.Height;

                            if (width > maxWidth)
                            {
                                height = (int)(height * ((double)maxWidth / width));
                                width = maxWidth;
                            }

                            if (height > maxHeight)
                            {
                                width = (int)(width * ((double)maxHeight / height));
                                height = maxHeight;
                            }

                            using (Image resizedImage = new Bitmap(width, height))
                            {
                                using (Graphics graphics = Graphics.FromImage(resizedImage))
                                {
                                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    graphics.DrawImage(image, 0, 0, width, height);
                                }

                                //string resizedFile = Path.Combine(Path.GetDirectoryName(file), "resized", Path.GetFileName(file));
                                string resizedFile = file;
                                Directory.CreateDirectory(Path.GetDirectoryName(resizedFile));

                                using (MemoryStream memoryStream = new MemoryStream())
                                {
                                    ImageCodecInfo encoderInfo = GetEncoderInfo("image/jpeg");
                                    EncoderParameters encoderParams = new EncoderParameters(1);
                                    encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 100L);

                                    resizedImage.Save(memoryStream, encoderInfo, encoderParams);
                                    bytes = memoryStream.ToArray();

                                    if (bytes.Length <= maxFileSize)
                                    {
                                        //File.WriteAllBytes(resizedFile, bytes);
                                        //Console.WriteLine($"Resized {file} to {resizedFile}");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Resizing {file} to reduce its size to less than {maxFileSize / 1024}Kb");

                                        double qualityRatio = Math.Sqrt((double)maxFileSize / bytes.Length);
                                        int qualityLevel = (int)Math.Round(qualityRatio * 100);

                                        encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, qualityLevel);

                                        memoryStream.Position = 0;
                                        resizedImage.Save(memoryStream, encoderInfo, encoderParams);
                                        bytes = memoryStream.ToArray();

                                        while (bytes.Length > maxFileSize && qualityLevel > 0)
                                        {
                                            qualityLevel -= 5;
                                            encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, qualityLevel);

                                            memoryStream.Position = 0;
                                            memoryStream.SetLength(0); // Reset stream to initial position
                                            resizedImage.Save(memoryStream, encoderInfo, encoderParams);
                                            bytes = memoryStream.ToArray();
                                        }

                                       
                                    }
                                }
                            }
                        }
                        if (bytes.Length <= maxFileSize)
                        {
                            var resizedFile = file;
                            File.WriteAllBytes(resizedFile, bytes);
                            Console.WriteLine($"Resized {file} to {resizedFile}");
                        }
                        else
                        {
                            Console.WriteLine($"Skipping {file} because it exceeds the maximum file size of {maxFileSize / 1024}Kb");
                        }
                    }
                }
                catch (Exception ext){
                    Console.WriteLine($"Skipping {file} because it throw an exception");
                }
            }
        }

        static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();

            for (int i = 0; i < encoders.Length; i++)
            {
                if (encoders[i].MimeType == mimeType)
                {
                    return encoders[i];
                }
            }

            return null;
        }
    }
}