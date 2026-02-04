namespace MoneyTracker.Application.DTOs.Reports
{
	public class PersonSummaryDto
    {
        public Guid PersonId { get; set; }
        public string Name { get; set; } = null!;
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Balance => TotalIncome - TotalExpense;
    }
}
