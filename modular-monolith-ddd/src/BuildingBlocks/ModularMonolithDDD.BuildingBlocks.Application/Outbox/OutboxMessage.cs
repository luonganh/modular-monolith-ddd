namespace ModularMonolithDDD.BuildingBlocks.Application.Outbox
{
	/// <summary>
	/// Event Persistence – Persisting events in a database.
	/// Reliability – Guarantees no event loss.
	/// Processing Tracking – Tracking the status of processing.
	/// Represents a message stored in the outbox pattern for reliable event publishing.
    /// The outbox pattern ensures that domain events are reliably published to external systems
    /// by storing them in the same database transaction as the business operation.
	/// </summary>
	public class OutboxMessage
	{
		/// <summary>
        /// Unique identifier for the outbox message.
        /// </summary>
		public Guid Id { get; set; }

		/// <summary>
        /// Timestamp when the domain event occurred.
        /// </summary>
		public DateTime OccurredOn { get; set; }

		/// <summary>
        /// The type name of the domain event (e.g., fully qualified class name).
        /// Used for deserialization and routing the event to appropriate handlers.
        /// </summary>
		public string Type { get; set; }

		/// <summary>
        /// Serialized JSON data of the domain event.
        /// Contains the actual event payload that will be published to external systems.
        /// </summary>
		public string Data { get; set; }

		/// <summary>
        /// Timestamp when the message was successfully processed and published.
        /// Null indicates the message is still pending processing.
        /// </summary>
		public DateTime? ProcessedDate { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="OutboxMessage"/> class.
		/// </summary>
		/// <param name="id">Unique identifier for the message.</param>
        /// <param name="occurredOn">Timestamp when the domain event occurred.</param>
        /// <param name="type">The type name of the domain event.</param>
        /// <param name="data">Serialized JSON data of the domain event.</param>
		public OutboxMessage(Guid id, DateTime occurredOn, string type, string data)
		{
			this.Id = id;
			this.OccurredOn = occurredOn;
			this.Type = type;
			this.Data = data;
		}

		/// <summary>
		/// Private parameterless constructor required by Entity Framework for database mapping.
		/// </summary>
		private OutboxMessage()
		{
		}
	}
}
