using MoneyTracker.Domain.Enums;

namespace MoneyTracker.Application.DTOs.Transactions
{
	/// <summary>
	/// Data transfer object used to create a new financial transaction.
	/// </summary>
	/// <param name="Amount">Monetary value of the transaction.</param>
	/// <param name="Type">Type of transaction (income or expense).</param>
	/// <param name="CategoryId">Identifier of the associated category.</param>
	/// <param name="Date">Date when the transaction occurred.</param>
	/// <param name="Description">Optional description or note about the transaction.</param>
	/// <param name="PersonId">Identifier of the person associated with the transaction.</param>
    public record TransactionSaveDto(
		decimal Amount,
		TransactionType Type,
		Guid CategoryId,
		DateTime Date,
		string? Description,
        Guid PersonId
	);
}
