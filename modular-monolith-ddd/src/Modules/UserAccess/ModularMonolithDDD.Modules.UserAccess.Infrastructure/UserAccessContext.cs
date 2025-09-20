using OpenIddict.EntityFrameworkCore.Models;
using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity.Stores;

namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure
{
    /// <summary>
    /// Entity Framework DbContext for the UserAccess module.
    /// Provides database access for users, outbox messages, and internal commands.
    /// Implements IDomainDbContext to integrate with the domain layer.
    /// </summary>
    public class UserAccessContext : DbContext, IDomainDbContext
	{
		/// <summary>
		/// DbSet for User entities. Provides access to the Users table in the database.
		/// </summary>
		public DbSet<User> Users { get; set; }
        
        /// <summary>
        /// DbSet for OutboxMessage entities. Used for the outbox pattern to ensure reliable message delivery.
        /// </summary>
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        /// <summary>
        /// DbSet for InternalCommand entities. Used for internal command processing within the module.
        /// </summary>
        public DbSet<InternalCommand> InternalCommands { get; set; }

        // OpenIddict DbSets for admin queries (using EF Core alongside Dapper stores)
        /// <summary>
        /// DbSet for OpenIddict Applications. Used for admin queries and reporting.
        /// Note: OpenIddict runtime uses Dapper stores, this is for EF-based admin operations.
        /// </summary>
        public DbSet<CustomApplication> OpenIddictApplications { get; set; }

        /// <summary>
        /// DbSet for OpenIddict Authorizations. Used for admin queries and reporting.
        /// </summary>
        public DbSet<CustomAuthorization> OpenIddictAuthorizations { get; set; }

        /// <summary>
        /// DbSet for OpenIddict Scopes. Used for admin queries and reporting.
        /// </summary>
        public DbSet<CustomScope> OpenIddictScopes { get; set; }

        /// <summary>
        /// DbSet for OpenIddict Tokens. Used for admin queries and reporting.
        /// </summary>
        public DbSet<CustomToken> OpenIddictTokens { get; set; }

       
        private readonly ILoggerFactory _loggerFactory;

		/// <summary>
		/// Initializes a new instance of the UserAccessContext.
		/// </summary>
		/// <param name="options">The DbContext options for configuring the context.</param>
		/// <param name="loggerFactory">Factory for creating loggers.</param>
		public UserAccessContext(DbContextOptions options, ILoggerFactory loggerFactory)
			: base(options)
		{
			_loggerFactory = loggerFactory;
		}

		/// <summary>
		/// Configures the entity models for the database schema.
		/// Applies entity type configurations for Users, OutboxMessages, and InternalCommands.
		/// </summary>
		/// <param name="modelBuilder">The model builder used to configure entity models.</param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new InternalCommandEntityTypeConfiguration());
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserAccessContext).Assembly);
        }
	}
}
