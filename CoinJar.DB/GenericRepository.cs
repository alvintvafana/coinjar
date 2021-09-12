using CoinJar.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoinJar.DB
{

    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly CoinJarContext _context;
        internal DbSet<TEntity> _dbSet;
        public GenericRepository(CoinJarContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
        public virtual async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> where = null)
        {
            IQueryable<TEntity> query = _dbSet;
            query = where != null ? query.Where(where) : query;
            return await query.FirstOrDefaultAsync();
        }
        public virtual async Task InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
