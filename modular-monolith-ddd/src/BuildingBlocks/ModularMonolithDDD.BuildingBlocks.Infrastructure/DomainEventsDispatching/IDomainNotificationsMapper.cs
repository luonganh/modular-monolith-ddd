namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
    /// <summary>
    /// Defines the contract for mapping between domain event notification types and their string representations.
    /// This interface is used to convert between strongly-typed domain event notifications and their names
    /// for serialization, storage, and external system communication purposes.
    /// </summary>
    public interface IDomainNotificationsMapper
    {
        /// <summary>
        /// Gets the string name representation of a domain event notification type.
        /// This method is typically used when serializing domain event notifications to external systems
        /// or storing them in the Outbox pattern.
        /// </summary>
        /// <param name="type">The domain event notification type to get the name for.</param>
        /// <returns>The string name representation of the type.</returns>
        string GetName(Type type);

        /// <summary>
        /// Gets the strongly-typed domain event notification type from its string name representation.
        /// This method is typically used when deserializing domain event notifications from external systems
        /// or reading them from the Outbox pattern.
        /// </summary>
        /// <param name="name">The string name representation of the type.</param>
        /// <returns>The strongly-typed domain event notification type.</returns>
        Type GetType(string name);
    }
}
