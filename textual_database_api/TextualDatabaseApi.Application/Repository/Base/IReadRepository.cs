using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TextualDatabaseApi.Application.Repository.Base
{
    public interface IReadRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<T> FindAsync(CancellationToken cancellationToken = default, params object[] keys);
    }
}