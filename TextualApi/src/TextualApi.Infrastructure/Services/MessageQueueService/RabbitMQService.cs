using System.Threading.Tasks;
using RabbitMQ.Client;
using TextualApi.Application.Interfaces;

namespace TextualApi.Infrastructure.Services.MessageQueueService
{
    public class RabbitMQService : IMessageQueueService
    {
        private readonly IModel _channel;

        public RabbitMQService(IModel channel)
        {
            _channel = channel;
        }

        public Task EnqueueText(string id, string text)
        {
            var message = new TextMessage {Id = id, Text = text}.ToJsonByteArray();
            _channel.BasicPublish(exchange: string.Empty,
                routingKey: "tfg-queue",
                basicProperties: null,
                body: message);
            
            return Task.CompletedTask;
        }
    }
}