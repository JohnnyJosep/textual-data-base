using TextualDatabaseApi.Application.Repository.Base;
using TextualDatabaseApi.Domain;

namespace TextualDatabaseApi.Application.Repository
{
    public interface ITextAttributeRepository : ICreateRepository<TextAttribute>, IReadRepository<TextAttribute>
    {
        
    }
}