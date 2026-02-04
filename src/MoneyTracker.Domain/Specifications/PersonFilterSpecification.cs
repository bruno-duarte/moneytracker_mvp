using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Queries;

namespace MoneyTracker.Domain.Specifications
{
	public class PersonFilterSpecification : BaseSpecification<Person>
	{
        public PersonFilterSpecification(PersonQuery query)
        {
            if (!string.IsNullOrEmpty(query.Name))
                AddCriteria(p => p.Name.Contains(query.Name));

            if (query.Age.HasValue)
                AddCriteria(p => p.Age == query.Age.Value);
        }
	}
}