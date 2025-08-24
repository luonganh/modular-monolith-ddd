namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
	/// <summary>
	/// The <see cref="DomainEventsDispatcherNotificationHandlerDecorator{T}"/> class is a decorator that wraps around 
	/// an existing <see cref="INotificationHandler{T}"/> and ensures that domain events are dispatched after 
	/// the notification handler has completed its processing.
	/// </summary>
	/// <typeparam name="T">The type of notification this decorator handles, which must implement <see cref="INotification"/>.</typeparam>
	public class DomainEventsDispatcherNotificationHandlerDecorator<T> : INotificationHandler<T>
		where T : INotification
	{
		private readonly INotificationHandler<T> _decorated;
		private readonly IDomainEventsDispatcher _domainEventsDispatcher;

		/// <summary>
		/// Initializes a new instance of the <see cref="DomainEventsDispatcherNotificationHandlerDecorator{T}"/> class.
		/// </summary>
		/// <param name="domainEventsDispatcher">The <see cref="IDomainEventsDispatcher"/> used to dispatch domain events after handling the notification.</param>
		/// <param name="decorated">The original <see cref="INotificationHandler{T}"/> being decorated.</param>
		public DomainEventsDispatcherNotificationHandlerDecorator(
			IDomainEventsDispatcher domainEventsDispatcher,
			INotificationHandler<T> decorated)
		{			
			_domainEventsDispatcher = domainEventsDispatcher;			
			_decorated = decorated;
			
		}

		/// <summary>
		/// Handles the notification by first invoking the decorated <see cref="INotificationHandler{T}"/> 
		/// and then dispatching any domain events using the <see cref="IDomainEventsDispatcher"/>.
		/// </summary>
		/// <param name="notification">The notification to be handled.</param>
		/// <param name="cancellationToken">A cancellation token to observe while handling the notification.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		public async Task Handle(T notification, CancellationToken cancellationToken)
		{
			try
			{				
				// Call the decorated notification handler to handle the notification
				await this._decorated.Handle(notification, cancellationToken);
								
				// After the notification has been handled, dispatch any domain events
				await this._domainEventsDispatcher.DispatchEventsAsync();				
			}
			catch (Exception ex)
			{
				Console.WriteLine($"⚠ Lỗi trong Handle: {ex.Message}");				
			}
			
		}
	}
}
