namespace MoneyTracker.Domain.Queries
{
	public class PersonQuery : BaseQuery
	{
        public string? Name { get; set; }
        public int? Age { get; set; }
	}
}