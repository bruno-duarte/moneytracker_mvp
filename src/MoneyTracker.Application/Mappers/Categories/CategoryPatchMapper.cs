using MoneyTracker.Application.DTOs.Categories;
using MoneyTracker.Domain.Entities;

namespace MoneyTracker.Application.Mappers
{
    public static class CategoryPatchMapper
    {
        public static void MapChanges(this CategoryPatchDto dto, Category c)
        {
            c.Patch(dto.Name, dto.Type);
        }
    }
}
