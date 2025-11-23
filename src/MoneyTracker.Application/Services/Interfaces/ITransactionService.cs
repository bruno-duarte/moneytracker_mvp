using MoneyTracker.Domain.Enums;
using MoneyTracker.Domain.Common;
using MoneyTracker.Application.DTOs.Transactions;

namespace MoneyTracker.Application.Services.Interfaces
{
  public interface ITransactionService
  {
    Task<TransactionDto> CreateAsync(decimal amount, TransactionType type, Guid categoryId, DateTime date, string? desc);
    Task<TransactionDto> UpdateAsync(Guid id, TransactionSaveDto dto);
    Task<TransactionDto> PatchAsync(Guid id, TransactionPatchDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<TransactionDto?> GetByIdAsync(Guid id);
    Task<PagedResult<TransactionDto>> ListAsync(TransactionQueryDto dto);
  }
}