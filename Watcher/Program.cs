using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Text;

namespace Watcher
{
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
            // var bytes = File.ReadAllBytes(path); 
            var bytes = new byte[] {0, 0, 1};
            _sender.send(bytes);
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


            while (Console.Read() != 'q') ;
        }
    }
}