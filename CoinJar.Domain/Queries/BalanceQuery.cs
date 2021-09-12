using CoinJar.Domain.Entities;
using CoinJar.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinJar.Domain.Queries
{
   public class BalanceQuery : IQuery<decimal>
    {
        public string UserName { get; set; }
    }
    public class BalanceQueryHandler : IQueryHandler<BalanceQuery, decimal>
    {
        private readonly IRepository<Jar> _repository;
        public BalanceQueryHandler(IRepository<Jar> repository)
        {
            _repository = repository;
        }
        public async Task<decimal> HandleAsync(BalanceQuery query)
        {
            var jar = await _repository.GetOneAsync(x => x.UserName == query.UserName);
            return jar.GetTotalAmount();
        }
    }
}
