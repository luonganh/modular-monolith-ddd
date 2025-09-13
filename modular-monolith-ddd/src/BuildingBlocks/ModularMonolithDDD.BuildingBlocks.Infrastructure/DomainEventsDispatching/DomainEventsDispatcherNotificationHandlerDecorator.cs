namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
    /// <summary>
    /// Decorator for MediatR notification handlers that automatically dispatches domain events
    /// after the decorated handler has completed its processing.
    /// This decorator ensures that domain events raised during command/query execution are
    /// automatically dispatched without requiring manual intervention.
    /// </summary>
    /// <typeparam name="T">The type of notification being handled.</typeparam>
    public class DomainEventsDispatcherNotificationHandlerDecorator<T> : INotificationHandler<T>
        where T : INotification
    {
        private readonly INotificationHandler<T> _decorated;
        private readonly IDomainEventsDispatcher _domainEventsDispatcher;

        /// <summary>
        /// Initializes a new instance of the DomainEventsDispatcherNotificationHandlerDecorator class.
        /// </summary>
        /// <param name="domainEventsDispatcher">The domain events dispatcher used to dispatch events after handler completion.</param>
        /// <param name="decorated">The actual notification handler being decorated.</param>
        public DomainEventsDispatcherNotificationHandlerDecorator(
            IDomainEventsDispatcher domainEventsDispatcher,
            INotificationHandler<T> decorated)
        {
            _domainEventsDispatcher = domainEventsDispatcher;
            _decorated = decorated;
        }

        /// <summary>
        /// Handles the notification by first executing the decorated handler, then automatically
        /// dispatching any domain events that were raised during the handler's execution.
        /// This ensures that domain events are processed in the correct order and context.
        /// </summary>
        /// <param name="notification">The notification to handle.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Handle(T notification, CancellationToken cancellationToken)
        {
            // Step 1: Execute the decorated handler first
            await this._decorated.Handle(notification, cancellationToken);

            // Step 2: Automatically dispatch any domain events raised during handler execution
            await this._domainEventsDispatcher.DispatchEventsAsync();
        }
    }
}
