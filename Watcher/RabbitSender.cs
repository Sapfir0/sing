using System;
using EasyNetQ;


namespace Watcher
{
    public class RabbitSender : ISender, IDisposable
    {
        private IBus _bus;
        
        public RabbitSender()
        {
            _bus = RabbitHutch.CreateBus("host=localhost");
        }

        public void send(File data)
        {
            Console.WriteLine("Изображение отправлено");
            _bus.Publish<File>(data);
        }

        public void Dispose()
        {
            _bus.Dispose();
        }
    }
}