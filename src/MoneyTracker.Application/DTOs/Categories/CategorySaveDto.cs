using MoneyTracker.Domain.Enums;

namespace MoneyTracker.Application.DTOs.Categories
{
  /// <summary>
  /// DTO used to create or update a category.
  /// </summary>
  public record CategorySaveDto(string Name, CategoryType Type);
}