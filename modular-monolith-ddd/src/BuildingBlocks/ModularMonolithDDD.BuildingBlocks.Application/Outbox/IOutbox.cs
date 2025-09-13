namespace ModularMonolithDDD.BuildingBlocks.Application.Outbox
{
	/// <summary>
	/// Event Publishing – Write events to the outbox.
	/// Transaction Safety – Guarantee events are persisted atomically with the transaction.
	/// Represents the outbox pattern interface for managing domain event messages.
	/// The outbox pattern ensures reliable message delivery by storing events in a database
	/// before publishing them, providing transactional consistency.
	/// </summary>
	public interface IOutbox
	{
		/// <summary>
		/// Adds a domain event message to the outbox for later processing.
    	/// The message will be persisted to the database and published asynchronously.
		/// </summary>
		/// <param name="message">The outbox message containing the domain event data.</param>	
		void Add(OutboxMessage message);

		/// <summary>
		/// Persists all pending outbox messages to the database within the current transaction.
    	/// This method should be called after adding messages to ensure they are saved atomically
    	/// with the business operation that generated the domain events.
		/// </summary>
		/// <returns>A task representing the asynchronous save operation.</returns>
		Task Save();
	}
}
