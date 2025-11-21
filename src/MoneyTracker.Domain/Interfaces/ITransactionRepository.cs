using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyTracker.Domain.Entities;

namespace MoneyTracker.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(Guid id);
        Task<Transaction?> GetByIdAsync(Guid id);
        Task<IEnumerable<Transaction>> ListAsync();
        Task<IEnumerable<Transaction>> ListByMonthAsync(int year, int month);
    }
}
