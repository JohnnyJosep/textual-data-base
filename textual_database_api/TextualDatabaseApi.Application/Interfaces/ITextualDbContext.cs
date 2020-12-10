using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TextualDatabaseApi.Domain;

namespace TextualDatabaseApi.Application.Interfaces
{
    public interface ITextualDbContext
    {
        DbSet<TextEntry> TextEntries { get; set; }
        DbSet<TextAttribute> TextAttributes { get; set; }
        DbSet<AttributeValue> AttributeValues { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}