namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration
{
    /// <summary>
    /// Composition root for the UserAccess module.
    /// Imeplementation of the Composition Root pattern to manage dependency injection.
    /// This class manages the dependency injection container and provides access to lifetime scopes
    /// for the UserAccess module's infrastructure components.
    /// Manage dependencies: Store container, provide scope to resolve dependencies.
    /// </summary>
    internal static class UserAccessCompositionRoot
    {
        /// <summary>
        /// The dependency injection container instance for the UserAccess module.
        /// </summary>
		private static IContainer _container;

        /// <summary>
        /// Sets the dependency injection container for the UserAccess module.
        /// This method should be called during application startup to initialize the container.
        /// </summary>
        /// <param name="container">The Autofac container instance to use for dependency injection.</param>
		internal static void SetContainer(IContainer container)
		{
			_container = container;
		}

        /// <summary>
        /// Begins a new lifetime scope for the UserAccess module.
        /// This creates a new scope that can be used to resolve dependencies with proper lifetime management.
        /// The scope should be disposed when no longer needed.
        /// </summary>
        /// <returns>A new lifetime scope instance.</returns>
		internal static ILifetimeScope BeginLifetimeScope()
		{
			return _container.BeginLifetimeScope();
		}
	}
}