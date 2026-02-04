using MoneyTracker.Application.DTOs.People;
using MoneyTracker.Domain.Common;

namespace MoneyTracker.Application.Services.Interfaces
{
	public interface IPersonService
	{
        Task<PersonDto> CreateAsync(PersonSaveDto dto);
        Task<PersonDto> UpdateAsync(Guid id, PersonSaveDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<PersonDto?> GetByIdAsync(Guid id);
        Task<PagedResult<PersonDto>> ListAsync(PersonQueryDto dto);
        Task<PersonDto> PatchAsync(Guid id, PersonPatchDto dto);
	}
}