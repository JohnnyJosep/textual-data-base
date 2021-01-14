using System.Threading;
using System.Threading.Tasks;
using TextualDatabaseApi.Domain.Base;

namespace TextualDatabaseApi.Application.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent, CancellationToken cancellationToken = default);
    }
}