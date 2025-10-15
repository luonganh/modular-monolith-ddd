namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.Inbox
{
    /// <summary>
    /// Represents an incoming integration event message stored in the inbox pattern.
    /// This class is used to store integration events received from other modules
    /// before they are processed, ensuring reliable message delivery and preventing
    /// duplicate processing in the modular monolith architecture.
    /// </summary>
    public class InboxMessage
    {
        /// <summary>
        /// Gets or sets the unique identifier of the inbox message.
        /// This ID is used for tracking, deduplication, and correlation purposes
        /// to ensure each message is processed only once.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the original business event occurred.
        /// This represents when the business event actually happened in the
        /// originating module, not when it was received or stored.
        /// </summary>
        public DateTime OccurredOn { get; set; }

        /// <summary>
        /// Gets or sets the full type name of the integration event.
        /// This is used for deserialization and routing the message to the
        /// appropriate handler when processing.
        /// </summary>
        public string Type { get; set; } = default!;

        /// <summary>
        /// Gets or sets the serialized JSON data of the integration event.
        /// This contains the complete event payload that will be deserialized
        /// and processed by the appropriate handler.
        /// </summary>
        public string Data { get; set; } = default!;

        /// <summary>
        /// Gets or sets the timestamp when the message was successfully processed.
        /// This is null for unprocessed messages and set to the processing time
        /// when the message has been handled to prevent duplicate processing.
        /// </summary>
        public DateTime? ProcessedDate { get; set; }

        /// <summary>
        /// Initializes a new instance of the InboxMessage class.
        /// This constructor is used when creating new inbox messages from
        /// incoming integration events.
        /// </summary>
        /// <param name="occurredOn">The timestamp when the original business event occurred</param>
        /// <param name="type">The full type name of the integration event</param>
        /// <param name="data">The serialized JSON data of the integration event</param>
        public InboxMessage(DateTime occurredOn, string type, string data)
        {
            this.Id = Guid.NewGuid();
            this.OccurredOn = occurredOn;
            this.Type = type;
            this.Data = data;
        }

        /// <summary>
        /// Private parameterless constructor for Entity Framework Core.
        /// This constructor is required by EF Core for materializing entities
        /// from the database and should not be used directly.
        /// </summary>
        private InboxMessage()
        {
        }
    }
}