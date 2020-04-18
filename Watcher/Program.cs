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
        public delegate void DirectoryHandler(string path);
        public event DirectoryHandler AddedFile;
        
        public void HandleDirectory(string path)
        {
            using FileSystemWatcher watcher = new FileSystemWatcher {Path = path};
            watcher.Created += OnAdded;
            watcher.EnableRaisingEvents = true;
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
            
            Watcher.HandleDirectory(path);
            
            while (Console.Read() != 'q');
        }
    }
}