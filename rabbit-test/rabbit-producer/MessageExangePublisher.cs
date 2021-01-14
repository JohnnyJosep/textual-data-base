using RabbitMQ.Client;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Text.Json;

namespace rabbit_producer
{
    public static class MessageExangePublisher 
    {
        public static void Publish(IModel channel, string queue) 
        {
            channel.ExchangeDeclare(
                queue, 
                ExchangeType.Fanout, 
                arguments: new Dictionary<string, object> {
                    { "x-message-ttl", 30000 }
                });

            var count = 0;

            while (true)
            {
                var message = new 
                { 
                    Name = "Producer", 
                    Message = $"Hello! Count: {count}" 
                };
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                var properties = channel.CreateBasicProperties();
                properties.Headers = new Dictionary<string, object> 
                { 
                    { "account", "update" } 
                };

                channel.BasicPublish(queue, "account.new", properties, body);
                count++;
                Thread.Sleep(500);
            }
        }
    }
}