namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Processing.Inbox
{
    /// <summary>
    /// Data transfer object for inbox message data from the database.
    /// This DTO represents a message that was received from external systems
    /// and needs to be processed by the inbox pattern.
    /// </summary>
    public class InboxMessageDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the inbox message.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the type name of the message for deserialization.
        /// </summary>
        public string Type { get; set; } = default!;

        /// <summary>
        /// Gets or sets the serialized message data.
        /// </summary>
        public string Data { get; set; } = default!;
    }
}