using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MoneyTracker.Infrastructure.Repositories
{
    public class BudgetCategoryRepository(MoneyTrackerDbContext db) : IBudgetCategoryRepository
    {
        private readonly MoneyTrackerDbContext _db = db;

        public async Task AddAsync(BudgetCategory category)
        {
            _db.BudgetCategories.Add(category);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var e = await _db.BudgetCategories.FindAsync(id);
            if (e != null) { _db.BudgetCategories.Remove(e); await _db.SaveChangesAsync(); }
        }

        public async Task<BudgetCategory?> GetByIdAsync(Guid id) => await _db.BudgetCategories.FindAsync(id);
        public async Task<IEnumerable<BudgetCategory>> ListAsync() => await _db.BudgetCategories.ToListAsync();

        public async Task UpdateAsync(BudgetCategory budgetCategory)
        {
            _db.BudgetCategories.Update(budgetCategory);
            await _db.SaveChangesAsync();
        }
    }
}
