using Microsoft.EntityFrameworkCore;
using MoneyTracker.Domain.Common;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Interfaces;
using MoneyTracker.Domain.Interfaces.Repositories;

using MoneyTracker.Infrastructure.Persistence.Common;
using MoneyTracker.Infrastructure.Repositories.Common;

namespace MoneyTracker.Infrastructure.Repositories.EfCore
{
	public class PersonRepository(IMoneyTrackerDbContext db) : BaseRepository<Person>(db), IPersonRepository
	{
        public async Task<PagedResult<Person>> ListAsync(ISpecification<Person> spec, int pageNumber, int pageSize)
        {
            var query = _set.AsNoTracking().Where(spec.Criteria);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(t => t.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Person>(items, pageNumber, pageSize, totalCount);
        }
	}
}