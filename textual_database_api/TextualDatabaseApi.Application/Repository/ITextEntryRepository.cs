using TextualDatabaseApi.Application.Repository.Base;
using TextualDatabaseApi.Domain.Entities;

namespace TextualDatabaseApi.Application.Repository
{
    public interface ITextEntryRepository : ICreateRepository<TextEntry>, IReadRepository<TextEntry>, IUpdateRepository<TextEntry>
    {
        
    }
}