namespace ModularMonolithDDD.Modules.UserAccess.Application.Contracts
{
    /// <summary>
    /// Base class for commands that don't return a result.
    /// Provides common functionality for all command implementations.
    /// </summary>
    public abstract class CommandBase : ICommand
    {
        /// <summary>
        /// Gets the unique identifier for the command.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Initializes a new instance of the CommandBase class with a new GUID.
        /// </summary>
        protected CommandBase()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Initializes a new instance of the CommandBase class with a specific ID.
        /// </summary>
        /// <param name="id">The unique identifier for the command</param>
        protected CommandBase(Guid id)
        {
            Id = id;
        }
    }

    /// <summary>
    /// Base class for commands that return a result.
    /// Provides common functionality for all command implementations with return values.
    /// </summary>
    /// <typeparam name="TResult">The type of result returned by the command</typeparam>
    public abstract class CommandBase<TResult> : ICommand<TResult>
    {
        /// <summary>
        /// Initializes a new instance of the CommandBase class with a new GUID.
        /// </summary>
        protected CommandBase()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Initializes a new instance of the CommandBase class with a specific ID.
        /// </summary>
        /// <param name="id">The unique identifier for the command</param>
        protected CommandBase(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets the unique identifier for the command.
        /// </summary>
        public Guid Id { get; }
    }
}