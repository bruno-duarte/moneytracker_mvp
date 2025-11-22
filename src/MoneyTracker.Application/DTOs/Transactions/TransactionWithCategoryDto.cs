using MoneyTracker.Application.DTOs.Categories;
using MoneyTracker.Domain.Enums;

namespace MoneyTracker.Application.DTOs.Transactions
{
  public class TransactionWithCategoryDto
  {
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public CategoryDto? Category { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
  }
}