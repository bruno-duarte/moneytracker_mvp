namespace MoneyTracker.Application.Exceptions
{
  public class InvalidReferenceException(string entityName, object key) : Exception($"{entityName} with key '{key}' does not exist.")
  {
    public string EntityName { get; } = entityName;
    public object Key { get; } = key;
  }
}
