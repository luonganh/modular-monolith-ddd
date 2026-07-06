namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Processing
{
    /// <summary>
    /// Static utility class for executing commands within the UserAccess module.
    /// This class provides a centralized way to execute commands with proper dependency injection scope management.
    /// It creates a new lifetime scope for each command execution to ensure proper resource cleanup.
    /// </summary>
	internal static class CommandsExecutor
	{
        /// <summary>
        /// Executes a command that does not return a result.
        /// Creates a new lifetime scope, resolves the mediator, and sends the command for processing.
        /// The scope is automatically disposed after command execution.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
		internal static async Task Execute(ICommand command)
		{
			using (var scope = UserAccessCompositionRoot.BeginLifetimeScope())
			{
				var mediator = scope.Resolve<IMediator>();
				await mediator.Send(command);
			}
		}

        /// <summary>
        /// Executes a command that returns a result.
        /// Creates a new lifetime scope, resolves the mediator, and sends the command for processing.
        /// The scope is automatically disposed after command execution.
        /// </summary>
        /// <typeparam name="TResult">The type of result returned by the command.</typeparam>
        /// <param name="command">The command to execute.</param>
        /// <returns>A task containing the result of the command execution.</returns>
		internal static async Task<TResult> Execute<TResult>(ICommand<TResult> command)
		{
			using (var scope = UserAccessCompositionRoot.BeginLifetimeScope())
			{
				var mediator = scope.Resolve<IMediator>();
				return await mediator.Send(command);
			}
		}
	}
}