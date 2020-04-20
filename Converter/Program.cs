using System;
using System.IO;

using System.Drawing;
using System.Reflection.Metadata;
using GroupDocs.Watermark;
using GroupDocs.Watermark.Common;
using GroupDocs.Watermark.Watermarks;
using Watcher;
using File = Watcher.File;

namespace Converter
{

    public interface IByteConverter
    {
        void Convert(byte[] bytes, Stream outputStream);
    }

    public class WatermarkOverlay : IByteConverter
    {
// файл умер

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
                } // он в файловой системе сохранит
                watermarker.Save(outputStream);
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
            
            using (var file = System.IO.File.Open(newFilePath, System.IO.FileMode.Create))
            {
                _converter.Convert(image.data, file);
                file.Close();
            }
// и
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

            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "outcoming");
            using var receiver = new RabbitReceiver();
            var fileDispatcher = new FileDispatcher(new WatermarkOverlay(), pathToSave);
            receiver.NewFileReceived += fileDispatcher.TransformFile;
            
            Console.WriteLine("Press q to quit");
            while (Console.Read() != 'q') ;
            
        }
    }
}