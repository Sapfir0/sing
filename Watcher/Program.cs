using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Watcher
{

    class Sender
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
    class Program
    {
        /*
        private Sender sender;
        */

        public Program()
        {
        }
        private byte[] Image2Byte()
        {
            throw new NotImplementedException();
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {           
            Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
            Sender.SendMessage();
        }
      
        
      
        private static void HandleDirectory(string path)
        {
            using FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;
            
            watcher.Created += OnChanged;
            
            watcher.EnableRaisingEvents = true;
            
            while(true);
        }


        static void Main(string[] args)
        {
            const string subpath = "\\incoming";
            var path = Directory.GetCurrentDirectory() + subpath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            var filesOnStart = Directory.GetFiles(path);
            var sandedFiles = new List<string>(); // можно сохранять уже обработанные файлы

            Console.WriteLine("Детектим изменения в " + path);

            HandleDirectory(path);

        }
    }
}