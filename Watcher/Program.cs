using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RabbitMQ.Client;

namespace Watcher
{
    class Program
    {
        private byte[] Image2Byte()
        {
            throw new NotImplementedException();
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {           
            Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
        }

        private static void HandleDirectory(string path)
        {
            using FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;


            watcher.NotifyFilter = NotifyFilters.LastAccess
                                   | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName
                                   | NotifyFilters.DirectoryName;


            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;

            watcher.EnableRaisingEvents = true;
        }


        static void Main(string[] args)
        {
            const string subpath = "/incoming";
            var path = Directory.GetCurrentDirectory() + subpath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            var filesOnStart = Directory.GetFiles(path);
            var sandedFiles = new List<string>();

            HandleDirectory(path);
        }
    }
}