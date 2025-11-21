using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Application.Services.Interfaces;

namespace MoneyTracker.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo) => _repo = repo;

        public async Task<BudgetCategory> CreateAsync(string name, decimal monthlyLimit)
        {
            var c = new BudgetCategory(Guid.NewGuid(), name, monthlyLimit);
            await _repo.AddAsync(c);
            return c;
        }

        public async Task DeleteAsync(Guid id) => await _repo.DeleteAsync(id);

        public async Task<BudgetCategory?> GetByIdAsync(Guid id) => await _repo.GetByIdAsync(id);

        public async Task<IEnumerable<BudgetCategory>> ListAsync() => await _repo.ListAsync();

        public async Task UpdateAsync(Guid id, string name, decimal monthlyLimit)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) throw new Exception("Not found");

            c.Update(name, monthlyLimit);
            await _repo.UpdateAsync(c);
        }
    }
}
