using MoneyTracker.Domain.Enums;

namespace MoneyTracker.Application.DTOs.Categories
{
    /// <summary>
    /// Data transfer object used to partially update an existing category.
    /// Only the properties provided will be modified.
    /// </summary>
    public class CategoryPatchDto
    {
        /// <summary>
        /// New category name.  
        /// If <c>null</c>, the current name will remain unchanged.
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// New category type.  
        /// If <c>null</c>, the current type will remain unchanged.
        /// </summary>
        public CategoryType? Type { get; set; }
    }
}
