namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.EventsBus
{
    /// <summary>
    /// Static startup class responsible for initializing event bus subscriptions for the UserAccess module.
    /// This class handles the setup of integration event handlers and manages the subscription process
    /// to external event sources during application startup.
    /// </summary>
    public static class EventsBusStartup
    {
        /// <summary>
        /// Initializes the event bus system for the UserAccess module.
        /// This method should be called during application startup to set up all necessary
        /// event subscriptions and handlers.
        /// </summary>
        /// <param name="logger">The Serilog logger instance for logging subscription activities.</param>
        public static void Initialize(
            Serilog.ILogger logger)
        {
            SubscribeToIntegrationEvents(logger);
        }

        /// <summary>
        /// Subscribes to all integration events that the UserAccess module needs to handle.
        /// This method resolves the event bus from the DI container and sets up subscriptions
        /// for all relevant integration events from other modules or external systems.
        /// </summary>
        /// <param name="logger">The Serilog logger instance for logging subscription activities.</param>
        private static void SubscribeToIntegrationEvents(Serilog.ILogger logger)
        {
			// Resolve the event bus instance from the DI container
			var eventBus = UserAccessCompositionRoot.BeginLifetimeScope().Resolve<IEventsBus>();
			
			// Subscribe to specific integration events
			// Uncomment and add specific integration events as needed
			//SubscribeToIntegrationEvent<MemberCreatedIntegrationEvent>(eventBus, logger);
		}

        /// <summary>
        /// Subscribes to a specific integration event type using a generic handler.
        /// This method creates a subscription for the specified integration event type
        /// and logs the subscription activity for monitoring and debugging purposes.
        /// </summary>
        /// <typeparam name="T">The type of integration event to subscribe to.</typeparam>
        /// <param name="eventBus">The event bus instance to subscribe to.</param>
        /// <param name="logger">The Serilog logger instance for logging subscription activities.</param>
        private static void SubscribeToIntegrationEvent<T>(IEventsBus eventBus, Serilog.ILogger logger)
            where T : IntegrationEvent
        {
            logger.Information("Subscribe to {@IntegrationEvent}", typeof(T).FullName);
            eventBus.Subscribe(
                new IntegrationEventGenericHandler<T>());
        }
    }
}