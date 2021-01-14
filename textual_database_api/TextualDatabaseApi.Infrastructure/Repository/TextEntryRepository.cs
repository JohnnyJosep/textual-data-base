using TextualDatabaseApi.Application.Repository;
using TextualDatabaseApi.Domain.Entities;
using TextualDatabaseApi.Infrastructure.Repository.Base;
using TextualDatabaseApi.Persistence;

namespace TextualDatabaseApi.Infrastructure.Repository
{
    public class TextEntryRepository : GenericRepository<TextEntry>, ITextEntryRepository
    {
        public TextEntryRepository(TextualDbContext context) : base(context)
        {
        }
    }
}