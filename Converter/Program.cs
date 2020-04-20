using System;
using System.Drawing;
using System.Net.Mime;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Converter
{

    public interface IByteConverter
    {
        byte[] Convert(byte[] data)
        {
            throw new NotImplementedException();
        }
    } 
    
    class Program
    {
       
        static void Main(string[] args)
        {
            using var receiver = new RabbitReceiver();
            receiver.NewMessage += msg => Console.WriteLine("а че тут писать");
            while (Console.Read() != 'q') ;
            Console.WriteLine("Hello World!");
        }
    }
}