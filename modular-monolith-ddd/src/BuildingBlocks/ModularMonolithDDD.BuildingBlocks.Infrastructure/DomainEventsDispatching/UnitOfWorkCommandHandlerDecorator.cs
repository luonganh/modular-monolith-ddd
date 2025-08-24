namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
	/// <summary>
	/// The <see cref="UnitOfWorkCommandHandlerDecorator{T}"/> class is a decorator that wraps an existing 
	/// <see cref="IRequestHandler{T}"/> to ensure that a unit of work is committed after the command is handled.
	/// This ensures that changes are persisted to the database or any other necessary storage after processing the command.
	/// </summary>
	/// <typeparam name="T">The type of the command being handled, which must implement <see cref="IRequest"/>.</typeparam>
	public class UnitOfWorkCommandHandlerDecorator<T> : IRequestHandler<T>
		where T : IRequest
	{
		private readonly IRequestHandler<T> _decorated;
		private readonly IUnitOfWork _unitOfWork;

		/// <summary>
		/// Initializes a new instance of the <see cref="UnitOfWorkCommandHandlerDecorator{T}"/> class.
		/// </summary>
		/// <param name="decorated">The original command handler that is being decorated.</param>
		/// <param name="unitOfWork">The unit of work that will be committed after handling the command.</param>
		public UnitOfWorkCommandHandlerDecorator(
			IRequestHandler<T> decorated,
			IUnitOfWork unitOfWork)
		{
			_decorated = decorated;
			_unitOfWork = unitOfWork;			
		}

		/// <summary>
		/// Handles the command by invoking the decorated handler and then committing the unit of work.
		/// </summary>
		/// <param name="command">The command to be handled.</param>
		/// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		public async Task Handle(T command, CancellationToken cancellationToken)
		{			
			// Delegate the handling of the command to the decorated handler
			await this._decorated.Handle(command, cancellationToken);

			// Commit the unit of work after handling the command0						
			await this._unitOfWork.CommitAsync(cancellationToken);
		}
	}
}
