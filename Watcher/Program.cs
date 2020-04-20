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
            var bytes = File.ReadAllBytes(path); 
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
            watcher.HandleDirectory(path);
            var dispatcher = new Dispatcher(watcher, new RabbitSender());


            while (Console.Read() != 'q') ;
        }
    }
}