using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Text;
using test;

namespace Watcher
{
    class Watcher
    {
        public static void HandleDirectory(string path)
        {
            using FileSystemWatcher watcher = new FileSystemWatcher {Path = path};
            watcher.Created += OnChanged;
            watcher.EnableRaisingEvents = true;
            
            while(true);
        }
        private static void OnChanged(object source, FileSystemEventArgs e)
        {           
            Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
            
        }
    }

    class Dispatcher
    {
        
    }
    class ImageSender
    {
        private Sender _sender;
        public ImageSender(Sender sender)
        {
            _sender = sender;
        }
        

    }
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

            Watcher.HandleDirectory(path);

        }
    }
}