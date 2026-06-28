namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.InternalCommands
{
    /// <summary>
    /// Entity Framework configuration for the InternalCommand entity.
    /// Defines the database schema mapping for internal command processing.
    /// </summary>
    internal class InternalCommandEntityTypeConfiguration : IEntityTypeConfiguration<InternalCommand>
    {
        /// <summary>
        /// Configures the InternalCommand entity mapping to the database schema.
        /// Maps the internal command properties to database columns.
        /// </summary>
        /// <param name="builder">The entity type builder for configuring the InternalCommand entity.</param>
        public void Configure(EntityTypeBuilder<InternalCommand> builder)
        {
            // Map to InternalCommands table in the users schema
            builder.ToTable("InternalCommands", "users");

            // Configure primary key
            builder.HasKey(b => b.Id);
            
            // Configure ID to not be auto-generated
            // Internal commands typically have pre-defined IDs for processing
            builder.Property(b => b.Id).ValueGeneratedNever();
        }
    }
}