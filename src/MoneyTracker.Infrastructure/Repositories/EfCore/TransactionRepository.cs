using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Domain.Common;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.Domain.Interfaces.Repositories;
using MoneyTracker.Infrastructure.Persistence.Common;
using MoneyTracker.Infrastructure.Repositories.Common;

namespace MoneyTracker.Infrastructure.Repositories.EfCore
{
    public class TransactionRepository(IMoneyTrackerDbContext db) : BaseRepository<Transaction>(db), ITransactionRepository
    {
        public async Task<PagedResult<Transaction>> ListAsync(ISpecification<Transaction> spec, int pageNumber, int pageSize)
        {
            var query = _set.AsNoTracking().Where(spec.Criteria);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(t => t.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Transaction>(items, pageNumber, pageSize, totalCount);
        }
    }
}
