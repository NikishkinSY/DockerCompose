using RabbitMQ.Client;
using System;
using System.Threading.Tasks;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://root:root@rabbitmq:5672");
            var exchangeName = "direct.exchange";
            var queueName = "queue";

            using (var conn = factory.CreateConnection())
            using (var channel = conn.CreateModel())
            {
                channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
                channel.QueueDeclare(queueName, true, false, false);

                var counter = 0;
                do
                {
                    var msg = $"counter: {counter++}";
                    var messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(msg);
                    channel.BasicPublish(string.Empty, queueName, body: messageBodyBytes);
                    Console.WriteLine(msg);
                    Task.Delay(1000).Wait();
                }
                while (true);
                //while (Console.ReadKey().Key != ConsoleKey.Escape);
            }
        }
    }
}
