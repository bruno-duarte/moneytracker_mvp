namespace MoneyTracker.Domain.Entities
{
	public class Person
	{
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public int Age { get; private set; }

        public ICollection<Transaction> Transactions { get; private set; }

        private Person() { }

        public Person(Guid id, string name, int age)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;

            if (string.IsNullOrWhiteSpace(name) || name.Length > 200)
                throw new ArgumentException("Invalid name");

            if (age < 0)
                throw new ArgumentException("Invalid age");

            Name = name;
            Age = age;
            Transactions = [];
        }

        public void Update(string name, int age)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length > 200)
                throw new ArgumentException("Invalid name");

            if (age < 0)
                throw new ArgumentException("Invalid age");

            Name = name;
            Age = age;
        }

        public void Patch(string name, int? age)
        {
            if (name != null)
                Name = name;

            if (age.HasValue && age.Value > 0)
                Age = age.Value;
        }
	}
}