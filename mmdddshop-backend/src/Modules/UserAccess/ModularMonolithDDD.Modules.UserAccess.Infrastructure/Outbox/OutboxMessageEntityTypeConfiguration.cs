namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Outbox
{
	/// <summary>
	/// Entity Framework configuration for the OutboxMessage entity.
	/// Defines the database schema mapping for outbox message storage.
	/// </summary>
	internal class OutboxMessageEntityTypeConfiguration : IEntityTypeConfiguration<OutboxMessage>
	{
		/// <summary>
		/// Configures the OutboxMessage entity mapping to the database schema.
		/// Maps the outbox message properties to database columns.
		/// </summary>
		/// <param name="builder">The entity type builder for configuring the OutboxMessage entity.</param>
		public void Configure(EntityTypeBuilder<OutboxMessage> builder)
		{
			// Map to OutboxMessages table in the users schema
			builder.ToTable("OutboxMessages", "users");

			// Configure primary key
			builder.HasKey(b => b.Id);
			
			// Configure ID to not be auto-generated
			// Outbox messages typically have pre-defined IDs for reliable processing
			builder.Property(b => b.Id).ValueGeneratedNever();
		}
	}
}
