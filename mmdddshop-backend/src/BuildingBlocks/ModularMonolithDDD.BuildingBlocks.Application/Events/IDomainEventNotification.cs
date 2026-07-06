namespace ModularMonolithDDD.BuildingBlocks.Application.Events
{
	/// <summary>
	/// Interface for domain event notifications.
	/// Event Publishing System.
	/// MediatR Integration.
	/// </summary>
	public interface IDomainEventNotification<out TEventType> : IDomainEventNotification
	{
		/// <summary>
		/// The domain event.
		/// Generic type for the domain event.
		/// </summary>
		TEventType DomainEvent { get; }
	}

	/// <summary>
	/// Interface for domain event notifications.	
	/// </summary>
	public interface IDomainEventNotification : INotification
	{
		/// <summary>
		/// The id of the domain event.
		/// Event identification.
		/// </summary>
		Guid Id { get; }
	}
}
