using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using TextualDatabaseApi.Application.Interfaces;
using TextualDatabaseApi.Application.Repository;

namespace TextualDatabaseApi.Application.UnitOfWork
{
    public interface IUnitOfWork
    {
        ITextualDbContext Context { get; }
        ITextEntryRepository TextEntryRepository { get; }
        ITextAttributeRepository TextAttributeRepository { get; }
        IAttributeValueRepository AttributeValueRepository { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<IDbContextTransaction> BeginTransactionAsync();
        void CommitTransaction();
        void RollbackTransaction();
    }
}