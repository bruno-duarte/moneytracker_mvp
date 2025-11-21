using System;

namespace MoneyTracker.Domain.Entities
{
    public class BudgetCategory
    {
        public Guid Id { get; private set; }
        public decimal MonthlyLimit { get; private set; }

        private BudgetCategory() { }

        public BudgetCategory(Guid id, decimal monthlyLimit)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            if (monthlyLimit < 0) throw new ArgumentException("Monthly limit must be >= 0", nameof(monthlyLimit));
            MonthlyLimit = monthlyLimit;
        }
    
        public void Update(decimal monthlyLimit)
        {
            if (monthlyLimit < 0) throw new ArgumentException("Monthly limit must be >= 0", nameof(monthlyLimit));
            MonthlyLimit = monthlyLimit;
        }
    }
}
