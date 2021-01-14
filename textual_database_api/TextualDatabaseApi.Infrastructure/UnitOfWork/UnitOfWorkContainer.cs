using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using TextualDatabaseApi.Application.Interfaces;
using TextualDatabaseApi.Application.Models;
using TextualDatabaseApi.Application.Repository;
using TextualDatabaseApi.Application.UnitOfWork;
using TextualDatabaseApi.Infrastructure.Repository;
using TextualDatabaseApi.Persistence;

namespace TextualDatabaseApi.Infrastructure.UnitOfWork
{
    public class UnitOfWorkContainer : IUnitOfWork
    {
        private readonly ITextualDbContext _context;
        
        public ITextEntryRepository TextEntryRepository { get; }
        public ITextAttributeRepository TextAttributeRepository { get; }
        public IAttributeValueRepository AttributeValueRepository { get; }
        
        public UnitOfWorkContainer(TextualDbContext context)
        {
            _context = context;
            TextEntryRepository = new TextEntryRepository(context);
            TextAttributeRepository = new TextAttributeRepository(context);
            AttributeValueRepository = new AttributeValueRepository(context);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            await _context.SaveChangesAsync(cancellationToken);

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) => 
            await _context.BeginTransactionAsync(cancellationToken);

        public Task CommitTransactionAsync(CancellationToken cancellationToken = default, params ApplicationEvent[] applicationEvents) => 
            _context.CommitTransactionAsync(cancellationToken, applicationEvents);

        public Task RollbackTransactionAsync(CancellationToken cancellationToken = default) => _context.RollbackTransactionAsync(cancellationToken);
    }
}