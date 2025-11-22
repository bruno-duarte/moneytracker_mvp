using FluentValidation;
using MoneyTracker.Application.DTOs.Transactions;

namespace MoneyTracker.Application.Validators.Transactions
{
    public class TransactionQueryDtoValidator : AbstractValidator<TransactionQueryDto>
    {
        public TransactionQueryDtoValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("PageNumber must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("PageSize must be greater than 0.")
                .LessThanOrEqualTo(200)
                .WithMessage("PageSize must be less than or equal to 200.");

            When(x => x.CategoryId.HasValue, () =>
            {
                RuleFor(x => x.CategoryId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("CategoryId must be a valid non-empty GUID.");
            });

            When(x => x.From.HasValue && x.To.HasValue, () =>
            {
                RuleFor(x => x)
                    .Must(x => x.From!.Value.Date <= x.To!.Value.Date)
                    .WithMessage("The 'From' date must be less than or equal to the 'To' date.");
            });

            RuleFor(x => x.From)
                .GreaterThan(new DateTime(2000, 1, 1))
                .WithMessage("Date 'From' cannot be earlier than the year 2000.")
                .When(x => x.From.HasValue);

            RuleFor(x => x.To)
                .LessThan(DateTime.UtcNow.AddYears(5))
                .WithMessage("Date 'To' cannot be more than 5 years in the future.")
                .When(x => x.To.HasValue);
        }
    }
}
