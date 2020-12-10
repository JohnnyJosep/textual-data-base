using TextualDatabaseApi.Application.Repository.Base;
using TextualDatabaseApi.Domain;

namespace TextualDatabaseApi.Application.Repository
{
    public interface IAttributeValueRepository : ICreateRepository<AttributeValue>, IReadRepository<AttributeValue>
    {
        
    }
}