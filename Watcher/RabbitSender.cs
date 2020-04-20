using System;
using System.Text;
using RabbitMQ.Client;
using test;
using EasyNetQ;
using EasyNetQ.Producer;

namespace Watcher
{
    public class RabbitSender : ISender
    {
        private IBus _bus;
        
        public RabbitSender()
        {
            _bus = GetRabbitConnection();
        }
        private IBus GetRabbitConnection()
        {
            return RabbitHutch.CreateBus("host=localhost");
        }

        public void send(byte[] data)
        {
            ((IPubSub) _bus).Publish(data);
            Console.WriteLine("Изображение отправлено");
        }
    }
}