using Microsoft.EntityFrameworkCore;
using TextualDatabaseApi.Domain.Entities;

namespace TextualDatabaseApi.Application.Interfaces
{
    public interface ITextualDbContext : IDbContext
    {
        DbSet<TextEntry> TextEntries { get; set; }
        DbSet<TextAttribute> TextAttributes { get; set; }
        DbSet<AttributeValue> AttributeValues { get; set; }
    }
}