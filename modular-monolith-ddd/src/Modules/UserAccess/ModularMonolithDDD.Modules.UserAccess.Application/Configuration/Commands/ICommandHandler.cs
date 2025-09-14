namespace ModularMonolithDDD.Modules.UserAccess.Application.Configuration.Commands
{
    /// <summary>
    /// Interface for command handlers that process commands without returning a result.
    /// Implements the Command Query Responsibility Segregation (CQRS) pattern.
    /// </summary>
    /// <typeparam name="TCommand">The type of command to handle</typeparam>
    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
        where TCommand : ICommand
    {
    }

    /// <summary>
    /// Interface for command handlers that process commands and return a result.
    /// Implements the Command Query Responsibility Segregation (CQRS) pattern.
    /// </summary>
    /// <typeparam name="TCommand">The type of command to handle</typeparam>
    /// <typeparam name="TResult">The type of result returned by the command</typeparam>
    public interface ICommandHandler<in TCommand, TResult> :
        IRequestHandler<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
    }
}