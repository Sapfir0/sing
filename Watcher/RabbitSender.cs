using System;
using System.Text;
using RabbitMQ.Client;
using test;

namespace Watcher
{
    public class RabbitSender : ISender
    {
        private IModel _model;
        private string _routingKey, _exchangeName, _queueName;
        public RabbitSender(Config config)
        {
            
            _routingKey = config._routingKey;
            _exchangeName = config._exchangeName;
            _queueName = config._queueName;
            _model = GetRabbitChannel(_exchangeName, _queueName, _routingKey);
            
        }
        private IConnection GetRabbitConnection()
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
        private IModel GetRabbitChannel(string exchangeName, string queueName, string routingKey)
        {
            var conn = GetRabbitConnection();
            var model = conn.CreateModel();
            model.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            model.QueueDeclare(queueName, false, false, false, null);
            model.QueueBind(queueName, exchangeName, routingKey, null);
            return model;
        }

        public void send(byte[] data)
        {
            _model.BasicPublish(_exchangeName, _routingKey, null, data);
        }
    }
}