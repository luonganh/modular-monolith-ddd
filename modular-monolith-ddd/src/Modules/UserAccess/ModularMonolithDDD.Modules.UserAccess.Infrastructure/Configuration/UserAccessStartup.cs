using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity.Stores;

namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration
{
    /// <summary>
	/// Host (API) called UserAccessStartup - the entry point of the UserAccess module.
    /// Startup class responsible for initializing and configuring the UserAccess module.
    /// This class handles the complete setup of the module including dependency injection,
    /// background processing, event bus subscriptions, and all infrastructure components.
    /// Initialize module: Configure dependencies, services, database,start Quartz, Event Bus.
    /// </summary>
    public class UserAccessStartup
	{
		/// <summary>
		/// The dependency injection container instance for the UserAccess module.
		/// This container manages all registered services and their lifetimes.
		/// </summary>
		private static IContainer _container;

		/// <summary>
		/// Initializes the UserAccess module with all required dependencies and configurations.
		/// This method sets up the composition root, configures background processing with Quartz,
		/// and initializes the event bus system for integration events.
		/// </summary>
		/// <param name="connectionString">Database connection string for the UserAccess module.</param>
		/// <param name="executionContextAccessor">Service for accessing execution context information.</param>
		/// <param name="logger">Serilog logger instance for logging module operations.</param>
		/// <param name="emailsConfiguration">Configuration for email services including SMTP settings.</param>
		/// <param name="textEncryptionKey">Encryption key for securing sensitive text data.</param>
		/// <param name="emailSender">Service for sending emails (notifications, confirmations, etc.).</param>
		/// <param name="eventsBus">Event bus for publishing and subscribing to integration events.</param>
		/// <param name="internalProcessingPoolingInterval">Optional polling interval in milliseconds for internal command processing. If null, uses default cron schedule.</param>
		public static void Initialize(
			string connectionString,
			IExecutionContextAccessor executionContextAccessor,
			ILogger logger,
			EmailsConfiguration emailsConfiguration,
			string textEncryptionKey,
			IEmailSender emailSender,
			IEventsBus eventsBus,
			long? internalProcessingPoolingInterval = null)
		{
			// Create a module-specific logger with "UserAccess" context for better log filtering
			var moduleLogger = logger.ForContext("Module", "UserAccess");

			// Configure the dependency injection container with all required modules
			ConfigureCompositionRoot(
				connectionString,
				executionContextAccessor,
				logger,
				emailsConfiguration,
				textEncryptionKey,
				emailSender,
				eventsBus);

			// Initialize Quartz.NET scheduler for background job processing
			// This handles outbox messages, inbox messages, and internal commands
			QuartzStartup.Initialize(moduleLogger, internalProcessingPoolingInterval);

			// Initialize event bus subscriptions for integration events
			// This allows the module to respond to events from other modules
			EventsBusStartup.Initialize(moduleLogger);
		}

		/// <summary>
		/// Configures the dependency injection container for the UserAccess module.
		/// This method registers all required modules and services using Autofac,
		/// establishing the complete composition root for the module.
		/// </summary>
		/// <param name="connectionString">Database connection string for data access.</param>
		/// <param name="executionContextAccessor">Service for accessing execution context.</param>
		/// <param name="logger">Serilog logger instance.</param>
		/// <param name="emailsConfiguration">Email service configuration.</param>
		/// <param name="textEncryptionKey">Encryption key for text security.</param>
		/// <param name="emailSender">Email sending service.</param>
		/// <param name="eventsBus">Event bus for integration events.</param>
		private static void ConfigureCompositionRoot(
			string connectionString,
			IExecutionContextAccessor executionContextAccessor,
			ILogger logger,
			EmailsConfiguration emailsConfiguration,
			string textEncryptionKey,
			IEmailSender emailSender,
			IEventsBus eventsBus)
		{
			// Create a new Autofac container builder
			var containerBuilder = new ContainerBuilder();

			// Register logging module for structured logging with module context
			containerBuilder.RegisterModule(new LoggingModule(logger.ForContext("Module", "UserAccess")));

			// Register data access module - EF Core needs ILoggerFactory, not ILogger directly
			// Note: DataAccessModule requires ILoggerFactory (Microsoft.Extensions.Logging) for EF Core,
			// while LoggingModule registers ILogger (Serilog) for application code
			// SerilogLoggerFactory acts as an adapter to bridge Serilog with Microsoft.Extensions.Logging
			var loggerFactory = new Serilog.Extensions.Logging.SerilogLoggerFactory(logger);
			containerBuilder.RegisterModule(new DataAccessModule(connectionString, loggerFactory));

			// Register custom Openiddict stores module
            containerBuilder.RegisterModule(new CustomOpenIddictStoresModule());
            
			// Register processing module for command/query handling and validation
            containerBuilder.RegisterModule(new ProcessingModule());
			
			// Register event bus module for integration event handling
			containerBuilder.RegisterModule(new EventsBusModule(eventsBus));
			
			// Register MediatR module for CQRS pattern implementation
			containerBuilder.RegisterModule(new MediatorModule());
			
			// Register outbox module for reliable message delivery pattern
			containerBuilder.RegisterModule(new OutboxModule(new BiDictionary<string, Type>()));

			// Register Quartz module for background job scheduling
			containerBuilder.RegisterModule(new QuartzModule());
			
			// Register email module for notification and communication services
			containerBuilder.RegisterModule(new EmailModule(emailsConfiguration, emailSender));
			
			// Register security module for encryption and security services
			containerBuilder.RegisterModule(new SecurityModule(textEncryptionKey));

			// Register execution context accessor as a singleton instance
			containerBuilder.RegisterInstance(executionContextAccessor);

			// Build the container from the blueprint (containerBuilder) into a working DI container
			// This converts all registered services from "plans" into actual usable instances
			_container = containerBuilder.Build();

			// Store the container in UserAccessCompositionRoot so any class in the UserAccess module
			// can access it later using UserAccessCompositionRoot.BeginLifetimeScope()
			// This makes the container available throughout the entire module
			UserAccessCompositionRoot.SetContainer(_container);
		}

		/// <summary>
		/// Add a public proxy method for take scope from outside of boundary
		/// </summary>
		/// <returns></returns>
		public static ILifetimeScope BeginLifetimeScope() => _container.BeginLifetimeScope();
    }
}