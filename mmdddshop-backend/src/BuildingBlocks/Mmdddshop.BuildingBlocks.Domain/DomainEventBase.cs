namespace Mmdddshop.BuildingBlocks.Domain
{
    /// <summary>
    /// Theoretical best practice (Pure DDD): DomainEventBase is abstract class
    /// Practical flexibility (Pragmatic DDD): DomainEventBase is concrete class.
    /// </summary>
    public class DomainEventBase : IDomainEvent
    {
        /// <summary>
        /// Gets unique identifier for the domain event.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets the date and time the domain event occurred.
        /// </summary>
        public DateTime OccurredOn { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEventBase"/> class.
        /// </summary>
        public DomainEventBase()
        {
            this.Id = Guid.NewGuid();
            this.OccurredOn = DateTime.UtcNow;
        }
    }
}
