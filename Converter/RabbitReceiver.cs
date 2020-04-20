using System;
using EasyNetQ;
using Watcher;

namespace Converter
{
    public class RabbitReceiver : IDisposable, IReceiver
    {
        private IBus _bus;
        public event IReceiver.NewFileHandler NewFileReceived;
        public RabbitReceiver()
        {
            _bus = GetRabbitConnection();
            _bus.Subscribe<File>("id", ReceiveMessage);
        }
       
        private IBus GetRabbitConnection()
        {
            return RabbitHutch.CreateBus("host=localhost");
        } 

        private void ReceiveMessage(File data)
        {
            NewFileReceived?.Invoke(data);
        }

        public void Dispose()
        {
            _bus.Dispose();
        }
    }
}