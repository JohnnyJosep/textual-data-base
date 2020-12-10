using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TextualDatabaseApi.Application.Interfaces;
using TextualDatabaseApi.Domain;

namespace TextualDatabaseApi.Persistence
{
    public class TextualDbContext : DbContext, ITextualDbContext
    {
        public DbSet<TextEntry> TextEntries { get; set; }
        public DbSet<TextAttribute> TextAttributes { get; set; }
        public DbSet<AttributeValue> AttributeValues { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}