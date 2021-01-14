using TextualDatabaseApi.Application.Repository.Base;
using TextualDatabaseApi.Domain.Entities;

namespace TextualDatabaseApi.Application.Repository
{
    public interface ITextAttributeRepository : ICreateRepository<TextAttribute>, IReadRepository<TextAttribute>
    {
        
    }
}