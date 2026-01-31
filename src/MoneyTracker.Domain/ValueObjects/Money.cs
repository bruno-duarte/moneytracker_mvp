namespace MoneyTracker.Domain.ValueObjects
{
    public readonly record struct Money
    {
        public decimal Value { get; }

        private Money(decimal value)
        {
            Value = value;
        }

        public static Money Create(decimal value)
        {
            if (value == 0)
                throw new ArgumentException("Amount must be different from zero", nameof(value));

            return new Money(value);
        }

        public override string ToString() => Value.ToString("F2");
    }
}
