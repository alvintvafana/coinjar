using CoinJar.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Jar.UnitTests
{
    public class MockBookRepository<TEntity> : IRepository<TEntity> where TEntity : CoinJar.Domain.Entities.Jar
    {
        public Dictionary<Guid, TEntity> Table = new Dictionary<Guid, TEntity>();
        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> where = null)
        {
            if (where == null)
                return await Task.Run(() => Table.Values.ToList());
            return await Task.Run(() => Table.Values.Where(where.Compile()));
        }

        public async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> where = null)
        {
            return await Task.Run(() => Table.Values.FirstOrDefault(where.Compile()));
        }

        public async Task InsertAsync(TEntity entity)
        {
            await Task.Run(() => Table.Add(entity.Id, entity));
        }

        public void Update(TEntity entity)
        {
            Table[entity.Id] = entity;
        }
    }
}
