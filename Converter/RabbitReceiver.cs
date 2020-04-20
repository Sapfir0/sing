using System;
using System.Text;
using EasyNetQ;

namespace Converter
{
    public class RabbitReceiver : IDisposable
    {
        private IBus _bus;
        public RabbitReceiver()
        {
            _bus = GetRabbitConnection();
            _bus.Subscribe<byte[]>("id", ReceiveMessage);
        }
       
        private IBus GetRabbitConnection()
        {
            return RabbitHutch.CreateBus("host=localhost");
        }

        public delegate void NewMessageHandler(byte[] data);
        public event NewMessageHandler NewMessage;
        
        private void ReceiveMessage(byte[] data)
        {
            NewMessage?.Invoke(data);
        }

        public void Dispose()
        {
            _bus.Dispose();
        }
    }
}