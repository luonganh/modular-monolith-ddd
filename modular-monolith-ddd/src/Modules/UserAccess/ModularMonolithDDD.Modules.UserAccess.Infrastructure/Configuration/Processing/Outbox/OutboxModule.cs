namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Processing.Outbox
{
	/// <summary>
	/// Autofac module for configuring outbox pattern dependencies in the UserAccess module.
	/// This module registers the outbox accessor and domain notifications mapper,
	/// and validates that all domain event notifications are properly mapped.
	/// </summary>
	internal class OutboxModule : Module
	{
		private readonly BiDictionary<string, Type> _domainNotificationsMap;

		/// <summary>
		/// Initializes a new instance of the OutboxModule.
		/// </summary>
		/// <param name="domainNotificationsMap">Bidirectional dictionary mapping domain event notification types to their string names.</param>
		public OutboxModule(BiDictionary<string, Type> domainNotificationsMap)
		{
			_domainNotificationsMap = domainNotificationsMap;
		}

		/// <summary>
		/// Loads the outbox module configuration into the Autofac container.
		/// Registers the outbox accessor and domain notifications mapper with proper lifetime scopes.
		/// </summary>
		/// <param name="builder">The Autofac container builder.</param>
		protected override void Load(ContainerBuilder builder)
		{
			// Register outbox accessor for reliable message delivery
			builder.RegisterType<OutboxAccessor>()
				.As<IOutbox>()
				.FindConstructorsWith(new AllConstructorFinder())
				.InstancePerLifetimeScope();

			// Validate that all domain event notifications are properly mapped
			CheckMappings();

			// Register domain notifications mapper for type resolution
			builder.RegisterType<DomainNotificationsMapper>()
				.As<IDomainNotificationsMapper>()
				.FindConstructorsWith(new AllConstructorFinder())
				.WithParameter("domainNotificationsMap", _domainNotificationsMap)
				.SingleInstance();
		}

		/// <summary>
		/// Validates that all domain event notifications in the application assembly are properly mapped.
		/// This ensures that all domain events can be correctly deserialized and processed.
		/// </summary>
		/// <exception cref="ApplicationException">Thrown when one or more domain event notifications are not mapped.</exception>
		private void CheckMappings()
		{
			// Get all types that implement IDomainEventNotification from the application assembly
			var domainEventNotifications = Assemblies.Application
				.GetTypes()
				.Where(x => x.GetInterfaces().Contains(typeof(IDomainEventNotification)))
				.ToList();

			// Check which notifications are not mapped in the dictionary
			List<Type> notMappedNotifications = [];
			foreach (var domainEventNotification in domainEventNotifications)
			{
				_domainNotificationsMap.TryGetBySecond(domainEventNotification, out var name);

				if (name == null)
				{
					notMappedNotifications.Add(domainEventNotification);
				}
			}

			// Throw exception if any notifications are not mapped
			if (notMappedNotifications.Any())
			{
				throw new ApplicationException($"Domain Event Notifications {notMappedNotifications.Select(x => x.FullName).Aggregate((x, y) => x + "," + y)} not mapped");
			}
		}
	}
}