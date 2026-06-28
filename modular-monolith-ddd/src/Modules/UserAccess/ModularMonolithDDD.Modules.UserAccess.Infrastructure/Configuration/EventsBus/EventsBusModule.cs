namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.EventsBus
{
    /// <summary>
    /// Autofac module responsible for configuring event bus dependencies for the UserAccess module.
    /// This module handles the registration of event bus implementations, supporting both external
    /// event bus instances and fallback to in-memory event bus for local development and testing.
    /// </summary>
    internal class EventsBusModule : Module
	{
		private readonly IEventsBus _eventsBus;

		/// <summary>
		/// Initializes a new instance of the EventsBusModule class.
		/// </summary>
		/// <param name="eventsBus">Optional external event bus instance. If null, an in-memory event bus will be used as fallback.</param>
		public EventsBusModule(IEventsBus eventsBus)
		{
			_eventsBus = eventsBus;
		}

		/// <summary>
		/// Loads the event bus configuration into the Autofac container builder.
		/// Registers either the provided external event bus instance or creates a new in-memory event bus
		/// as a singleton to ensure consistent event handling throughout the application lifecycle.
		/// </summary>
		/// <param name="builder">The Autofac container builder used for dependency registration.</param>
		protected override void Load(ContainerBuilder builder)
		{
			if (_eventsBus != null)
			{
				// Register the provided external event bus instance as singleton
				// This is typically used in production environments with external message brokers
				builder.RegisterInstance(_eventsBus).SingleInstance();
			}
			else
			{
				// Register in-memory event bus as fallback
				// This is used for local development, testing, or when no external event bus is configured
				builder.RegisterType<InMemoryEventBusClient>()
					.As<IEventsBus>()
					.SingleInstance();
			}
		}
	}
}