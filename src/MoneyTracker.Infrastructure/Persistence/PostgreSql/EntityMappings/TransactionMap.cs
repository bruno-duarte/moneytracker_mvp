using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyTracker.Domain.Entities;

namespace MoneyTracker.Infrastructure.Persistence.PostgreSql.EntityMappings
{
    public class TransactionMap : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> b)
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Amount).HasColumnType("numeric(18,2)");
            b.Property(x => x.Type).IsRequired();
            b.Property(x => x.CategoryId).IsRequired();

            b.HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            b.Property(x => x.Date).IsRequired();
        }
    }
}
