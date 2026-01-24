using Microsoft.EntityFrameworkCore;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Infrastructure.Persistence.Common;

namespace MoneyTracker.Infrastructure
{
    public class PostgreSqlDbContext(DbContextOptions<PostgreSqlDbContext> options) 
        : BaseDbContext(options)
    {
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<Category> Categories => Set<Category>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostgreSqlDbContext).Assembly);
        }
    }
}
