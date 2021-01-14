using TextualDatabaseApi.Application.Interfaces;
using TextualDatabaseApi.Application.Repository;
using TextualDatabaseApi.Domain.Entities;
using TextualDatabaseApi.Infrastructure.Repository.Base;
using TextualDatabaseApi.Persistence;

namespace TextualDatabaseApi.Infrastructure.Repository
{
    public class TextAttributeRepository : GenericRepository<TextAttribute>, ITextAttributeRepository
    {
        public TextAttributeRepository(TextualDbContext context) : base(context)
        {
        }
    }
}