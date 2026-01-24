using Microsoft.EntityFrameworkCore;

namespace MoneyTracker.Infrastructure.Persistence.Common
{
	public class BaseDbContext
	    : DbContext, IMoneyTrackerDbContext
    {
        protected BaseDbContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}