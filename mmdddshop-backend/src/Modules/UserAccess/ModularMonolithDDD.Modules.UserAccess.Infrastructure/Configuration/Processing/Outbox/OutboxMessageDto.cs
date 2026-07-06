namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Processing.Outbox
{
    /// <summary>
    /// Data Transfer Object for outbox messages retrieved from the database.
    /// This DTO represents the structure of outbox messages when they are fetched
    /// from the database for processing, containing the essential information needed
    /// to deserialize and publish domain events.
    /// </summary>
    public class OutboxMessageDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the outbox message.
        /// This ID corresponds to the original domain event ID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the type name of the domain event notification.
        /// This is used to map the message back to its corresponding domain event type.
        /// </summary>
        public string Type { get; set; } = default!;

        /// <summary>
        /// Gets or sets the serialized data of the domain event notification.
        /// This contains the JSON representation of the domain event payload.
        /// </summary>
        public string Data { get; set; } = default!;
    }
}