using System;
using System.Drawing;
using System.Net.Mime;
using System.Text;
using RabbitMQ.Client.Events;

namespace Converter
{
    class Program
    {
        /*private Image PutWatermark()
        {
            
        }*/
        
        private void RabbitListener()
        {
            model = GetRabbitChannel(exchangeName, queueName, routingKey);
            var subscription = new Subscription(model, queueName, false);
            while (true)
            {
                BasicDeliverEventArgs basicDeliveryEventArgs = subscription.Next();
                string messageContent = Encoding.UTF8.GetString(basicDeliveryEventArgs.Body);
                messagesTextBox.Invoke((MethodInvoker)delegate { messagesTextBox.Text += messageContent + "\r\n"; });
                subscription.Ack(basicDeliveryEventArgs);
            }
        }
        static void Main(string[] args)
        {
            
            Console.WriteLine("Hello World!");
        }
    }
}