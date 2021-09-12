using CoinJar.Domain.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoinJar.Domain.Services
{
    public interface IRepository<TEntity> 
    {

        Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> where = null);
        Task InsertAsync(TEntity entity);

        void Update(TEntity entity);
    }
}
