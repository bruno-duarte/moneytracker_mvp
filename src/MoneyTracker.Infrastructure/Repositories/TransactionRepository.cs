using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyTracker.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly MoneyTrackerDbContext _db;

        public TransactionRepository(MoneyTrackerDbContext db) => _db = db;

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

        public async Task<IEnumerable<Transaction>> ListAsync() => await _db.Transactions.ToListAsync();

        public async Task<IEnumerable<Transaction>> ListByMonthAsync(int year, int month)
        {
            return await _db.Transactions
                .AsNoTracking()
                .ToListAsync()
                .ContinueWith(t => t.Result.FindAll(x => x.Date.Year == year && x.Date.Month == month));
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            _db.Transactions.Update(transaction);
            await _db.SaveChangesAsync();
        }
    }
}
