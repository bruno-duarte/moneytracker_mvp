using FluentValidation;
using MoneyTracker.Application.DTOs.Transactions;

namespace MoneyTracker.Application.Validators.Transactions
{
    public class TransactionSaveDtoValidator : AbstractValidator<TransactionSaveDto>
    {
        public TransactionSaveDtoValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than zero.");

            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage("Invalid transaction type.");

            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage("CategoryId is required.");

            RuleFor(x => x.Date)
                .NotEmpty()
                .WithMessage("Date is required.");

            RuleFor(x => x.Description)
                .MaximumLength(250)
                .WithMessage("Description cannot exceed 250 characters.");
        }
    }
}
