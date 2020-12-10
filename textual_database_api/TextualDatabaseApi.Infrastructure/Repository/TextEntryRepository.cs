using System.Collections.Generic;
using System.Threading.Tasks;
using TextualDatabaseApi.Application.Interfaces;
using TextualDatabaseApi.Application.Model;
using TextualDatabaseApi.Application.Repository;

namespace TextualDatabaseApi.Infrastructure.Repository
{
    public class TextualDataRepository : ITextualDataRepository
    {
        private ITextualDbContext _context;

        public TextualDataRepository(ITextualDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(TextualData entity)
        {
            throw new System.NotImplementedException();
        }

        public Task AddAsync(IEnumerable<TextualData> entities)
        {
            throw new System.NotImplementedException();
        }
    }
}