using Microsoft.EntityFrameworkCore;
using MoneyTracker.Domain.Interfaces.Repositories;
using MoneyTracker.Infrastructure.Persistence.Common;
using System.Linq.Expressions;

namespace MoneyTracker.Infrastructure.Repositories.Common
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly IMoneyTrackerDbContext _context;
        protected readonly DbSet<T> _set;

        public BaseRepository(IMoneyTrackerDbContext context)
        {
            _context = context;
            _set = _context.Set<T>();
        }

        public virtual async Task AddAsync(T entity)
        {
            await _set.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _set.Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _set.FindAsync(id);
            if (entity != null)
            {
                _set.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _set.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> ListAsync()
        {
            return await _set.AsNoTracking().ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _set.AsNoTracking().Where(predicate).ToListAsync();
        }
    }
}
