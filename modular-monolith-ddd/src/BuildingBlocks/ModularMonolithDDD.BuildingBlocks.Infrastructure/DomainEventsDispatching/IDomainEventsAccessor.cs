namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
	/// <summary>
    /// Provides access to domain events that have been raised by aggregate roots within the current unit of work.
    /// This interface allows the infrastructure layer to retrieve and manage domain events before they are dispatched.
    /// </summary>
	public interface IDomainEventsAccessor
	{
		/// <summary>
        /// Retrieves all domain events that have been raised by aggregate roots in the current unit of work.
        /// These events are collected from all modified aggregate roots and are ready to be dispatched.
        /// </summary>
        /// <returns>A read-only collection of domain events that need to be processed.</returns>
		IReadOnlyCollection<IDomainEvent> GetAllDomainEvents();

		/// <summary>
        /// Clears all domain events from the current unit of work.
        /// This method is typically called after domain events have been successfully dispatched
        /// to prevent duplicate event processing.
        /// </summary>
		void ClearAllDomainEvents();
	}
}
