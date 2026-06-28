namespace ModularMonolithDDD.Modules.UserAccess.Application.Configuration.Commands
{
    /// <summary>
    /// Base class for internal commands that don't return a result.
    /// Internal commands are used for communication between modules within the application.
    /// </summary>
    public abstract class InternalCommandBase : ICommand
    {
        /// <summary>
        /// Initializes a new instance of the InternalCommandBase class with a specific ID.
        /// </summary>
        /// <param name="id">The unique identifier for the command</param>
        protected InternalCommandBase(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets the unique identifier for the command.
        /// </summary>
        public Guid Id { get; }
    }

    /// <summary>
    /// Base class for internal commands that return a result.
    /// Internal commands are used for communication between modules within the application.
    /// </summary>
    /// <typeparam name="TResult">The type of result returned by the command</typeparam>
    public abstract class InternalCommandBase<TResult> : ICommand<TResult>
    {
        /// <summary>
        /// Initializes a new instance of the InternalCommandBase class with a new GUID.
        /// </summary>
        protected InternalCommandBase()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Initializes a new instance of the InternalCommandBase class with a specific ID.
        /// </summary>
        /// <param name="id">The unique identifier for the command</param>
        protected InternalCommandBase(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets the unique identifier for the command.
        /// </summary>
        public Guid Id { get; }
    }
}