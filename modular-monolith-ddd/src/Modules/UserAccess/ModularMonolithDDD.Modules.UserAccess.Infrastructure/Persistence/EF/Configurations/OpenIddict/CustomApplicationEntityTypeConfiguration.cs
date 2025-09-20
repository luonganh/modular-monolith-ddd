using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity.Stores;

namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Persistence.EF.Configurations.OpenIddict
{
    /// <summary>
    /// Entity Framework configuration for CustomApplication entity.
    /// Maps the CustomApplication POCO to the OpenIddictApplications table in the users schema.
    /// This allows EF Core to query OpenIddict data for admin purposes while using Dapper for runtime.
    /// </summary>
    internal sealed class CustomApplicationEntityTypeConfiguration : IEntityTypeConfiguration<CustomApplication>
    {
        public void Configure(EntityTypeBuilder<CustomApplication> builder)
        {
            builder.ToTable("OpenIddictApplications", "users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ClientId).HasMaxLength(100);
            builder.Property(x => x.ClientSecret).HasMaxLength(200);
            builder.Property(x => x.DisplayName).HasMaxLength(200);
            builder.HasMany(x => x.Authorizations).WithOne().HasForeignKey(x => x.ApplicationId);
            builder.HasMany(x => x.Tokens).WithOne().HasForeignKey(x => x.ApplicationId);
        }
    }
}
