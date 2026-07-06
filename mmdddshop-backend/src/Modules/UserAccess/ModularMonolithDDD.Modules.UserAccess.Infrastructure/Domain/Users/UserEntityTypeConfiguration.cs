namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Domain.Users
{
    /// <summary>
    /// Entity Framework configuration for the User entity.
    /// Defines the database schema mapping for User domain objects.
    /// </summary>
    internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        /// <summary>
        /// Configures the User entity mapping to the database schema.
        /// Maps domain properties to database columns and defines relationships.
        /// </summary>
        /// <param name="builder">The entity type builder for configuring the User entity.</param>
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Map to Users table in the users schema
            builder.ToTable("Users", "users");

            // Configure primary key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,            // to provider (Guid)
                value => new UserId(value) // from provider
            )
            .ValueGeneratedNever()        // manual set Guid
            .HasColumnName("Id");

            // Map private fields to database columns
            // Using string literal for private field names as they are not accessible directly
            builder.Property<string>("_login").HasColumnName("Login");
            builder.Property<string>("_email").HasColumnName("Email");
            builder.Property<string>("_password").HasColumnName("Password");
            builder.Property<bool>("_isActive").HasColumnName("IsActive");
            builder.Property<string>("_firstName").HasColumnName("FirstName");
            builder.Property<string>("_lastName").HasColumnName("LastName");
            builder.Property<string>("_name").HasColumnName("Name");
            builder.Property<string>("_externalId").HasColumnName("ExternalId");
            builder.HasIndex("_externalId").IsUnique();

            // Configure owned entity for user roles
            // UserRole is an owned entity that represents a collection of roles for a user
            builder.OwnsMany<UserRole>("_roles", b =>
            {
                b.WithOwner().HasForeignKey("UserId");
                b.ToTable("UserRoles", "users");
                b.Property<UserId>("UserId");
                b.Property<string>("Value").HasColumnName("RoleCode");
                b.HasKey("UserId", "Value");
            });
        }
    }
}
