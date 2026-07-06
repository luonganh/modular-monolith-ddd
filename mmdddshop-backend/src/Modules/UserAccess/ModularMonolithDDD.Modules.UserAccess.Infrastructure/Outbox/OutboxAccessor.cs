namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Outbox
{
    /// <summary>
    /// Implementation of the outbox pattern for reliable message delivery.
    /// Stores outbox messages in the database to ensure they are processed even if the system fails.
    /// </summary>
    public class OutboxAccessor : IOutbox
    {
        private readonly UserAccessContext _userAccessContext;

        /// <summary>
        /// Initializes a new instance of the OutboxAccessor.
        /// </summary>
        /// <param name="userAccessContext">The Entity Framework context for database operations.</param>
        public OutboxAccessor(UserAccessContext userAccessContext)
        {
            _userAccessContext = userAccessContext;
        }

        /// <summary>
        /// Adds an outbox message to the database for reliable delivery.
        /// The message will be persisted when SaveChanges is called on the context.
        /// </summary>
        /// <param name="message">The outbox message to add for processing.</param>
        public void Add(OutboxMessage message)
        {
            _userAccessContext.OutboxMessages.Add(message);
        }

        /// <summary>
        /// Saves the outbox messages to the database.
        /// In this implementation, saving is handled automatically by Entity Framework's change tracking
        /// mechanism when SaveChanges is called on the context, so this method returns immediately.
        /// </summary>
        /// <returns>A completed task since saving is handled by EF Core change tracking.</returns>
        public Task Save()
        {
            return Task.CompletedTask; // Save is done automatically using EF Core Change Tracking mechanism during SaveChanges.
        }
    }
}