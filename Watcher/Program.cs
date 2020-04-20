using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace Watcher
{
    
    public class File
    {
        public string name { get; set; }
        public byte[] data { get; set; }
    }
    class Watcher : IDisposable
    {
        private FileSystemWatcher _watcher;
        public delegate void DirectoryHandler(string path);
        public event DirectoryHandler AddedFile;
        
        public void HandleDirectory(string path)
        {
            _watcher = new FileSystemWatcher {Path = path};

            _watcher.Created += OnAdded;
            _watcher.EnableRaisingEvents = true;
        }


        private void OnAdded(object source, FileSystemEventArgs e)
        {
            var stop = false;
            while (!stop)
            {
                try
                {
                    using var file = System.IO.File.OpenRead(e.FullPath);
                    stop = true;
                }
                catch (IOException)
                {
                    stop = false;
                }
            }
            AddedFile?.Invoke(e.FullPath);
        }

        public void Dispose()
        {
            _watcher.Dispose();
        }
    }

    class Dispatcher
    {
        private ISender _sender;
        private Watcher _watcher;
        public Dispatcher(Watcher watcher, ISender sender)
        {
            _sender = sender;
            _watcher = watcher;
            _watcher.AddedFile += SendFile;
        }

        public void SendFile(string path)
        {
            var bytes = System.IO.File.ReadAllBytes(path); 
            _sender.send(new File()
            {
                data = bytes,
                name = Path.GetFileName(path)
            });
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
            
            var watcher = new Watcher();
            using var sender = new RabbitSender();
            watcher.HandleDirectory(path);
            var dispatcher = new Dispatcher(watcher, sender);

            Console.WriteLine("Press q for quit"); // Engfish, bro 
            while (Console.Read() != 'q') ;
        }
    }
}