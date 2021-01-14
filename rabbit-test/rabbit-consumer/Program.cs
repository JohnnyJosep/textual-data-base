using System;
using RabbitMQ.Client;

namespace rabbit_consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            MessageExangeConsumer.Consume(channel, "demo-queue");
        }
    }
}
