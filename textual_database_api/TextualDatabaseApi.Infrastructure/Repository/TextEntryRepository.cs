using TextualDatabaseApi.Application.Interfaces;
using TextualDatabaseApi.Application.Repository;
using TextualDatabaseApi.Domain;
using TextualDatabaseApi.Infrastructure.Repository.Base;

namespace TextualDatabaseApi.Infrastructure.Repository
{
    public class TextEntryRepository : GenericRepository<TextEntry>, ITextEntryRepository
    {
        public TextEntryRepository(ITextualDbContext context) : base (context)
        {
        }
    }
}