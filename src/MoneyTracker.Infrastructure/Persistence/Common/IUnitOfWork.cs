namespace MoneyTracker.Infrastructure.Persistence.Common
{
	public interface IUnitOfWork
	{
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}