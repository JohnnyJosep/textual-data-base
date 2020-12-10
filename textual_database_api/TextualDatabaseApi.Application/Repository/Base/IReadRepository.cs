using System.Collections.Generic;
using System.Threading.Tasks;

namespace TextualDatabaseApi.Application.Repository.Base
{
    public interface IReadRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
    }
}