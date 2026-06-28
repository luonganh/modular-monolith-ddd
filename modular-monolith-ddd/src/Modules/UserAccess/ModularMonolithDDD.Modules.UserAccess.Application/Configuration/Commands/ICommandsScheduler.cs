namespace ModularMonolithDDD.Modules.UserAccess.Application.Configuration.Commands
{
    /// <summary>
    /// Interface for scheduling commands to be executed asynchronously.
    /// Provides a mechanism to queue commands for background processing.
    /// </summary>
    public interface ICommandsScheduler
    {
        /// <summary>
        /// Enqueues a command for asynchronous execution without a return value.
        /// </summary>
        /// <param name="command">The command to enqueue</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task EnqueueAsync(ICommand command);

        /// <summary>
        /// Enqueues a command for asynchronous execution with a return value.
        /// </summary>
        /// <typeparam name="T">The type of the command result</typeparam>
        /// <param name="command">The command to enqueue</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task EnqueueAsync<T>(ICommand<T> command);
    }
}