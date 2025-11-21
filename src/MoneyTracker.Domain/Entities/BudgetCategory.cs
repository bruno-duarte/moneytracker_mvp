using System;

namespace MoneyTracker.Domain.Entities
{
    public class BudgetCategory
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public decimal MonthlyLimit { get; private set; }

        private BudgetCategory() { }

        public BudgetCategory(Guid id, string name, decimal monthlyLimit)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required", nameof(name));
            Name = name;
            if (monthlyLimit < 0) throw new ArgumentException("Monthly limit must be >= 0", nameof(monthlyLimit));
            MonthlyLimit = monthlyLimit;
        }

        public void Update(string name, decimal monthlyLimit)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required", nameof(name));
            Name = name;
            if (monthlyLimit < 0) throw new ArgumentException("Monthly limit must be >= 0", nameof(monthlyLimit));
            MonthlyLimit = monthlyLimit;
        }
    }
}
