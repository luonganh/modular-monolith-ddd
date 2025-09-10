namespace ModularMonolithDDD.BuildingBlocks.Application.Outbox
{
	/// <summary>
	/// Event Persistence – Persisting events in a database.
	/// Reliability – Guarantees no event loss.
	/// Processing Tracking – Tracking the status of processing.
	/// </summary>
	public class OutboxMessage
	{
		/// <summary>
		/// The id.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// The occurred on.
		/// </summary>
		public DateTime OccurredOn { get; set; }

		/// <summary>
		/// The type.
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// The data.
		/// </summary>
		public string Data { get; set; }

		/// <summary>
		/// The processed date.
		/// </summary>
		public DateTime? ProcessedDate { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="OutboxMessage"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="occurredOn">The occurred on.</param>
		/// <param name="type">The type.</param>
		/// <param name="data">The data.</param>
		public OutboxMessage(Guid id, DateTime occurredOn, string type, string data)
		{
			this.Id = id;
			this.OccurredOn = occurredOn;
			this.Type = type;
			this.Data = data;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OutboxMessage"/> class.
		/// </summary>
		private OutboxMessage()
		{
		}
	}
}
