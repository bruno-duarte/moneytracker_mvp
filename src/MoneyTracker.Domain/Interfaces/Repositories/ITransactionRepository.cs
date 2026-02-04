using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Common;

namespace MoneyTracker.Domain.Interfaces.Repositories
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task<bool> DeleteAsync(Guid id);
        Task<Transaction?> GetByIdAsync(Guid id);
        Task<PagedResult<Transaction>> ListAsync(ISpecification<Transaction> spec, int pageNumber, int pageSize);
        Task<IQueryable<IGrouping<PersonGroup, Transaction>>> GetTotalsByPersonAsync();
    }
}
