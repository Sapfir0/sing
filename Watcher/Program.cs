using System;
using System.IO;

namespace Watcher
{
    

 
    class Program
    {
        static void Main(string[] args)
        {
            const string subpath = "incoming";
            var path = Path.Combine(Directory.GetCurrentDirectory(), subpath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            Console.WriteLine("Детектим изменения в " + path);
            
            var watcher = new Watcher();
            using var sender = new RabbitSender();
            watcher.HandleDirectory(path);
            var dispatcher = new Dispatcher(watcher, sender);

            Console.WriteLine("Press q for quit"); // Engfish, bro 
            while (Console.Read() != 'q') ;
        }
    }
}