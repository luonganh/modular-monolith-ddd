namespace ModularMonolithDDD.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
	/// <summary>
	/// The <see cref="DomainNotificationsMapper"/> class provides a mapping between domain event notification names 
	/// and their corresponding types, enabling the retrieval of a notification's name from its type, 
	/// and vice versa.
	/// </summary>
	public class DomainNotificationsMapper : IDomainNotificationsMapper
	{
		private readonly BiDictionary<string, Type> _domainNotificationsMap;

		/// <summary>
		/// Initializes a new instance of the <see cref="DomainNotificationsMapper"/> class with a specified 
		/// <see cref="BiDictionary{string, Type}"/> to store the mapping of notification names to types.
		/// </summary>
		/// <param name="domainNotificationsMap">A bi-directional dictionary that holds the mapping between 
		/// notification names (strings) and their corresponding types (Type).</param>
		public DomainNotificationsMapper(BiDictionary<string, Type> domainNotificationsMap)
		{
			_domainNotificationsMap = domainNotificationsMap;
		}

		/// <summary>
		/// Retrieves the name associated with a given notification type.
		/// </summary>
		/// <param name="type">The type of the domain event notification.</param>
		/// <returns>The name of the notification type, or <c>null</c> if no mapping is found.</returns>
		public string GetName(Type type)
		{
			return _domainNotificationsMap.TryGetBySecond(type, out var name) ? name : null;
		}

		/// <summary>
		/// Retrieves the type associated with a given notification name.
		/// </summary>
		/// <param name="name">The name of the domain event notification.</param>
		/// <returns>The type of the notification, or <c>null</c> if no mapping is found.</returns>
		public Type GetType(string name)
		{
			return _domainNotificationsMap.TryGetByFirst(name, out var type) ? type : null;
		}
	}
}
