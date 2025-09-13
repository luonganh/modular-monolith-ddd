namespace ModularMonolithDDD.BuildingBlocks.Infrastructure
{	
	/// <summary>
    /// Defines the contract for a Unit of Work pattern implementation that manages
    /// database transactions and ensures data consistency across multiple operations.
    /// This interface provides a way to commit changes atomically and handle
    /// internal command processing within the modular monolith architecture.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Commits all pending changes to the database within a single transaction.
        /// This method ensures that all changes made within the current unit of work
        /// are persisted atomically, maintaining data consistency across the system.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to cancel the operation</param>
        /// <param name="internalCommandId">Optional internal command ID for tracking and correlation purposes</param>
        /// <returns>A task that represents the asynchronous commit operation, returning the number of affected records</returns>
        Task<int> CommitAsync(
            CancellationToken cancellationToken = default,
            Guid? internalCommandId = null);
    }
}
