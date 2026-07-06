namespace ModularMonolithDDD.BuildingBlocks.Infrastructure
{
	/// <summary>
    /// Wrapper class that adapts Autofac's ILifetimeScope to the standard IServiceProvider interface.
    /// This class enables the use of Autofac's dependency injection container in scenarios where
    /// the standard .NET service provider interface is expected, providing seamless integration
    /// between Autofac and other parts of the application that rely on IServiceProvider.
    /// </summary>
    public class ServiceProviderWrapper : IServiceProvider
    {
        private readonly ILifetimeScope lifeTimeScope;

        /// <summary>
        /// Initializes a new instance of the ServiceProviderWrapper class.
        /// This constructor sets up the wrapper with an Autofac lifetime scope
        /// that will be used to resolve services.
        /// </summary>
        /// <param name="lifeTimeScope">The Autofac lifetime scope to wrap</param>
        public ServiceProviderWrapper(ILifetimeScope lifeTimeScope)
        {
            this.lifeTimeScope = lifeTimeScope;
        }

        /// <summary>
        /// Gets a service of the specified type from the Autofac lifetime scope.
        /// This method attempts to resolve the service and returns null if the service
        /// is not registered, following the standard IServiceProvider pattern.
        /// </summary>
        /// <param name="serviceType">The type of service to resolve</param>
        /// <returns>The resolved service instance, or null if the service is not registered</returns>
        #nullable enable
        public object? GetService(Type serviceType) => this.lifeTimeScope.ResolveOptional(serviceType);
    }
}
