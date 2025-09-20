using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity.Stores;

namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Persistence.EF.Configurations.OpenIddict
{
    /// <summary>
    /// Entity Framework configuration for CustomScope entity.
    /// Maps the CustomScope POCO to the OpenIddictScopes table in the users schema.
    /// This allows EF Core to query OpenIddict scope data for admin purposes.
    /// </summary>
    internal sealed class CustomScopeEntityTypeConfiguration : IEntityTypeConfiguration<CustomScope>
    {
        public void Configure(EntityTypeBuilder<CustomScope> builder)
        {
            builder.ToTable("OpenIddictScopes", "users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(200);
            builder.Property(x => x.DisplayName).HasMaxLength(200);
        }
    }
}
