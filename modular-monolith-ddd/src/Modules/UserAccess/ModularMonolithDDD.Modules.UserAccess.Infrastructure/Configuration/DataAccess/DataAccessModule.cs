namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.DataAccess
{
    /// <summary>
    /// Autofac module for configuring data access dependencies in the UserAccess module.
    /// Registers database context, connection factory, and repository implementations.
    /// </summary>
    internal class DataAccessModule : Module
    {
        private readonly string _databaseConnectionString;
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Initializes a new instance of the DataAccessModule.
        /// </summary>
        /// <param name="databaseConnectionString">The database connection string for SQL Server.</param>
        /// <param name="loggerFactory">Factory for creating loggers.</param>
        internal DataAccessModule(string databaseConnectionString, ILoggerFactory loggerFactory)
        {
            _databaseConnectionString = databaseConnectionString;
            _loggerFactory = loggerFactory;
        }

        /// <summary>
        /// Loads the data access module configuration into the Autofac container.
        /// Registers SQL connection factory, Entity Framework context, and repository implementations.
        /// </summary>
        /// <param name="builder">The Autofac container builder.</param>
        protected override void Load(ContainerBuilder builder)
        {
            // Register SQL connection factory for database connections
            builder.RegisterType<SqlConnectionFactory>()
                .As<ISqlConnectionFactory>()
                .WithParameter("connectionString", _databaseConnectionString)
                .InstancePerLifetimeScope();

            // Register Entity Framework DbContext with SQL Server configuration
            builder
                .Register(c =>
                {
                    var dbContextOptionsBuilder = new DbContextOptionsBuilder<UserAccessContext>();
                    dbContextOptionsBuilder.UseSqlServer(_databaseConnectionString);

                    // Replace default value converter selector with custom one for strongly-typed IDs
                    dbContextOptionsBuilder
                        .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();

                    return new UserAccessContext(dbContextOptionsBuilder.Options, _loggerFactory);
                })
                .AsSelf()
                .As<DbContext>()
                .InstancePerLifetimeScope();

            // Get the infrastructure assembly for repository registration
            var infrastructureAssembly = typeof(UserAccessContext).Assembly;

            // Auto-register all repository implementations
            builder.RegisterAssemblyTypes(infrastructureAssembly)
                .Where(type => type.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .FindConstructorsWith(new AllConstructorFinder());
        }
    }
}