using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using TextualDatabaseApi.Application.Models;
using TextualDatabaseApi.Application.Repository;

namespace TextualDatabaseApi.Application.UnitOfWork
{
    public interface IUnitOfWork
    {
        ITextEntryRepository TextEntryRepository { get; }
        ITextAttributeRepository TextAttributeRepository { get; }
        IAttributeValueRepository AttributeValueRepository { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default, params ApplicationEvent[] applicationEvents);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}