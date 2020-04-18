using System.Text;
using RabbitMQ.Client;

namespace test
{
    public class RabbitSender : Sender
    {
        private static IConnection GetRabbitConnection()
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/",
                HostName = "localhost"
            };
            IConnection conn = factory.CreateConnection();
            return conn;
        }
        private static IModel GetRabbitChannel(string exchangeName, string queueName, string routingKey)
        {
            var conn = GetRabbitConnection();
            var model = conn.CreateModel();
            model.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            model.QueueDeclare(queueName, false, false, false, null);
            model.QueueBind(queueName, exchangeName, routingKey, null);
            return model;
        }
        
        public static void SendMessage(string exchangeName="test", string queueName="test", string routingKey="test")
        {
            IModel model = GetRabbitChannel(exchangeName, queueName, routingKey);
            byte[] messageBodyBytes = Encoding.UTF8.GetBytes("Hello, world!");
            model.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);
        }
    }
}