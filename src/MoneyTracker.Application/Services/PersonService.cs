using MoneyTracker.Application.DTOs.People;
using MoneyTracker.Application.Mappers.People;
using MoneyTracker.Application.Services.Interfaces;
using MoneyTracker.Domain.Common;
using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Interfaces.Repositories;
using MoneyTracker.Domain.Specifications;

namespace MoneyTracker.Application.Services
{
	public class PersonService(IPersonRepository repo) : IPersonService
    {
        public async Task<PersonDto> CreateAsync(PersonSaveDto dto)
        {
            var p = new Person(Guid.NewGuid(), dto.Name, dto.Age);
            await repo.AddAsync(p);
            return p.ToDto();
        }

        public async Task<bool> DeleteAsync(Guid id) => await repo.DeleteAsync(id);
        public async Task<PersonDto?> GetByIdAsync(Guid id)
        {
            var p = await repo.GetByIdAsync(id);
            return p?.ToDto();
        }

        public async Task<PagedResult<PersonDto>> ListAsync(PersonQueryDto dto)
        {
            var query = dto.ToQuery();
            var spec = new PersonFilterSpecification(query);
            var pagedResult = await repo.ListAsync(spec, query.PageNumber, query.PageSize);

            return new PagedResult<PersonDto>(pagedResult.Items.Select(x => x.ToDto()), pagedResult.PageNumber, pagedResult.PageSize, pagedResult.TotalCount);
        }

        public async Task<PersonDto> UpdateAsync(Guid id, PersonSaveDto dto)
        {
            var p = await repo.GetByIdAsync(id) ?? throw new Exception("Not found");
            p.Update(dto.Name, dto.Age);
            await repo.UpdateAsync(p);

            return p.ToDto();
        }

        public async Task<PersonDto> PatchAsync(Guid id, PersonPatchDto dto)
        {
            var p = await repo.GetByIdAsync(id) 
                ?? throw new KeyNotFoundException("Person not found.");
            
            dto.MapChanges(p);

            await repo.UpdateAsync(p);

            return p.ToDto();
        }
    }
}