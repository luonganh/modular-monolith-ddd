namespace ModularMonolithDDD.BuildingBlocks.Infrastructure
{
	/// <summary>
	/// A wrapper around Autofac's <see cref="ILifetimeScope"/> to implement <see cref="IServiceProvider"/>.
	/// This allows resolving services within a specific lifetime scope, enabling integration with other dependency injection systems.
	/// </summary>
	public class ServiceProviderWrapper : IServiceProvider
    {
		// The Autofac lifetime scope used to resolve services
		private readonly ILifetimeScope lifeTimeScope;

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceProviderWrapper"/> class.
		/// </summary>
		/// <param name="lifeTimeScope">The Autofac lifetime scope used for resolving services.</param>
		public ServiceProviderWrapper(ILifetimeScope lifeTimeScope)
        {
            this.lifeTimeScope = lifeTimeScope;
        }

		/// <summary>
		/// Resolves a service of the specified type from the current lifetime scope.
		/// If the service is not registered, it returns null.
		/// </summary>
		/// <param name="serviceType">The type of the service to resolve.</param>
		/// <returns>The resolved service or null if the service is not registered.</returns>
#nullable enable
		public object? GetService(Type serviceType) => lifeTimeScope.ResolveOptional(serviceType);
    }
}
