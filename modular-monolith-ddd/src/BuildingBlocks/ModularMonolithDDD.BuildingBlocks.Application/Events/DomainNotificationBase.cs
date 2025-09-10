namespace ModularMonolithDDD.BuildingBlocks.Application.Events
{
	/// <summary>
	/// Base implementation.
	/// Wrap domain events in notifications.
	/// </summary>
	public class DomainNotificationBase<T> : IDomainEventNotification<T>
		where T : IDomainEvent
	{
		/// <summary>
		/// The domain event.
		/// </summary>
		public T DomainEvent { get; }

		/// <summary>
		/// The id of the domain notification.
		/// Id generation: auto-generated unique ID.
		/// </summary>
		public Guid Id { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="DomainNotificationBase{T}"/> class.
		/// </summary>
		/// <param name="domainEvent">The domain event.</param>
		/// <param name="id">The id of the domain notification.</param>
		public DomainNotificationBase(T domainEvent, Guid id)
		{
			this.Id = id;
			this.DomainEvent = domainEvent;
		}
	}
}
