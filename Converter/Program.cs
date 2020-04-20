using System;
using System.IO;


namespace Converter
{
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