namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
    /// <summary>
    /// Implementation of IDomainNotificationsMapper that uses a bidirectional dictionary for mapping
    /// between domain event notification types and their string representations.
    /// This class provides efficient bidirectional lookup for type-to-name and name-to-type conversions.
    /// </summary>
    public class DomainNotificationsMapper : IDomainNotificationsMapper
    {
        private readonly BiDictionary<string, Type> _domainNotificationsMap;

        /// <summary>
        /// Initializes a new instance of the DomainNotificationsMapper class.
        /// </summary>
        /// <param name="domainNotificationsMap">A bidirectional dictionary containing the mapping between string names and domain event notification types.</param>
        public DomainNotificationsMapper(BiDictionary<string, Type> domainNotificationsMap)
        {
            _domainNotificationsMap = domainNotificationsMap;
        }

        /// <summary>
        /// Gets the string name representation of a domain event notification type.
        /// Uses the bidirectional dictionary to perform efficient lookup by type.
        /// </summary>
        /// <param name="type">The domain event notification type to get the name for.</param>
        /// <returns>The string name representation of the type, or null if not found.</returns>
        public string GetName(Type type)
        {
            return _domainNotificationsMap.TryGetBySecond(type, out var name) ? name : null;
        }

        /// <summary>
        /// Gets the strongly-typed domain event notification type from its string name representation.
        /// Uses the bidirectional dictionary to perform efficient lookup by name.
        /// </summary>
        /// <param name="name">The string name representation of the type.</param>
        /// <returns>The strongly-typed domain event notification type, or null if not found.</returns>
        public Type GetType(string name)
        {
            return _domainNotificationsMap.TryGetByFirst(name, out var type) ? type : null;
        }
    }
}
