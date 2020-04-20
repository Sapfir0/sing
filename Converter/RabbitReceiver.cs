using System;
using System.Text;
using RabbitMQ.Client;
using EasyNetQ;
using EasyNetQ.Producer;

namespace Converter
{
    public class RabbitReceiver
    {
        private IBus _bus;
        public RabbitReceiver()
        {
            _bus = GetRabbitConnection();
            ((IPubSub)_bus).Subscribe<byte[]>("id", msg => Console.WriteLine("а че тут писать"));
        }
        
        private IBus GetRabbitConnection()
        {
            return RabbitHutch.CreateBus("host=localhost");
        }
        
        private void ReceiveMessage()
        {

        }
        
    }
}