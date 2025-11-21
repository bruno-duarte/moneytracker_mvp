namespace MoneyTracker.Domain.Entities
{
  public class Category
  {
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    private Category() { }

    public Category(Guid id, string name)
    {
      Id = id == Guid.Empty ? Guid.NewGuid() : id;
      if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required", nameof(name));
      Name = name;
    }

    public void Update(string name)
    {
      if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required", nameof(name));
      Name = name;
    }
  }
}