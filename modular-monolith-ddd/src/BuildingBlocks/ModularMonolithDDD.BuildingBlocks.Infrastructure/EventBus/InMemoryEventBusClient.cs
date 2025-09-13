namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.EventBus
{
    /// <summary>
    /// Client wrapper for the InMemoryEventBus that provides logging capabilities
    /// and implements the IEventsBus interface. This class acts as a facade that
    /// delegates all operations to the singleton InMemoryEventBus instance while
    /// adding logging and other cross-cutting concerns.
    /// </summary>
    public class InMemoryEventBusClient : IEventsBus
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the InMemoryEventBusClient.
        /// </summary>
        /// <param name="logger">The logger instance used for event bus operations</param>
        public InMemoryEventBusClient(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Disposes of the event bus client resources.
        /// In this implementation, no cleanup is required as the underlying
        /// InMemoryEventBus is a singleton that manages its own lifecycle.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Publishes an integration event through the in-memory event bus.
        /// This method logs the event publication and delegates to the singleton
        /// InMemoryEventBus instance for actual event distribution.
        /// </summary>
        /// <typeparam name="T">The type of integration event to publish</typeparam>
        /// <param name="event">The integration event instance to publish</param>
        /// <returns>A task representing the asynchronous publish operation</returns>
        public async Task Publish<T>(T @event)
            where T : IntegrationEvent
        {
            _logger.Information("Publishing {Event}", @event.GetType().FullName);
            await InMemoryEventBus.Instance.Publish(@event);
        }

        /// <summary>
        /// Subscribes a handler to receive integration events of a specific type.
        /// This method delegates to the singleton InMemoryEventBus instance for event subscription.
        /// </summary>
        /// <typeparam name="T">The type of integration event to subscribe to</typeparam>
        /// <param name="handler">The handler that will process events of the specified type</param>
        public void Subscribe<T>(IIntegrationEventHandler<T> handler)
            where T : IntegrationEvent
        {
            InMemoryEventBus.Instance.Subscribe(handler);
        }

        /// <summary>
        /// Starts the event bus consumer to begin processing events.
        /// In this implementation, no consumption is required as the underlying
        /// InMemoryEventBus is a singleton that manages its own lifecycle.
        /// </summary>
        public void StartConsuming()
        {
        }
    }
}