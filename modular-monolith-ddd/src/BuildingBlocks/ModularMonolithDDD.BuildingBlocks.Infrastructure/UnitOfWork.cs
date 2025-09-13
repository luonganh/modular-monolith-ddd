namespace ModularMonolithDDD.BuildingBlocks.Infrastructure
{
	/// <summary>
	/// Represents a unit of work that handles database transactions and domain events.
	/// It commits changes to the database and dispatches domain events within a single transactional scope.
	/// Event handlers can participate in the same transaction and apply additional changes before persistence.
	/// If a handler fails, the CommitAsync rolls back, ensuring no data is written to the database and consistency is preserved.
	/// </summary>
	public class UnitOfWork : IUnitOfWork
    {
		// The DbContext used to manage database interactions
		private readonly DbContext _context;

		// The service responsible for dispatching domain events
		private readonly IDomainEventsDispatcher _domainEventsDispatcher;

		/// <summary>
		/// Initializes a new instance of the <see cref="UnitOfWork"/> class.
		/// </summary>
		/// <param name="context">The DbContext used for database operations.</param>
		/// <param name="domainEventsDispatcher">The service responsible for dispatching domain events.</param>
		public UnitOfWork(
            DbContext context,
            IDomainEventsDispatcher domainEventsDispatcher)
        {
			_context = context;
			_domainEventsDispatcher = domainEventsDispatcher;
        }

		/// <summary>
		/// Commits the changes in the unit of work, dispatches any domain events, and saves the changes to the database.
		/// </summary>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
		/// <param name="internalCommandId">An optional identifier for the internal command related to this commit.</param>
		/// <returns>The number of state entries written to the database.</returns>
		public async Task<int> CommitAsync(
            CancellationToken cancellationToken = default,
            Guid? internalCommandId = null)
        {
			// Dispatch any domain events before committing changes
			await _domainEventsDispatcher.DispatchEventsAsync();

			// Commit changes to the database
			return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}