namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Mediation
{
    /// <summary>
    /// Autofac module for configuring MediatR dependencies in the UserAccess module.
    /// This module registers MediatR services, command handlers, query handlers, notification handlers,
    /// validators, and pipeline behaviors with proper lifetime scopes and decorator support.
    /// </summary>
	public class MediatorModule : Module
	{
        /// <summary>
        /// Loads the MediatR configuration into the Autofac container.
        /// Registers MediatR services, handlers, validators, and pipeline behaviors
        /// with appropriate lifetime scopes and decorator support.
        /// </summary>
        /// <param name="builder">The Autofac container builder.</param>
		protected override void Load(ContainerBuilder builder)
		{
			// Register service provider wrapper for MediatR dependency resolution
			builder.RegisterType<ServiceProviderWrapper>()
			.As<IServiceProvider>()
			.InstancePerDependency()
			.IfNotRegistered(typeof(IServiceProvider));

			// Register all MediatR core services from the MediatR assembly
			builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
				.AsImplementedInterfaces()
				.InstancePerLifetimeScope();

			// Define all MediatR handler types that need to be registered
			var mediatorOpenTypes = new[]
			{
				typeof(IRequestHandler<,>),           // Query and command handlers with return values
				typeof(INotificationHandler<>),       // Domain event notification handlers
				typeof(IValidator<>),                 // FluentValidation validators
				typeof(IRequestPreProcessor<>),       // Request pre-processing behaviors
				typeof(IRequestHandler<>),            // Command handlers without return values
				typeof(IStreamRequestHandler<,>),     // Stream request handlers
				typeof(IRequestPostProcessor<,>),     // Request post-processing behaviors
				typeof(IRequestExceptionHandler<,,>), // Exception handling behaviors
				typeof(IRequestExceptionAction<,>),   // Exception action behaviors
				typeof(ICommandHandler<>),            // Custom command handlers without return values
				typeof(ICommandHandler<,>),           // Custom command handlers with return values
			};

			// Register contravariant registration source for generic type support
			builder.RegisterSource(new ScopedContravariantRegistrationSource(
				mediatorOpenTypes));

			// Register all handler implementations from application and infrastructure assemblies
			foreach (var mediatorOpenType in mediatorOpenTypes)
			{
				builder
					.RegisterAssemblyTypes(Assemblies.Application, ThisAssembly)
					.AsClosedTypesOf(mediatorOpenType)
					.AsImplementedInterfaces()
					.FindConstructorsWith(new AllConstructorFinder());
			}

			// Register MediatR pipeline behaviors for cross-cutting concerns
			builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
			builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
		}

		/// <summary>
		/// Custom registration source that provides scoped contravariant registration support for MediatR handlers.
		/// This class filters and provides registrations only for the specified generic type definitions,
		/// enabling proper dependency injection for generic handler types.
		/// </summary>
		private class ScopedContravariantRegistrationSource : IRegistrationSource
		{
			private readonly ContravariantRegistrationSource _source = new();
			private readonly List<Type> _types = new();

			/// <summary>
			/// Initializes a new instance of the ScopedContravariantRegistrationSource class.
			/// </summary>
			/// <param name="types">Array of generic type definitions to support for contravariant registration.</param>
			/// <exception cref="ArgumentNullException">Thrown when types parameter is null.</exception>
			/// <exception cref="ArgumentException">Thrown when any of the supplied types is not a generic type definition.</exception>
			public ScopedContravariantRegistrationSource(params Type[] types)
			{
				ArgumentNullException.ThrowIfNull(types);

				if (!types.All(x => x.IsGenericTypeDefinition))
				{
					throw new ArgumentException("Supplied types should be generic type definitions");
				}

				_types.AddRange(types);
			}

			/// <summary>
			/// Provides component registrations for the specified service.
			/// Filters registrations to only include those that match the configured generic type definitions.
			/// </summary>
			/// <param name="service">The service to get registrations for.</param>
			/// <param name="registrationAccessor">Function to access service registrations.</param>
			/// <returns>Filtered component registrations that match the configured types.</returns>
			public IEnumerable<IComponentRegistration> RegistrationsFor(
				Service service,
				Func<Service, IEnumerable<ServiceRegistration>> registrationAccessor)
			{
				var components = _source.RegistrationsFor(service, registrationAccessor);
				foreach (var c in components)
				{
					var defs = c.Target.Services
						.OfType<TypedService>()
						.Select(x => x.ServiceType.GetGenericTypeDefinition());

					if (defs.Any(_types.Contains))
					{
						yield return c;
					}
				}
			}

			/// <summary>
			/// Gets a value indicating whether this registration source is an adapter for individual components.
			/// </summary>
			public bool IsAdapterForIndividualComponents => _source.IsAdapterForIndividualComponents;
		}
	}
}
