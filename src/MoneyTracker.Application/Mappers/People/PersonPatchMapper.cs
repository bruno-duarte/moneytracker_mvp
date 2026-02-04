using MoneyTracker.Application.DTOs.People;
using MoneyTracker.Domain.Entities;

namespace MoneyTracker.Application.Mappers.People
{
	public static class PersonPatchMapper
	{
        public static void MapChanges(this PersonPatchDto dto, Person p)
        {
            p.Patch(dto.Name, dto.Age);
        }
	}
}