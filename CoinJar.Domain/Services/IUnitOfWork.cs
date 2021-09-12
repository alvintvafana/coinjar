using System;
using System.Threading.Tasks;

namespace CoinJar.Domain.Services
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveAsync();
    }
}
