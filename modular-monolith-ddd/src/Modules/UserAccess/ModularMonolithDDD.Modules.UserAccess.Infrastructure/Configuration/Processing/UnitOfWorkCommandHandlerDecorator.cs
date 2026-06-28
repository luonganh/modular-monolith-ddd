namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Processing
{
    /// <summary>
    /// Decorator for command handlers that adds unit of work functionality.
    /// This decorator ensures that all database changes are committed as a single transaction
    /// and handles internal command processing status updates.
    /// </summary>
    /// <typeparam name="T">The type of command being handled.</typeparam>
    internal class UnitOfWorkCommandHandlerDecorator<T> : ICommandHandler<T>
        where T : ICommand
    {
        private readonly ICommandHandler<T> _decorated;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserAccessContext _userAccessContext;

        /// <summary>
        /// Initializes a new instance of the UnitOfWorkCommandHandlerDecorator class.
        /// </summary>
        /// <param name="decorated">The decorated command handler.</param>
        /// <param name="unitOfWork">The unit of work for transaction management.</param>
        /// <param name="userAccessContext">The database context for internal command tracking.</param>
        public UnitOfWorkCommandHandlerDecorator(
            ICommandHandler<T> decorated,
            IUnitOfWork unitOfWork,
            UserAccessContext userAccessContext)
        {
            _decorated = decorated;
            _unitOfWork = unitOfWork;
            _userAccessContext = userAccessContext;
		}

        /// <summary>
        /// Handles the command within a unit of work transaction.
        /// Executes the decorated command handler and commits all changes as a single transaction.
        /// For internal commands, updates the processed date to mark completion.
        /// </summary>
        /// <param name="command">The command to handle.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Handle(T command, CancellationToken cancellationToken)
        {			
			// Execute the decorated command handler
			await this._decorated.Handle(command, cancellationToken);

            // Update internal command processing status if this is an internal command
            if (command is InternalCommandBase)
            {
                var internalCommand = await _userAccessContext.InternalCommands.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken: cancellationToken);

                if (internalCommand != null)
                {
                    internalCommand.ProcessedDate = DateTime.UtcNow;
                }
            }

            // Commit all changes as a single transaction
            await this._unitOfWork.CommitAsync(cancellationToken);
        }
    }
}