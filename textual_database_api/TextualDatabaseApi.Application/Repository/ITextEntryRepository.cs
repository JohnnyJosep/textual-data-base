using TextualDatabaseApi.Application.Repository.Base;
using TextualDatabaseApi.Domain;

namespace TextualDatabaseApi.Application.Repository
{
    public interface ITextEntryRepository : ICreateRepository<TextEntry>, IReadRepository<TextEntry>
    {
        
    }
}