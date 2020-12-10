using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using TextualDatabaseApi.Application.Interfaces;
using TextualDatabaseApi.Application.Repository;
using TextualDatabaseApi.Application.UnitOfWork;
using TextualDatabaseApi.Infrastructure.Repository;
using TextualDatabaseApi.Persistence;

namespace TextualDatabaseApi.Infrastructure.UnitOfWork
{
    public class UnitOfWorkContainer : IUnitOfWork
    {
        private readonly TextualDbContext _context;

        public ITextualDbContext Context => _context;
        
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

        public async Task<IDbContextTransaction> BeginTransactionAsync() => 
            await _context.Database.BeginTransactionAsync();

        public void CommitTransaction() => _context.Database.CommitTransaction();

        public void RollbackTransaction() => _context.Database.RollbackTransaction();
    }
}