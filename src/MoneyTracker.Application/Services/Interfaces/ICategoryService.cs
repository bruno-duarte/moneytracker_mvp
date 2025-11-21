using MoneyTracker.Domain.Entities;

namespace MoneyTracker.Application.Services.Interfaces
{
  public interface ICategoryService
  {
    Task<BudgetCategory> CreateAsync(string name, decimal monthlyLimit);
    Task UpdateAsync(Guid id, string name, decimal monthlyLimit);
    Task DeleteAsync(Guid id);
    Task<BudgetCategory?> GetByIdAsync(Guid id);
    Task<IEnumerable<BudgetCategory>> ListAsync();
  }
}