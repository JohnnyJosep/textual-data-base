using System.Text;
using System.Text.Json;

namespace TextualApi.Infrastructure.Services.MessageQueueService
{
    public class TextMessage
    {
        public string Id { get; set; }
        public string Text { get; set; }

        public byte[] ToJsonByteArray()
        {
            var json = JsonSerializer.Serialize(this);
            var bytes = Encoding.UTF8.GetBytes(json);

            return bytes;
        }
    }
}