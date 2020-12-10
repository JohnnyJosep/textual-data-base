using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace TextualDatabaseApi.Application.Repository.Base
{
    public interface ICreateRepository<T> where T : class
    {
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task<T> GetOrAddAsync(T entity, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    }
}