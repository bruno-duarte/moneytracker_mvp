using MoneyTracker.Domain.Enums;

namespace MoneyTracker.Application.DTOs.Categories
{
    /// <summary>
    /// Data transfer object that represents a category used in financial transactions.
    /// </summary>
    public class CategoryDto
    {
        /// <summary>
        /// Unique identifier of the category.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the category.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Type of the category (Expense, Income, Both).
        /// </summary>
        public CategoryType Type { get; set; }
    }
}
