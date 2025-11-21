using MoneyTracker.Domain.Entities;

namespace MoneyTracker.Application.Services.Interfaces
{
  public interface ICategoryService
  {
    Task<Category> CreateAsync(string name);
    Task UpdateAsync(Guid id, string name);
    Task DeleteAsync(Guid id);
    Task<Category?> GetByIdAsync(Guid id);
    Task<IEnumerable<Category>> ListAsync();
  }
}