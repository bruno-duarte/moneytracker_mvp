using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Interfaces.Repositories;
using MoneyTracker.Application.Services.Interfaces;
using MoneyTracker.Application.Mappers;
using MoneyTracker.Application.DTOs.Categories;
using MoneyTracker.Domain.Common;
using MoneyTracker.Domain.Specifications;

namespace MoneyTracker.Application.Services
{
    public class CategoryService(ICategoryRepository repo) : ICategoryService
    {
        private readonly ICategoryRepository _repo = repo;

        public async Task<CategoryDto> CreateAsync(string name)
        {
            var c = new Category(Guid.NewGuid(), name);
            await _repo.AddAsync(c);
            return c.ToDto();
        }

        public async Task<bool> DeleteAsync(Guid id) => await _repo.DeleteAsync(id);

        public async Task<CategoryDto?> GetByIdAsync(Guid id)
        {
            var c = await _repo.GetByIdAsync(id);
            return c?.ToDto();
        }

        public async Task<PagedResult<CategoryDto>> ListAsync(CategoryQueryDto dto)
        {
            var query = dto.ToQuery();
            var spec = new CategoryFilterSpecification(query);
            var pagedResult = await _repo.ListAsync(spec, query.PageNumber, query.PageSize);

            return new PagedResult<CategoryDto>(pagedResult.Items.Select(x => x.ToDto()), pagedResult.PageNumber, pagedResult.PageSize, pagedResult.TotalCount);
        }

        public async Task<CategoryDto> UpdateAsync(Guid id, string name)
        {
            var c = await _repo.GetByIdAsync(id) ?? throw new Exception("Not found");
            c.Update(name);
            await _repo.UpdateAsync(c);

            return c.ToDto();
        }

        public async Task<CategoryDto> PatchAsync(Guid id, CategoryPatchDto dto)
        {
            var c = await _repo.GetByIdAsync(id) 
                ?? throw new KeyNotFoundException("Category not found.");
            
            dto.MapChanges(c);

            await _repo.UpdateAsync(c);

            return c.ToDto();
        }
    }
}
