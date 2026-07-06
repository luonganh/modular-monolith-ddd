namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.EventBus
{
      /// <summary>
    /// Defines the contract for an event bus that enables communication between modules
    /// in a modular monolith architecture. The event bus provides a decoupled way for
    /// modules to publish and subscribe to integration events.
    /// </summary>
    public interface IEventsBus : IDisposable
    {
        /// <summary>
        /// Publishes an integration event to all registered subscribers.
        /// This method is used by modules to broadcast events that other modules
        /// might be interested in processing.
        /// </summary>
        /// <typeparam name="T">The type of integration event to publish</typeparam>
        /// <param name="event">The integration event instance to publish</param>
        /// <returns>A task representing the asynchronous publish operation</returns>
        Task Publish<T>(T @event)
            where T : IntegrationEvent;

        /// <summary>
        /// Subscribes a handler to receive integration events of a specific type.
        /// When an event of the specified type is published, all registered handlers
        /// for that type will be invoked.
        /// </summary>
        /// <typeparam name="T">The type of integration event to subscribe to</typeparam>
        /// <param name="handler">The handler that will process events of the specified type</param>
        void Subscribe<T>(IIntegrationEventHandler<T> handler)
            where T : IntegrationEvent;

        /// <summary>
        /// Starts the event bus consumer to begin processing events.
        /// This method initializes the background processing mechanism that
        /// handles event consumption and delivery to registered handlers.
        /// </summary>
        void StartConsuming();
    }
}