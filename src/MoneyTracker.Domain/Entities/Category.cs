using MoneyTracker.Domain.Enums;

namespace MoneyTracker.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public CategoryType Type { get; private set; }

        public ICollection<Transaction> Transactions { get; private set; }

        private Category() { }

        public Category(Guid id, string name, CategoryType type)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required", nameof(name));

            Name = name;
            Type = type;
            Transactions = [];
        }

        public void Update(string name, CategoryType type)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required", nameof(name));

            Name = name;
            Type = type;
        }

        public void Patch(string? name, CategoryType? type)
        {
            if (name != null)
                Name = name;

            if (type.HasValue)
                Type = type.Value;
        }
    }
}
