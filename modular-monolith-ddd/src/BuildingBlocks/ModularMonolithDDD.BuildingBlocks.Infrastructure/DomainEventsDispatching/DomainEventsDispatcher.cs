namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
    /// <summary>
    /// Implementation of IDomainEventsDispatcher that handles the complete domain event dispatching process.
    /// This class coordinates between domain events, MediatR for internal event handling, and the Outbox pattern
    /// for integration events that need to be sent to external systems.
    /// </summary>
    public class DomainEventsDispatcher : IDomainEventsDispatcher
    {
        private readonly IMediator _mediator;

        private readonly ILifetimeScope _scope;

        private readonly IOutbox _outbox;

        private readonly IDomainEventsAccessor _domainEventsProvider;

        private readonly IDomainNotificationsMapper _domainNotificationsMapper;

        /// <summary>
        /// Initializes a new instance of the DomainEventsDispatcher class.
        /// </summary>
        /// <param name="mediator">MediatR instance for publishing domain events to internal handlers.</param>
        /// <param name="scope">Autofac lifetime scope for resolving domain event notifications.</param>
        /// <param name="outbox">Outbox instance for storing integration events.</param>
        /// <param name="domainEventsProvider">Provider for accessing domain events from the current unit of work.</param>
        /// <param name="domainNotificationsMapper">Mapper for converting domain events to integration event names.</param>
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
        /// Dispatches all pending domain events through two channels:
        /// 1. Internal event handlers via MediatR for application-level processing
        /// 2. Integration events via Outbox pattern for external system communication
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DispatchEventsAsync()
        {
            // Step 1: Retrieve all domain events from the current unit of work
            var domainEvents = _domainEventsProvider.GetAllDomainEvents();

            // Step 2: Create domain event notifications for integration events
            List<IDomainEventNotification<IDomainEvent>> domainEventNotifications = [];
            foreach (var domainEvent in domainEvents)
            {
                Type domainEvenNotificationType = typeof(IDomainEventNotification<>);
                var domainNotificationWithGenericType = domainEvenNotificationType.MakeGenericType(domainEvent.GetType());
                var domainNotification = _scope.ResolveOptional(domainNotificationWithGenericType, new List<Autofac.Core.Parameter>
                {
                    new NamedParameter("domainEvent", domainEvent),
                    new NamedParameter("id", domainEvent.Id)
                });

                if (domainNotification != null)
                {
                    var notification = domainNotification as IDomainEventNotification<IDomainEvent>;
                    if (notification != null)
                    {
                        domainEventNotifications.Add(notification);
                    }                    
                }
            }

            // Step 3: Clear domain events from entities to prevent duplicate processing
            _domainEventsProvider.ClearAllDomainEvents();

            // Step 4: Publish domain events to internal handlers via MediatR
            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent);
            }

            // Step 5: Add integration events to Outbox for external system communication
            foreach (var domainEventNotification in domainEventNotifications)
            {
                var type = _domainNotificationsMapper.GetName(domainEventNotification.GetType());
                if (type == null)
                {
                    // Skip this notification if we can't get its type name
                    continue;
                }
                var data = JsonConvert.SerializeObject(domainEventNotification, new JsonSerializerSettings
                {
                    ContractResolver = new AllPropertiesContractResolver()
                });

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
