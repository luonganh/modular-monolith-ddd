namespace ModularMonolithDDD.BuildingBlocks.Application.Outbox
{
	/// <summary>
	/// Event Publishing – Write events to the outbox.
	/// Transaction Safety – Guarantee events are persisted atomically with the transaction.
	/// </summary>
	public interface IOutbox
	{
		/// <summary>
		/// Add a message to the outbox.
		/// </summary>
		/// <param name="message">The message.</param>	
		void Add(OutboxMessage message);

		/// <summary>
		/// Save the messages to the outbox.
		/// </summary>
		/// <returns>The task.</returns>
		Task Save();
	}
}
