using System;
using System.IO;
using GroupDocs.Watermark;
using GroupDocs.Watermark.Common;
using GroupDocs.Watermark.Watermarks;

namespace Converter
{
    public class WatermarkOverlay : IStreamConverter
    {
        public void Convert(Stream inputStream, Stream outputStream)
        {
            var watermarker = new Watermarker(inputStream);
            
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