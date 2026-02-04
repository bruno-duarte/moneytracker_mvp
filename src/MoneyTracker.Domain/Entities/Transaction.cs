using MoneyTracker.Domain.Enums;
using MoneyTracker.Domain.ValueObjects;

namespace MoneyTracker.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; private set; }
        public Money Amount { get; private set; }
        public TransactionType Type { get; private set; }

        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; }

        public Guid PersonId { get; private set; }
        public Person Person { get; private set; }

        public DateTime Date { get; private set; }
        public string? Description { get; private set; }

        #pragma warning disable CS8618
        private Transaction() { } // EF Core
        #pragma warning restore CS8618

        private Transaction(
            Guid id,
            Money amount,
            TransactionType type,
            Category category,
            Person person,
            DateTime date,
            string? description)
        {
            ValidateBusinessRules(type, category, person);

            Id = id;
            Amount = amount;
            Type = type;
            CategoryId = category.Id;
            Category = category;
            PersonId = person.Id;
            Person = person;
            Date = date;
            Description = description;
        }

        public static Transaction Create(
            decimal amount,
            TransactionType type,
            Category category,
            Person person,
            DateTime date,
            string? description)
        {
            var money = Money.Create(amount);
            
            return new Transaction(Guid.NewGuid(), money, type, category, person, date, description?.Trim());
        }

        public void Update(
            decimal amount,
            TransactionType type,
            Guid categoryId,
            DateTime date,
            string? description)
        {
            if (categoryId == Guid.Empty)
                throw new Exception("CategoryId must be different from empty Guid.");

            Amount = Money.Create(amount);
            Type = type;
            CategoryId = categoryId;
            Date = date;
            Description = description?.Trim();
        }

        public void Patch(
            decimal? amount,
            TransactionType? type,
            Guid? categoryId,
            DateTime? date,
            string? description)
        {
            if (amount.HasValue)
                Amount = Money.Create(amount.Value);

            if (type.HasValue)
                Type = type.Value;

            if (categoryId.HasValue)
            {
                if (categoryId.Value == Guid.Empty)
                    throw new Exception("CategoryId não pode ser vazio.");

                CategoryId = categoryId.Value;
            }

            if (date.HasValue)
                Date = date.Value;

            if (description != null)
                Description = description.Trim();
        }

        private static void ValidateBusinessRules(TransactionType type, Category category, Person person)
        {
            if (person.Age < 18 && type == TransactionType.Income)
                throw new Exception("Minors cannot register income.");

            if (category.Type == CategoryType.Expense && type == TransactionType.Income)
                throw new Exception("Category not allowed for income.");

            if (category.Type == CategoryType.Income && type == TransactionType.Expense)
                throw new Exception("Category not allowed for expense.");
        }
    }
}
