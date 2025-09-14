namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Processing
{
    /// <summary>
    /// Decorator for command handlers that return results and adds unit of work functionality.
    /// This decorator ensures that all database changes are committed as a single transaction
    /// and handles internal command processing status updates.
    /// </summary>
    /// <typeparam name="T">The type of command being handled.</typeparam>
    /// <typeparam name="TResult">The type of result returned by the command.</typeparam>
    internal class UnitOfWorkCommandHandlerWithResultDecorator<T, TResult> : ICommandHandler<T, TResult>
        where T : ICommand<TResult>
    {
        private readonly ICommandHandler<T, TResult> _decorated;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserAccessContext _userAccessContext;

        /// <summary>
        /// Initializes a new instance of the UnitOfWorkCommandHandlerWithResultDecorator class.
        /// </summary>
        /// <param name="decorated">The decorated command handler.</param>
        /// <param name="unitOfWork">The unit of work for transaction management.</param>
        /// <param name="userAccessContext">The database context for internal command tracking.</param>
        public UnitOfWorkCommandHandlerWithResultDecorator(
            ICommandHandler<T, TResult> decorated,
            IUnitOfWork unitOfWork,
            UserAccessContext userAccessContext)
        {
            _decorated = decorated;
            _unitOfWork = unitOfWork;
            _userAccessContext = userAccessContext;
		}

        /// <summary>
        /// Handles the command within a unit of work transaction and returns the result.
        /// Executes the decorated command handler and commits all changes as a single transaction.
        /// For internal commands, updates the processed date to mark completion.
        /// </summary>
        /// <param name="command">The command to handle.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task containing the result of the command execution.</returns>
        public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
        {			
			// Execute the decorated command handler and get the result
			var result = await this._decorated.Handle(command, cancellationToken);

            // Update internal command processing status if this is an internal command
            if (command is InternalCommandBase<TResult>)
            {
                var internalCommand = await _userAccessContext.InternalCommands.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken: cancellationToken);

                if (internalCommand != null)
                {
                    internalCommand.ProcessedDate = DateTime.UtcNow;
                }
            }

            // Commit all changes as a single transaction
            await this._unitOfWork.CommitAsync(cancellationToken);

            return result;
        }
    }
}