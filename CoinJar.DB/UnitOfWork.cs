using CoinJar.Domain.Services;
using System;
using System.Threading.Tasks;

namespace CoinJar.DB
{
    public class UnitOfWork : IUnitOfWork
        {
            private readonly CoinJarContext _context;

            public UnitOfWork(CoinJarContext context)
            {
                _context = context;
            }

            public async Task SaveAsync()
            {
                await _context.SaveChangesAsync();
            }
            private bool disposed = false;
            protected virtual void Dispose(bool disposing)
            {
                if (!this.disposed)
                {
                    if (disposing)
                    {
                        _context.Dispose();
                    }
                }
                this.disposed = true;
            }
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }
    }
