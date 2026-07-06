namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.EventBus
{
    /// <summary>
    /// In-memory implementation of the event bus that provides synchronous event publishing
    /// and subscription capabilities within a single application process. This implementation
    /// is suitable for modular monolith architectures where all modules run in the same process.
    /// </summary>
    public sealed class InMemoryEventBus
    {
        /// <summary>
        /// Static constructor for the InMemoryEventBus singleton.
        /// This ensures the singleton instance is created only once.
        /// </summary>
        static InMemoryEventBus()
        {
        }

        /// <summary>
        /// Private constructor for the singleton pattern.
        /// Initializes the handlers dictionary to store event type to handler mappings.
        /// </summary>
        private InMemoryEventBus()
        {
            _handlersDictionary = new Dictionary<string, List<IIntegrationEventHandler>>();
        }

        /// <summary>
        /// Gets the singleton instance of the InMemoryEventBus.
        /// This ensures only one event bus instance exists throughout the application.
        /// </summary>
        public static InMemoryEventBus Instance { get; } = new InMemoryEventBus();

        /// <summary>
        /// Dictionary that maps event type names to lists of registered handlers.
        /// The key is the full type name of the integration event, and the value
        /// is a list of handlers that can process events of that type.
        /// </summary>
        private readonly IDictionary<string, List<IIntegrationEventHandler>> _handlersDictionary;

        /// <summary>
        /// Subscribes a handler to receive integration events of a specific type.
        /// Multiple handlers can be registered for the same event type, and all
        /// registered handlers will be invoked when an event of that type is published.
        /// </summary>
        /// <typeparam name="T">The type of integration event to subscribe to</typeparam>
        /// <param name="handler">The handler that will process events of the specified type</param>
        public void Subscribe<T>(IIntegrationEventHandler<T> handler)
            where T : IntegrationEvent
        {
            var eventType = typeof(T).FullName;
            if (eventType != null)
            {
                if (_handlersDictionary.ContainsKey(eventType))
                {
                    var handlers = _handlersDictionary[eventType];
                    handlers.Add(handler);
                }
                else
                {
                    _handlersDictionary.Add(eventType, [handler]);
                }
            }
        }

        /// <summary>
        /// Publishes an integration event to all registered handlers of that event type.
        /// This method synchronously invokes all handlers registered for the event type,
        /// ensuring that all subscribers receive the event in the same process.
        /// </summary>
        /// <typeparam name="T">The type of integration event to publish</typeparam>
        /// <param name="event">The integration event instance to publish</param>
        /// <returns>A task representing the asynchronous publish operation</returns>
        public async Task Publish<T>(T @event)
            where T : IntegrationEvent
        {
            var eventType = @event.GetType().FullName;

            if (eventType == null)
            {
                return;
            }

            List<IIntegrationEventHandler> integrationEventHandlers = _handlersDictionary[eventType];

            foreach (var integrationEventHandler in integrationEventHandlers)
            {
                if (integrationEventHandler is IIntegrationEventHandler<T> handler)
                {
                    await handler.Handle(@event);
                }
            }
        }
    }  
}