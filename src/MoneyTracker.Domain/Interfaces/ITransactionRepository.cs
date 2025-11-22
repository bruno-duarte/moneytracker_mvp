using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Common;

namespace MoneyTracker.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(Guid id);
        Task<Transaction?> GetByIdAsync(Guid id);
        Task<PagedResult<Transaction>> ListAsync(ISpecification<Transaction> spec, int pageNumber, int pageSize);
        Task<IEnumerable<Transaction>> ListByMonthAsync(int year, int month);
    }
}
