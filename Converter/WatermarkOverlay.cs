using System;
using System.IO;
using GroupDocs.Watermark;
using GroupDocs.Watermark.Common;
using GroupDocs.Watermark.Watermarks;

namespace Converter
{
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
}