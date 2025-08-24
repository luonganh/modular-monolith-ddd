namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
	/// <summary>
	/// The <see cref="DomainEventsAccessor"/> class is responsible for retrieving and clearing domain events 
	/// associated with entities tracked by the Entity Framework DbContext.
	/// </summary>
	public class DomainEventsAccessor : IDomainEventsAccessor
	{
		private readonly DbContext _dbContext;

		/// <summary>
		/// Initializes a new instance of the <see cref="DomainEventsAccessor"/> class.
		/// </summary>
		/// <param name="dbContext">The Entity Framework DbContext used to access the database.</param>
		public DomainEventsAccessor(DbContext dbContext)
		{
			_dbContext = dbContext;
		}

		/// <summary>
		/// Retrieves all domain events that are attached to entities tracked by the DbContext.
		/// <para>
		/// This method checks the change tracker for all entities of type <see cref="Entity"/> that have domain events 
		/// associated with them. It returns all of those domain events as a read-only collection.
		/// </para>
		/// </summary>
		/// <returns>A read-only collection of all domain events associated with entities tracked by the DbContext.</returns>
		public IReadOnlyCollection<IDomainEvent> GetAllDomainEvents()
		{
			// Get all entities tracked by the DbContext that have domain events
			var domainEntities = this._dbContext.ChangeTracker
				.Entries<Entity>()
				.Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

			// Flatten and return the list of domain events from those entities
			return domainEntities
				.SelectMany(x => x.Entity.DomainEvents)
				.ToList();
		}

		/// <summary>
		/// Clears all domain events associated with entities tracked by the DbContext.
		/// <para>
		/// This method iterates through all entities with domain events and calls <see cref="Entity.ClearDomainEvents()"/> 
		/// to clear the domain events for each entity.
		/// </para>
		/// </summary>
		public void ClearAllDomainEvents()
		{
			// Get all entities tracked by the DbContext that have domain events
			var domainEntities = this._dbContext.ChangeTracker
				.Entries<Entity>()
				.Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

			// Clear the domain events for each entity
			domainEntities
				.ForEach(entity => entity.Entity.ClearDomainEvents());
		}
	}
}
