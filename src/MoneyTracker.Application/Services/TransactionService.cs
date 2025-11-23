using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Enums;
using MoneyTracker.Domain.Interfaces.Repositories;
using MoneyTracker.Domain.Common;
using MoneyTracker.Infrastructure.Kafka;
using MoneyTracker.Application.Services.Interfaces;
using MoneyTracker.Application.DTOs.Transactions;
using MoneyTracker.Application.Mappers;
using MoneyTracker.Domain.Specifications;
using MoneyTracker.Application.Exceptions;

namespace MoneyTracker.Application.Services
{
    public class TransactionService(
        ITransactionRepository repo, 
        ICategoryRepository categoryRepo, 
        ProducerWrapper producer) : ITransactionService
    {
        private readonly ITransactionRepository _repo = repo;
        private readonly ProducerWrapper _producer = producer;
        private readonly ICategoryRepository _categoryRepo = categoryRepo;

        public async Task<TransactionDto> CreateAsync(decimal amount, TransactionType type, Guid categoryId, DateTime date, string? desc)
        {
            var category = await _categoryRepo.GetByIdAsync(categoryId);

            if (category == null)
                throw new InvalidReferenceException("Category", categoryId);
                
            var t = new Transaction(Guid.NewGuid(), amount, type, categoryId, date, desc);
            await _repo.AddAsync(t);
            // publish event
            await _producer.ProduceAsync("transaction.created", t.ToDto());
            return t.ToDto();
        }

        public async Task<bool> DeleteAsync(Guid id) => await _repo.DeleteAsync(id);

        public async Task<TransactionDto?> GetByIdAsync(Guid id)
        {
            var t = await _repo.GetByIdAsync(id);
            return t?.ToDto();
        }

        public async Task<PagedResult<TransactionDto>> ListAsync(TransactionQueryDto dto)
        {
            var query = dto.ToQuery();
            var spec = new TransactionFilterSpecification(query);
            var pagedResult = await _repo.ListAsync(spec, query.PageNumber, query.PageSize);

            return new PagedResult<TransactionDto>(pagedResult.Items.Select(x => x.ToDto()), pagedResult.PageNumber, pagedResult.PageSize, pagedResult.TotalCount);
        }

        public async Task<TransactionDto> UpdateAsync(Guid id, TransactionSaveDto dto)
        {
            var t = await _repo.GetByIdAsync(id)
                ?? throw new EntityNotFoundException("Transaction", id);

            t.Update(dto.Amount, dto.Type, dto.CategoryId, dto.Date, dto.Description);
            await _repo.UpdateAsync(t);

            return t.ToDto();
        }

        public async Task<TransactionDto> PatchAsync(Guid id, TransactionPatchDto dto)
        {
            var t = await _repo.GetByIdAsync(id)
                ?? throw new EntityNotFoundException("Transaction", id);

            dto.MapChanges(t);

            await _repo.UpdateAsync(t);

            return t.ToDto();
        }
    }
}
