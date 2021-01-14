using System.Threading;
using System.Threading.Tasks;
using TextualDatabaseApi.Application.Models;

namespace TextualDatabaseApi.Application.Interfaces
{
    public interface IApplicationEventService
    {
        Task Publish(ApplicationEvent applicationEvent, CancellationToken cancellationToken = default);
    }
}