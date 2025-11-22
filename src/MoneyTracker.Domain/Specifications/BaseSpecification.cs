using System.Linq.Expressions;
using MoneyTracker.Domain.Interfaces;

namespace MoneyTracker.Domain.Specifications
{
  public abstract class BaseSpecification<T> : ISpecification<T>
  {
      private Expression<Func<T, bool>>? _criteria;

      Expression<Func<T, bool>> ISpecification<T>.Criteria => _criteria ?? (x => true);

      protected void AddCriteria(Expression<Func<T, bool>> expression)
      {
          if (_criteria == null)
          {
              _criteria = expression;
              return;
          }

          var parameter = Expression.Parameter(typeof(T));
          var leftInvoke = Expression.Invoke(_criteria, parameter);
          var rightInvoke = Expression.Invoke(expression, parameter);
          var body = Expression.AndAlso(leftInvoke, rightInvoke);
          _criteria = Expression.Lambda<Func<T, bool>>(body, parameter);
      }
  }
}