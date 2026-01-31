using MoneyTracker.Domain.Entities;
using MoneyTracker.Domain.Enums;
using MoneyTracker.Domain.Interfaces.Repositories;
using MoneyTracker.Domain.Common;
using MoneyTracker.Application.Services.Interfaces;
using MoneyTracker.Application.DTOs.Transactions;
using MoneyTracker.Application.Mappers;
using MoneyTracker.Domain.Specifications;
using MoneyTracker.Application.Exceptions;
using MoneyTracker.Messaging.Abstractions;
using MoneyTracker.Messaging.Abstractions.Models;
using MoneyTracker.Application.Messaging.Topics;
using MoneyTracker.Domain.Events;

namespace MoneyTracker.Application.Services
{
    public class TransactionService(
        ITransactionRepository repo, 
        ICategoryRepository categoryRepo, 
        IMessageProducer producer) : ITransactionService
    {
        public async Task<TransactionDto> CreateAsync(decimal amount, TransactionType type, Guid categoryId, DateTime date, string? desc)
        {
            var category = await categoryRepo.GetByIdAsync(categoryId) 
                ?? throw new InvalidReferenceException("Category", categoryId);

            var t = Transaction.Create(Guid.NewGuid(), amount, type, categoryId, date, desc);
            await repo.AddAsync(t);
            // publish event
            await producer.PublishAsync(
                TransactionTopics.Created, 
                new MessageEnvelope
                {
                    MessageType = nameof(TransactionCreatedEvent),
                    OccurredAt = DateTime.UtcNow,
                    Payload = t.ToEvent(),
                });
            return t.ToDto();
        }

        public async Task<bool> DeleteAsync(Guid id) => await repo.DeleteAsync(id);

        public async Task<TransactionDto?> GetByIdAsync(Guid id)
        {
            var t = await repo.GetByIdAsync(id);
            return t?.ToDto();
        }

        public async Task<PagedResult<TransactionDto>> ListAsync(TransactionQueryDto dto)
        {
            var query = dto.ToQuery();
            var spec = new TransactionFilterSpecification(query);
            var pagedResult = await repo.ListAsync(spec, query.PageNumber, query.PageSize);

            return new PagedResult<TransactionDto>(pagedResult.Items.Select(x => x.ToDto()), pagedResult.PageNumber, pagedResult.PageSize, pagedResult.TotalCount);
        }

        public async Task<TransactionDto> UpdateAsync(Guid id, TransactionSaveDto dto)
        {
            var t = await repo.GetByIdAsync(id)
                ?? throw new EntityNotFoundException("Transaction", id);

            t.Update(dto.Amount, dto.Type, dto.CategoryId, dto.Date, dto.Description);
            await repo.UpdateAsync(t);

            return t.ToDto();
        }

        public async Task<TransactionDto> PatchAsync(Guid id, TransactionPatchDto dto)
        {
            var t = await repo.GetByIdAsync(id)
                ?? throw new EntityNotFoundException("Transaction", id);

            dto.MapChanges(t);

            await repo.UpdateAsync(t);

            return t.ToDto();
        }

		public Task ProcessCreatedTransactionEventAsync(TransactionCreatedEvent ev, CancellationToken cancellationToken)
		{
            // implement event processing logic here
			return Task.CompletedTask;
		}
	}
}
