using Microsoft.EntityFrameworkCore;

namespace MoneyTracker.Infrastructure.Persistence.Common
{
	public interface IMoneyTrackerDbContext : IUnitOfWork
	{
		DbSet<TEntity> Set<TEntity>() where TEntity : class;
	}
}