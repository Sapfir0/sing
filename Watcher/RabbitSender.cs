using System;
using System.Text;
using test;
using EasyNetQ;
using EasyNetQ.Producer;

namespace Watcher
{
    public class RabbitSender : ISender, IDisposable
    {
        private IBus _bus;
        
        public RabbitSender()
        {
            _bus = RabbitHutch.CreateBus("host=localhost");
        }

        public void send(byte[] data)
        {
            Console.WriteLine("Изображение отправлено");
            _bus.Publish<byte[]>(data);

        }

        public void Dispose()
        {
            _bus.Dispose();
        }
    }
}