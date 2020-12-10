using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TextualDatabaseApi.Application.Interfaces;

namespace TextualDatabaseApi.Infrastructure.Repository.Base
{
    public abstract class GenericRepository<T> where T : class
    {
        private readonly ITextualDbContext _context;

        protected GenericRepository(ITextualDbContext context)
        {
            _context = context;
        }
        
        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<T>().AddAsync(entity, cancellationToken);
        }
        
        public virtual async Task<T> GetOrAddAsync(T entity, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var t = await _context.Set<T>().FirstAsync(predicate, cancellationToken);
            if (t != null)
            {
                return t;
            }
            
            await _context.Set<T>().AddAsync(entity, cancellationToken);
            return entity;
        }
        
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
    }
}