namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
    /// <summary>
    /// Defines the contract for dispatching domain events that have been raised by aggregate roots.
    /// This interface is responsible for publishing domain events to their respective event handlers
    /// after the unit of work has been successfully committed.
    /// </summary>
    public interface IDomainEventsDispatcher
    {
        /// <summary>
        /// Dispatches all pending domain events to their registered event handlers asynchronously.
        /// This method should be called after the unit of work has been successfully committed
        /// to ensure that domain events are only published when the business operation is complete.
        /// </summary>
        /// <returns>A task representing the asynchronous operation of dispatching domain events.</returns>
        Task DispatchEventsAsync();
    }
}
