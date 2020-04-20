using Watcher;

namespace Converter
{
    public interface IReceiver
    {
        public delegate void NewFileHandler(File data);
        public event NewFileHandler NewFileReceived;
    }
}