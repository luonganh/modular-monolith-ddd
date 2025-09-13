namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
    /// <summary>
    /// Implementation of IDomainEventsAccessor that retrieves domain events from Entity Framework's ChangeTracker.
    /// This class scans all tracked entities for domain events and provides methods to collect and clear them.
    /// </summary>
    public class DomainEventsAccessor : IDomainEventsAccessor
    {
        private readonly DbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the DomainEventsAccessor class.
        /// </summary>
        /// <param name="dbContext">The Entity Framework DbContext used to access tracked entities.</param>
        public DomainEventsAccessor(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves all domain events from entities currently being tracked by Entity Framework.
        /// This method scans the ChangeTracker for all Entity objects that have domain events
        /// and flattens them into a single collection.
        /// </summary>
        /// <returns>A read-only collection of all domain events from tracked entities.</returns>
        public IReadOnlyCollection<IDomainEvent> GetAllDomainEvents()
        {
            var domainEntities = this._dbContext.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

            return domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();
        }

        /// <summary>
        /// Clears all domain events from entities currently being tracked by Entity Framework.
        /// This method finds all tracked entities with domain events and calls ClearDomainEvents()
        /// on each entity to remove the events from their internal collection.
        /// </summary>
        public void ClearAllDomainEvents()
        {
            var domainEntities = this._dbContext.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

            domainEntities
                .ForEach(entity => entity.Entity.ClearDomainEvents());
        }
    }
}