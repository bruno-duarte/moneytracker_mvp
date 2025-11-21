using FluentValidation;
using MoneyTracker.Application.DTOs;

namespace MoneyTracker.Api.Application.Validators
{
    public class TransactionCreateDtoValidator : AbstractValidator<TransactionCreateDto>
    {
        public TransactionCreateDtoValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.CategoryId).NotEmpty();
            RuleFor(x => x.Date).NotEmpty();
        }
    }
}
