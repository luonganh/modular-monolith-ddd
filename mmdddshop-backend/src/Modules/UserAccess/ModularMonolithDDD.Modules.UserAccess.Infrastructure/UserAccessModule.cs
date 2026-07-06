namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure
{
    /// <summary>
    /// Main entry point for the UserAccess module. This class implements the IUserAccessModule interface
    /// and provides methods to execute commands and queries within the module's context.
    /// It acts as a facade that handles dependency injection and lifetime management for module operations.
    /// </summary>
    public class UserAccessModule : IUserAccessModule
    {
        /// <summary>
        /// Executes a command that returns a result asynchronously.
        /// This method handles commands that produce a return value (e.g., CreateUserCommand returns UserId).
        /// </summary>
        /// <typeparam name="TResult">The type of result returned by the command.</typeparam>
        /// <param name="command">The command to execute, must implement ICommand&lt;TResult&gt;.</param>
        /// <returns>A Task containing the result of the command execution.</returns>
        public async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
        {
            // Delegate command execution to CommandsExecutor which handles the actual command processing
            // CommandsExecutor is a static class that manages command execution pipeline
            return await CommandsExecutor.Execute(command);
        }

        /// <summary>
        /// Executes a command that does not return a result asynchronously.
        /// This method handles commands that perform actions without returning values (e.g., DeleteUserCommand).
        /// </summary>
        /// <param name="command">The command to execute, must implement ICommand.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task ExecuteCommandAsync(ICommand command)
        {
            // Delegate command execution to CommandsExecutor for void commands
            // The command will be processed but no result will be returned
            await CommandsExecutor.Execute(command);
        }

        /// <summary>
        /// Executes a query that returns a result asynchronously.
        /// This method handles queries that retrieve data from the system (e.g., GetUserByIdQuery).
        /// Unlike commands, queries use direct MediatR resolution for better performance.
        /// </summary>
        /// <typeparam name="TResult">The type of result returned by the query.</typeparam>
        /// <param name="query">The query to execute, must implement IQuery&lt;TResult&gt;.</param>
        /// <returns>A Task containing the result of the query execution.</returns>
        public async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
        {
            // Create a new lifetime scope from the UserAccessCompositionRoot
            // This ensures proper dependency injection and lifetime management for the query
            // The scope will be automatically disposed when exiting the using block
			using (var scope = UserAccessCompositionRoot.BeginLifetimeScope())
			{
                // Resolve IMediator from the dependency injection container
                // IMediator is the MediatR interface that handles query processing
                // It will find and execute the appropriate query handler
				var mediator = scope.Resolve<IMediator>();

                // Send the query to MediatR for processing
                // MediatR will route the query to the correct handler based on the query type
                // The handler will process the query and return the result
				return await mediator.Send(query);
			}
            // The scope is automatically disposed here, cleaning up any resolved dependencies
		}
    }
}