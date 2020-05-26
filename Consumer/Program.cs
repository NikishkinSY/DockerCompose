using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading.Tasks;

namespace Consumer
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

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (ch, ea) =>
                {
                    var str = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());
                    Console.WriteLine(str);
                };
                channel.BasicConsume(queueName, false, consumer);

                while (true)
                {
                    Task.Delay(1000).Wait();
                }
                //while (Console.ReadKey().Key != ConsoleKey.Escape)
                //{
                //}
            }
        }
    }
}
