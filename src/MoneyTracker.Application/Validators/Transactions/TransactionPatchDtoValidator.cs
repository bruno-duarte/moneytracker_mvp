using FluentValidation;
using MoneyTracker.Application.DTOs.Transactions;

namespace MoneyTracker.Application.Validators.Transactions
{
    public class TransactionPatchDtoValidator : AbstractValidator<TransactionPatchDto>
    {
        public TransactionPatchDtoValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .When(x => x.Amount.HasValue)
                .WithMessage("Amount must be greater than zero.");

            RuleFor(x => x.Type)
                .IsInEnum()
                .When(x => x.Type.HasValue)
                .WithMessage("Invalid transaction type.");

            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .When(x => x.CategoryId.HasValue)
                .WithMessage("CategoryId cannot be empty when provided.");

            RuleFor(x => x.Description)
                .MaximumLength(250)
                .When(x => x.Description != null)
                .WithMessage("Description cannot exceed 500 characters.");
        }
    }
}
