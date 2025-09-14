namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Processing
{
    /// <summary>
    /// Decorator for command handlers that adds validation functionality.
    /// This decorator validates commands using FluentValidation validators before executing the command handler.
    /// If validation fails, an InvalidCommandException is thrown with the validation errors.
    /// </summary>
    /// <typeparam name="T">The type of command being handled.</typeparam>
    internal class ValidationCommandHandlerDecorator<T> : ICommandHandler<T>
        where T : ICommand
    {
        private readonly IList<IValidator<T>> _validators;
        private readonly ICommandHandler<T> _decorated;

        /// <summary>
        /// Initializes a new instance of the ValidationCommandHandlerDecorator class.
        /// </summary>
        /// <param name="validators">The list of validators to use for command validation.</param>
        /// <param name="decorated">The decorated command handler.</param>
        public ValidationCommandHandlerDecorator(
            IList<IValidator<T>> validators,
            ICommandHandler<T> decorated)
        {
			_validators = validators;
            _decorated = decorated;
		}

        /// <summary>
        /// Handles the command with validation.
        /// Validates the command using all registered validators before executing the decorated handler.
        /// Throws InvalidCommandException if validation fails.
        /// </summary>
        /// <param name="command">The command to handle.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidCommandException">Thrown when command validation fails.</exception>
        public async Task Handle(T command, CancellationToken cancellationToken)
        {			
			// Validate the command using all registered validators
			var errors = _validators
                .Select(v => v.Validate(command))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            // Throw exception if validation errors exist
            if (errors.Any())
            {
                throw new InvalidCommandException(errors.Select(x => x.ErrorMessage).ToList());
            }

            // Execute the decorated command handler if validation passes
            await _decorated.Handle(command, cancellationToken);
        }
    }
}