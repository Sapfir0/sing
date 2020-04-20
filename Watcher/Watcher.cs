using System;
using System.IO;

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

}