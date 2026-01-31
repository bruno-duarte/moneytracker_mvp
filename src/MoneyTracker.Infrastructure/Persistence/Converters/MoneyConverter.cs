using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MoneyTracker.Domain.ValueObjects;

namespace MoneyTracker.Infrastructure.Persistence.Converters
{
	public class MoneyConverter : ValueConverter<Money, decimal>
    {
        public MoneyConverter()
            : base(
                money => money.Value,
                value => Money.Create(value))
        {
        }
    }
}