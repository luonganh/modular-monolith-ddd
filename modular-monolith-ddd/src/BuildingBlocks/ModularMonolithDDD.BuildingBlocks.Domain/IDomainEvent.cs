namespace ModularMonolithDDD.BuildingBlocks.Domain
{
	/// <summary>
	/// Represents a domain event.
	/// Domain event is handled by pattern mediator.
	/// Implement INotification from MediatR.
	/// </summary>
    public interface IDomainEvent : INotification
	{
		/// <summary>
		/// Unique identifier for the domain event.
		/// </summary>
		Guid Id { get; }

		/// <summary>
		/// The date and time the domain event occurred.
		/// </summary>
		DateTime OccurredOn { get; }
	}
}
