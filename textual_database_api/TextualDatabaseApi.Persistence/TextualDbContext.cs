using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TextualDatabaseApi.Application.Interfaces;
using TextualDatabaseApi.Application.Models;
using TextualDatabaseApi.Domain.Base;
using TextualDatabaseApi.Domain.Entities;

namespace TextualDatabaseApi.Persistence
{
    public class TextualDbContext : DbContext, ITextualDbContext
    {
        private readonly IDomainEventService _domainEventService;
        private readonly IApplicationEventService _applicationEventService;
        
        public DbSet<TextEntry> TextEntries { get; set; }
        public DbSet<TextAttribute> TextAttributes { get; set; }
        public DbSet<AttributeValue> AttributeValues { get; set; }

        public TextualDbContext(DbContextOptions options, IDomainEventService domainEventService, IApplicationEventService applicationEventService) : base(options)
        {
            _domainEventService = domainEventService;
            _applicationEventService = applicationEventService;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            await DispatchDomainEvents(cancellationToken);
            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) =>
            await Database.BeginTransactionAsync(cancellationToken);


        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default, params ApplicationEvent[] applicationEvents)
        {
            await Database.CommitTransactionAsync(cancellationToken);
            foreach (var applicationEvent in applicationEvents)
            {
                await _applicationEventService.Publish(applicationEvent, cancellationToken);
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default) => 
            await Database.RollbackTransactionAsync(cancellationToken);
        
        private async Task DispatchDomainEvents(CancellationToken cancellationToken = default)
        {
            while (true)
            {
                var domainEventEntity = ChangeTracker
                    .Entries<IHasDomainEvent>()
                    .Select(x => x.Entity.DomainEvents)
                    .SelectMany(x => x)
                    .FirstOrDefault(domainEvent => !domainEvent.IsPublished);
                
                if (domainEventEntity == null) break;

                domainEventEntity.IsPublished = true;
                await _domainEventService.Publish(domainEventEntity, cancellationToken);
            }
        }
    }
}