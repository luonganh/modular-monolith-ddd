using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity.Stores;

namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Persistence.EF.Configurations.OpenIddict
{
    /// <summary>
    /// Entity Framework configuration for CustomToken entity.
    /// Maps the CustomToken POCO to the OpenIddictTokens table in the users schema.
    /// This allows EF Core to query OpenIddict token data for admin purposes.
    /// </summary>
    internal sealed class CustomTokenEntityTypeConfiguration : IEntityTypeConfiguration<CustomToken>
    {
        public void Configure(EntityTypeBuilder<CustomToken> builder)
        {
            builder.ToTable("OpenIddictTokens", "users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ReferenceId).HasMaxLength(100);
            builder.Property(x => x.Subject).HasMaxLength(400);
        }
    }
}
