using System;
using System.IO;
using GroupDocs.Watermark;
using GroupDocs.Watermark.Common;
using GroupDocs.Watermark.Watermarks;
using File = Watcher.File;

namespace Converter
{

    public interface IByteConverter
    {
        void Convert(byte[] bytes, Stream outputStream);
    }

    public class WatermarkOverlay : IByteConverter
    {
        public void Convert(byte[] bytes, Stream outputStream)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                var watermarker = new Watermarker(stream);
                
                Console.WriteLine(Directory.GetCurrentDirectory());
                using (var watermark = new ImageWatermark("./logo.png"))
                {
                    watermark.HorizontalAlignment = HorizontalAlignment.Right;
                    watermark.VerticalAlignment = VerticalAlignment.Bottom;
                    watermarker.Add(watermark);
                } 

                watermarker.Save(outputStream);
                Console.WriteLine($"Изображение получено {outputStream.Length}");
            }

        }
    }

    public class FileDispatcher
    {

        private IByteConverter _converter;
        private string _directory;
        public FileDispatcher(IByteConverter converter, string saveDirectory)
        {
            _converter = converter;
            _directory = saveDirectory;
        }
        
        public void TransformFile(File image)
        {
            Console.WriteLine($"Изображение получено {image.data.Length}");
            var newFilePath = Path.Combine(_directory, image.name);
            
            using (var file = System.IO.File.OpenWrite(newFilePath))
            {
                _converter.Convert(image.data, file);
                file.Close();
            }
            
            Console.WriteLine("Изображение сохранено в " + newFilePath);
        }
    }


    class Program
    {
       
        static void Main(string[] args)
        {
            const string subpath = "outcoming";
            var path = Path.Combine(Directory.GetCurrentDirectory(), subpath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var logoPath = "../../../logo.png";
            if (!System.IO.File.Exists("./logo.png"))
            {
                System.IO.File.Copy(logoPath, Path.Combine(Directory.GetCurrentDirectory(), "logo.png"));
            }

            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "outcoming");
            using var receiver = new RabbitReceiver();
            var fileDispatcher = new FileDispatcher(new WatermarkOverlay(), pathToSave);
            receiver.NewFileReceived += fileDispatcher.TransformFile;

            Console.WriteLine("Press q to quit");
            while (Console.Read() != 'q') ;
            
        }
    }
}