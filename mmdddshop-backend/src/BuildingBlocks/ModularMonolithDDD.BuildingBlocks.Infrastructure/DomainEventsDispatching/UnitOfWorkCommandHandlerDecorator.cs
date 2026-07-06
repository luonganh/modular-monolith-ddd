namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
    /// <summary>
    /// Decorator for MediatR command handlers that automatically commits the unit of work
    /// after the decorated handler has completed its processing.
    /// This decorator ensures that database transactions are properly committed and
    /// domain events are dispatched after successful command execution.
    /// </summary>
    /// <typeparam name="T">The type of command being handled.</typeparam>
    public class UnitOfWorkCommandHandlerDecorator<T> : IRequestHandler<T>
        where T : IRequest
    {
        private readonly IRequestHandler<T> _decorated;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the UnitOfWorkCommandHandlerDecorator class.
        /// </summary>
        /// <param name="decorated">The actual command handler being decorated.</param>
        /// <param name="unitOfWork">The unit of work used to commit database transactions.</param>
        public UnitOfWorkCommandHandlerDecorator(
            IRequestHandler<T> decorated,
            IUnitOfWork unitOfWork)
        {
            _decorated = decorated;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Handles the command by first executing the decorated handler, then automatically
        /// committing the unit of work to persist all changes and dispatch domain events.
        /// This ensures that database transactions are properly managed and domain events
        /// are only dispatched after successful command execution.
        /// </summary>
        /// <param name="command">The command to handle.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Handle(T command, CancellationToken cancellationToken)
        {
            // Step 1: Execute the decorated command handler first
            await this._decorated.Handle(command, cancellationToken);

            // Step 2: Commit the unit of work to persist changes and dispatch domain events
            await this._unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
