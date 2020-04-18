using System;
using System.Text;
using RabbitMQ.Client;
using EasyNetQ;
using EasyNetQ.Producer;

namespace Converter
{
    public class RabbitReceiver
    {
        public RabbitReceiver()
        {
            
            var bus = GetRabbitConnection();
            ((IPubSub)bus).Subscribe<byte[]>("test", msg => Console.WriteLine("а че тут писать"));
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