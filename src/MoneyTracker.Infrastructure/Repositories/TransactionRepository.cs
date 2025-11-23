using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace MoneyTracker.Infrastructure.Repositories
{
    public class TransactionRepository(MoneyTrackerDbContext db) : ITransactionRepository
    {
        private readonly MoneyTrackerDbContext _db = db;

        public async Task AddAsync(Transaction transaction)
        {
            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var t = await _db.Transactions.FindAsync(id);
            if (t != null) { _db.Transactions.Remove(t); await _db.SaveChangesAsync(); }
        }

        public async Task<Transaction?> GetByIdAsync(Guid id) => await _db.Transactions.FindAsync(id);

        public async Task<PagedResult<Transaction>> ListAsync(ISpecification<Transaction> spec, int pageNumber, int pageSize)
        {
            var query = _db.Transactions.AsNoTracking().Where(spec.Criteria);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(t => t.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Transaction>(items, pageNumber, pageSize, totalCount);
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            _db.Transactions.Update(transaction);
            await _db.SaveChangesAsync();
        }
    }
}
