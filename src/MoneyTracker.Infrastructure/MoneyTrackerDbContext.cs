using Microsoft.EntityFrameworkCore;
using MoneyTracker.Domain.Entities;

namespace MoneyTracker.Infrastructure
{
    public class MoneyTrackerDbContext : DbContext
    {
        public MoneyTrackerDbContext(DbContextOptions<MoneyTrackerDbContext> options) : base(options) { }

        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<BudgetCategory> BudgetCategories => Set<BudgetCategory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Amount).HasColumnType("numeric(18,2)");
                b.Property(x => x.Type).IsRequired();
                b.Property(x => x.Category).HasMaxLength(100);
            });

            modelBuilder.Entity<BudgetCategory>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).HasMaxLength(100).IsRequired();
                b.Property(x => x.MonthlyLimit).HasColumnType("numeric(18,2)");
            });
        }
    }
}
