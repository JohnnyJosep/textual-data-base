using TextualDatabaseApi.Application.Repository;
using TextualDatabaseApi.Domain.Entities;
using TextualDatabaseApi.Infrastructure.Repository.Base;
using TextualDatabaseApi.Persistence;

namespace TextualDatabaseApi.Infrastructure.Repository
{
    public class AttributeValueRepository : GenericRepository<AttributeValue>, IAttributeValueRepository
    {
        public AttributeValueRepository(TextualDbContext context) : base(context)
        {
        }
    }
}