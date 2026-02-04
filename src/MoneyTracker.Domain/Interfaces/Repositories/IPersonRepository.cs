using MoneyTracker.Domain.Common;
using MoneyTracker.Domain.Entities;

namespace MoneyTracker.Domain.Interfaces.Repositories
{
	public interface IPersonRepository
	{
        Task AddAsync(Person person);
        Task UpdateAsync(Person person);
        Task<bool> DeleteAsync(Guid id);
        Task<Person?> GetByIdAsync(Guid id);
        Task<PagedResult<Person>> ListAsync(ISpecification<Person> spec, int pageNumber, int pageSize);
	}
}