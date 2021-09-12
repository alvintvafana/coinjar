using CoinJar.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoinJar.DB
{
    public class CoinJarContext : DbContext
    {
        public CoinJarContext(DbContextOptions<CoinJarContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Jar> Jars { get; set; }
    }
}
