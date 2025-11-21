using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MoneyTracker.Infrastructure.Repositories
{
    public class CategoryRepository(MoneyTrackerDbContext db) : ICategoryRepository
    {
        private readonly MoneyTrackerDbContext _db = db;

        public async Task AddAsync(Category category)
        {
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var e = await _db.Categories.FindAsync(id);
            if (e != null) { _db.Categories.Remove(e); await _db.SaveChangesAsync(); }
        }

        public async Task<Category?> GetByIdAsync(Guid id) => await _db.Categories.FindAsync(id);
        public async Task<IEnumerable<Category>> ListAsync() => await _db.Categories.ToListAsync();

        public async Task UpdateAsync(Category category)
        {
            _db.Categories.Update(category);
            await _db.SaveChangesAsync();
        }
    }
}
