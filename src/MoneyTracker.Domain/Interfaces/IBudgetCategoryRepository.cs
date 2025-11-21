using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyTracker.Domain.Entities;

namespace MoneyTracker.Domain.Interfaces
{
    public interface IBudgetCategoryRepository
    {
        Task AddAsync(BudgetCategory category);
        Task UpdateAsync(BudgetCategory category);
        Task DeleteAsync(Guid id);
        Task<BudgetCategory?> GetByIdAsync(Guid id);
        Task<IEnumerable<BudgetCategory>> ListAsync();
    }
}
