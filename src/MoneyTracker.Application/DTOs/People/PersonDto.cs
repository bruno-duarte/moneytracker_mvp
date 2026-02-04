namespace MoneyTracker.Application.DTOs.People
{
	public class PersonDto
	{
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Age { get; set; }
	}
}