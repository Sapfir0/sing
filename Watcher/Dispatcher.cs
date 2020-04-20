using System.IO;

namespace Watcher
{
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
            System.IO.File.Delete(path);

        }
    }
}