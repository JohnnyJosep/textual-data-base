using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading;
using System.Text;

namespace rabbit_consumer
{
    public static class MessageExangeConsumer 
    {
        public static void Consume(IModel channel, string queue)
        {
            channel.ExchangeDeclare(queue, ExchangeType.Fanout);
            channel.QueueDeclare(queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);


            channel.QueueBind(queue, queue, string.Empty);
            channel.BasicQos(0, 10, false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) => {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);

                Thread.Sleep(3000);
            };

            channel.BasicConsume(queue, true, consumer);
            Console.WriteLine("Consumer started");
            Console.ReadLine();
        }
    }
}