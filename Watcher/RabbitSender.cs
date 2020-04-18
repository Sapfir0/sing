using System;
using System.Text;
using RabbitMQ.Client;
using test;
using EasyNetQ;

namespace Watcher
{
    public class RabbitSender : ISender
    {
        private IBus _bus;
        
        public RabbitSender(Config config)
        {
            
        }
        private IBus GetRabbitConnection()
        {
            return RabbitHutch.CreateBus("host=localhost");
        }

        public void send(byte[] data)
        {
            _bus.Publish(data);
        }
    }
}