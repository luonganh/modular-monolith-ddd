namespace ModularMonolithDDD.Modules.UserAccess.Application.Contracts
{
    /// <summary>
    /// Interface for commands that return a result.
    /// Represents a request to perform an action that will return data.
    /// </summary>
    /// <typeparam name="TResult">The type of result returned by the command</typeparam>
    public interface ICommand<out TResult> : IRequest<TResult>
    {
        /// <summary>
        /// Gets the unique identifier for the command.
        /// </summary>
        Guid Id { get; }
    }

    /// <summary>
    /// Interface for commands that don't return a result.
    /// Represents a request to perform an action without returning data.
    /// </summary>
    public interface ICommand : IRequest
    {
        /// <summary>
        /// Gets the unique identifier for the command.
        /// </summary>
        Guid Id { get; }
    }
}
