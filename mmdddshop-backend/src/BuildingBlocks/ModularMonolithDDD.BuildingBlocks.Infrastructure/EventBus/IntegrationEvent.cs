namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.EventBus
{
   /// <summary>
    /// Base class for all integration events in the modular monolith architecture.
    /// Integration events represent significant business occurrences that have happened
    /// in one module and need to be communicated to other modules. These events are
    /// used for inter-module communication and maintaining eventual consistency.
    /// </summary>
    public abstract class IntegrationEvent : INotification
    {
        /// <summary>
        /// Gets the unique identifier of the integration event.
        /// This ID is used for tracking, deduplication, and correlation purposes
        /// across different modules and systems.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets the timestamp when the integration event occurred.
        /// This timestamp represents when the business event actually happened,
        /// not when the integration event was created or published.
        /// </summary>
        public DateTime OccurredOn { get; }

        /// <summary>
        /// Initializes a new instance of the IntegrationEvent base class.
        /// This constructor is protected to ensure that only derived classes
        /// can create instances of integration events.
        /// </summary>
        /// <param name="id">The unique identifier for the integration event</param>
        /// <param name="occurredOn">The timestamp when the business event occurred</param>
        protected IntegrationEvent(Guid id, DateTime occurredOn)
        {
            this.Id = id;
            this.OccurredOn = occurredOn;
        }
    }
}