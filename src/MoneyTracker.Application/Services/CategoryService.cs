using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Application.Services.Interfaces;

namespace MoneyTracker.Application.Services
{
    public class CategoryService(ICategoryRepository repo) : ICategoryService
    {
        private readonly ICategoryRepository _repo = repo;

        public async Task<Category> CreateAsync(string name)
        {
            var c = new Category(Guid.NewGuid(), name);
            await _repo.AddAsync(c);
            return c;
        }

        public async Task DeleteAsync(Guid id) => await _repo.DeleteAsync(id);

        public async Task<Category?> GetByIdAsync(Guid id) => await _repo.GetByIdAsync(id);

        public async Task<IEnumerable<Category>> ListAsync() => await _repo.ListAsync();

        public async Task UpdateAsync(Guid id, string name)
        {
            var c = await _repo.GetByIdAsync(id) ?? throw new Exception("Not found");
            c.Update(name);
            await _repo.UpdateAsync(c);
        }
    }
}
