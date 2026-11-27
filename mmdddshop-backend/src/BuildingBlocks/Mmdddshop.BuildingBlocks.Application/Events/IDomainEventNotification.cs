namespace Mmdddshop.BuildingBlocks.Application.Events
{
    /// <summary>
    /// Interface for domain event notifications.
    /// Event Publishing System.
    /// MediatR Integration.
    /// </summary>
    public interface IDomainEventNotification<out TEventType> : IDomainEventNotification
    {
        /// <summary>
        /// Gets the domain event.
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
        /// Gets the id of the domain event.
        /// Event identification.
        /// </summary>
        Guid Id { get; }
    }
}
