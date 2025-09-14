namespace ModularMonolithDDD.Modules.UserAccess.Application.Contracts
{
    /// <summary>
    /// Interface for the UserAccess module facade.
    /// Provides a unified entry point for executing commands and queries within the UserAccess module.
    /// </summary>
    public interface IUserAccessModule
    {
        /// <summary>
        /// Executes a command that returns a result asynchronously.
        /// </summary>
        /// <typeparam name="TResult">The type of result returned by the command</typeparam>
        /// <param name="command">The command to execute</param>
        /// <returns>A task representing the asynchronous operation with the command result</returns>
        Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command);

        /// <summary>
        /// Executes a command that doesn't return a result asynchronously.
        /// </summary>
        /// <param name="command">The command to execute</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task ExecuteCommandAsync(ICommand command);

        /// <summary>
        /// Executes a query and returns the result asynchronously.
        /// </summary>
        /// <typeparam name="TResult">The type of result returned by the query</typeparam>
        /// <param name="query">The query to execute</param>
        /// <returns>A task representing the asynchronous operation with the query result</returns>
        Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query);
    }
}