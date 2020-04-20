using System;
using System.IO;

using System.Drawing;
using System.Reflection.Metadata;
using GroupDocs.Watermark;
using GroupDocs.Watermark.Common;
using GroupDocs.Watermark.Watermarks;

namespace Converter
{

    public interface IByteConverter
    {
        /*byte[] Convert(byte[] imageBytes)
        {

        }*/
    }

    public class WatermarkOverlay
    {
        public static GroupDocs.Watermark.Watermarker PutWatermark(File image)
        {
            using var stream = System.IO.File.Open(image.name, FileMode.Open, FileAccess.ReadWrite);
            using var watermarker = new Watermarker(stream);
            using (var watermark = new ImageWatermark("./logo.png"))
            {
                watermark.HorizontalAlignment = HorizontalAlignment.Right;
                watermark.VerticalAlignment = VerticalAlignment.Bottom;
                watermarker.Add(watermark);
            }

            return watermarker;
            
        }

    }

    public class Getter
    {
        public static void GetMessage(byte[] image)
        {
            Console.WriteLine("Изображение получено");
            var imageInstance = new File() {data = image};
            var watermarked = WatermarkOverlay.PutWatermark(imageInstance);
            watermarked.Save( Path.Combine("outcoming", imageInstance.name));

        }
    }
    
    
    public class File
    {
        public string name { get; set; }
        public byte[] data { get; set; }
    }

    
    class Program
    {
       
        static void Main(string[] args)
        {
            using var receiver = new RabbitReceiver();
            receiver.NewMessage += msg => Getter.GetMessage(msg);
            while (Console.Read() != 'q') ;
            
        }
    }
}