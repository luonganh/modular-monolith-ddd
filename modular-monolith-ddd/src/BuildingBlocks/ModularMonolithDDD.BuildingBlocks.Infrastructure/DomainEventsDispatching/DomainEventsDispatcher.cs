namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
	/// <summary>
	/// The <see cref="DomainEventsDispatcher"/> is responsible for handling and dispatching domain events 
	/// within the system. It collects domain events from entities, converts them to domain event notifications, 
	/// publishes them through MediatR, and stores them in an outbox for further processing.
	/// </summary>
	public class DomainEventsDispatcher : IDomainEventsDispatcher
	{
		private readonly IMediator _mediator;

		private readonly ILifetimeScope _scope;

		private readonly IOutbox _outbox;

		private readonly IDomainEventsAccessor _domainEventsProvider;

		private readonly IDomainNotificationsMapper _domainNotificationsMapper;

		/// <summary>
		/// Initializes a new instance of the <see cref="DomainEventsDispatcher"/> class.
		/// </summary>
		/// <param name="mediator">The mediator used for publishing domain events.</param>
		/// <param name="scope">The Autofac lifetime scope for resolving domain event notifications.</param>
		/// <param name="outbox">The outbox used for storing domain event notifications to be processed later.</param>
		/// <param name="domainEventsProvider">The provider for retrieving all domain events from tracked entities.</param>
		/// <param name="domainNotificationsMapper">The mapper to convert domain event notifications to serialized data.</param>
		public DomainEventsDispatcher(			
			IMediator mediator,
			ILifetimeScope scope,
			IOutbox outbox,
			IDomainEventsAccessor domainEventsProvider,
			IDomainNotificationsMapper domainNotificationsMapper)
		{
			_mediator = mediator;
			_scope = scope;
			_outbox = outbox;
			_domainEventsProvider = domainEventsProvider;
			_domainNotificationsMapper = domainNotificationsMapper;						
		}

		/// <summary>
		/// Dispatches all collected domain events asynchronously.
		/// <para>
		/// This method collects domain events, resolves corresponding domain event notifications from the container, 
		/// publishes the events using MediatR, and stores the events in the outbox for persistence.
		/// </para>
		/// </summary>
		public async Task DispatchEventsAsync()
		{
			// Collect all domain events from the domain events provider
			var domainEvents = _domainEventsProvider.GetAllDomainEvents();

			List<IDomainEventNotification<IDomainEvent>> domainEventNotifications = [];

			// Convert each domain event into a corresponding domain event notification
			foreach (var domainEvent in domainEvents)
			{
				Type domainEvenNotificationType = typeof(IDomainEventNotification<>);
				var domainNotificationWithGenericType = domainEvenNotificationType.MakeGenericType(domainEvent.GetType());

				// Resolve the domain event notification from the container and pass the domain event as a parameter
				var domainNotification = _scope.ResolveOptional(domainNotificationWithGenericType, new List<Autofac.Core.Parameter>
				{
					new NamedParameter("domainEvent", domainEvent),
					new NamedParameter("id", domainEvent.Id)
				});

				if (domainNotification != null)
				{
					// Add the resolved domain event notification to the list
					domainEventNotifications.Add(domainNotification as IDomainEventNotification<IDomainEvent>);
				}
			}

			// Clear all domain events after processing them
			_domainEventsProvider.ClearAllDomainEvents();

			// Publish each domain event using MediatR
			foreach (var domainEvent in domainEvents)
			{
				await _mediator.Publish(domainEvent);
			}

			// For each domain event notification, serialize it and store it in the outbox
			foreach (var domainEventNotification in domainEventNotifications)
			{
				var type = _domainNotificationsMapper.GetName(domainEventNotification.GetType());
				var data = JsonConvert.SerializeObject(domainEventNotification, new JsonSerializerSettings
				{
					ContractResolver = new AllPropertiesContractResolver()
				});

				// Create and add the outbox message for persistent storage
				var outboxMessage = new OutboxMessage(
					domainEventNotification.Id,
					domainEventNotification.DomainEvent.OccurredOn,
					type,
					data);

				_outbox.Add(outboxMessage);
			}
		}
	}
}
