using System.Threading;
using System.Threading.Tasks;

namespace TextualDatabaseApi.Application.Interfaces
{
    public interface IScopedProcessingService
    {
        Task DoWork(CancellationToken stoppingToken);
    }
}