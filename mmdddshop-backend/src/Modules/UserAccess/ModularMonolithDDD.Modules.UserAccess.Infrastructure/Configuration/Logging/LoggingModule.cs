namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Logging
{
    /// <summary>
    /// Autofac module responsible for configuring logging dependencies for the UserAccess module.
    /// This module registers the Serilog ILogger instance as a singleton in the dependency injection container.
    /// </summary>
    internal class LoggingModule : Module
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the LoggingModule class.
        /// </summary>
        /// <param name="logger">The Serilog logger instance to be registered in the DI container.</param>
        internal LoggingModule(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Loads the logging configuration into the Autofac container builder.
        /// Registers the logger instance as a singleton to ensure the same logger instance
        /// is used throughout the application lifecycle.
        /// </summary>
        /// <param name="builder">The Autofac container builder used for dependency registration.</param>
        protected override void Load(ContainerBuilder builder)
        {
            // Register the logger instance as a singleton in the DI container
            // This ensures that the same logger instance is shared across all components
            // that require ILogger dependency injection
            builder.RegisterInstance(_logger)
                .As<ILogger>()
                .SingleInstance();
        }
    }
}