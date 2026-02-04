using MoneyTracker.Application.DTOs.People;
using MoneyTracker.Domain.Queries;

namespace MoneyTracker.Application.Mappers.People
{
	public static class PersonQueryMapper
	{
        public static PersonQuery ToQuery(this PersonQueryDto dto)
		{
			return new PersonQuery
			{
				Name = dto.Name,
				Age = dto.Age
			};
		}
	}
}