using System;
using Xunit;
using MoneyTracker.Api.Domain.Entities;

namespace MoneyTracker.Tests
{
    public class TransactionDomainTests
    {
        [Fact]
        public void Create_WithValidData_Succeeds()
        {
            var t = new Transaction(Guid.NewGuid(), 100.50m, TransactionType.Expense, "Food", DateTime.UtcNow, "Lunch");
            Assert.Equal(100.50m, t.Amount);
            Assert.Equal(TransactionType.Expense, t.Type);
        }

        [Fact]
        public void Create_WithZeroAmount_Throws()
        {
            Assert.Throws<ArgumentException>(() => new Transaction(Guid.NewGuid(), 0m, TransactionType.Expense, "Food", DateTime.UtcNow, null));
        }
    }
}
