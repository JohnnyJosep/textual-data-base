using TextualDatabaseApi.Application.Interfaces;
using TextualDatabaseApi.Application.Repository;
using TextualDatabaseApi.Domain;
using TextualDatabaseApi.Infrastructure.Repository.Base;

namespace TextualDatabaseApi.Infrastructure.Repository
{
    public class TextAttributeRepository : GenericRepository<TextAttribute>, ITextAttributeRepository
    {
        public TextAttributeRepository(ITextualDbContext context) : base(context)
        {
        }
    }
}