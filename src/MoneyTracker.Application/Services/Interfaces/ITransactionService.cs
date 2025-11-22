using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Enums;
using MoneyTracker.Domain.Common;
using MoneyTracker.Application.DTOs.Transactions;

namespace MoneyTracker.Application.Services.Interfaces
{
  public interface ITransactionService
  {
    Task<Transaction> CreateAsync(decimal amount, TransactionType type, Guid categoryId, DateTime date, string? desc);
    Task<Transaction> UpdateAsync(Guid id, TransactionSaveDto dto);
    Task<Transaction> PatchAsync(Guid id, TransactionPatchDto dto);
    Task DeleteAsync(Guid id);
    Task<Transaction?> GetByIdAsync(Guid id);
    Task<PagedResult<Transaction>> ListAsync(TransactionQueryDto dto);
  }
}