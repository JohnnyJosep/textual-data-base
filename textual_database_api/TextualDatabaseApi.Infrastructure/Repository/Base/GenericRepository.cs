using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TextualDatabaseApi.Application.Interfaces;
using TextualDatabaseApi.Persistence;

namespace TextualDatabaseApi.Infrastructure.Repository.Base
{
    public abstract class GenericRepository<T> where T : class
    {
        private readonly TextualDbContext _context;

        protected GenericRepository(TextualDbContext context)
        {
            _context = context;
        }
        
        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<T>().AddAsync(entity, cancellationToken);
        }
        
        public virtual async Task<T> GetOrAddAsync(T entity, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var t = await _context.Set<T>().FirstOrDefaultAsync(predicate, cancellationToken);
            if (t != null)
            {
                return t;
            }
            
            await AddAsync(entity, cancellationToken);
            return entity;
        }
        
        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<T> FindAsync(CancellationToken cancellationToken = default, params object[] keys)
        {
            return await _context.Set<T>().FindAsync(keys, cancellationToken);
        }

        public virtual void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}