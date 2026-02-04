namespace MoneyTracker.Application.DTOs.People
{
	public class PersonQueryDto : BaseQueryDto
	{
        public string? Name { get; set; }
        public int? Age { get; set; }
	}
}