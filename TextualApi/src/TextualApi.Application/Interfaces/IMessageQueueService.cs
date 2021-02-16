using System.Threading.Tasks;

namespace TextualApi.Application.Interfaces
{
    public interface IMessageQueueService
    {
        Task EnqueueText(string id, string text);
    }
}