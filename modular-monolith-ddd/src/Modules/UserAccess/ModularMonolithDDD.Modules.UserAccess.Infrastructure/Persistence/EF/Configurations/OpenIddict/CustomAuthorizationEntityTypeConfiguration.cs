using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity.Stores;

namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Persistence.EF.Configurations.OpenIddict
{
    /// <summary>
    /// Entity Framework configuration for CustomAuthorization entity.
    /// Maps the CustomAuthorization POCO to the OpenIddictAuthorizations table in the users schema.
    /// This allows EF Core to query OpenIddict authorization data for admin purposes.
    /// </summary>
    internal sealed class CustomAuthorizationEntityTypeConfiguration : IEntityTypeConfiguration<CustomAuthorization>
    {
        public void Configure(EntityTypeBuilder<CustomAuthorization> builder)
        {
            builder.ToTable("OpenIddictAuthorizations", "users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Status).HasMaxLength(50);
            builder.Property(x => x.Subject).HasMaxLength(400);
            builder.HasMany(x => x.Tokens).WithOne().HasForeignKey(x => x.AuthorizationId);
        }
    }
}
