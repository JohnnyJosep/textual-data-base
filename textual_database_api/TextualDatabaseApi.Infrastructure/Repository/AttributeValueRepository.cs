using TextualDatabaseApi.Application.Interfaces;
using TextualDatabaseApi.Application.Repository;
using TextualDatabaseApi.Domain;
using TextualDatabaseApi.Infrastructure.Repository.Base;

namespace TextualDatabaseApi.Infrastructure.Repository
{
    public class AttributeValueRepository : GenericRepository<AttributeValue>, IAttributeValueRepository
    {
        public AttributeValueRepository(ITextualDbContext context) : base(context)
        {
        }
    }
}