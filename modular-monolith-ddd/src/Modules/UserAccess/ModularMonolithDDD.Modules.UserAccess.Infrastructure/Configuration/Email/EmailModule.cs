namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Email
{
	/// <summary>
	/// Dependency injection module for email services configuration.
	/// This module handles the registration of email-related services in the DI container,
	/// providing flexible configuration for email sending capabilities within the UserAccess module.
	/// </summary>
	internal class EmailModule : Module
	{
		private readonly IEmailSender _emailSender;
		private readonly EmailsConfiguration _configuration;

		/// <summary>
		/// Initializes a new instance of the EmailModule class.
		/// This constructor sets up the email configuration and sender dependencies
		/// required for email service registration.
		/// </summary>
		/// <param name="configuration">The email configuration containing sender settings and parameters</param>
		/// <param name="emailSender">Optional email sender instance for dependency injection</param>
		public EmailModule(
			EmailsConfiguration configuration,
			IEmailSender emailSender)
		{
			_configuration = configuration;
			_emailSender = emailSender;
		}

		/// <summary>
		/// Registers email services in the dependency injection container.
		/// This method configures the email sender service registration, either using
		/// a provided instance or creating a new instance with the specified configuration.
		/// </summary>
		/// <param name="builder">The container builder used for service registration</param>
		protected override void Load(ContainerBuilder builder)
		{
			if (_emailSender != null)
			{
				// Register the provided email sender instance
				builder.RegisterInstance(_emailSender);
			}
			else
			{
				// Register a new EmailSender instance with configuration
				builder.RegisterType<EmailSender>()
					.As<IEmailSender>()
					.WithParameter("configuration", _configuration)
					.InstancePerLifetimeScope();
			}
		}
	}
}