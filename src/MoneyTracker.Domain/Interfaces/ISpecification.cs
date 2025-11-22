using System.Linq.Expressions;

namespace MoneyTracker.Domain.Interfaces
{
  public interface ISpecification<T>
  {
      Expression<Func<T, bool>> Criteria { get; }
  }
}