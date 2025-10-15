namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity.CustomStores
{
    // Minimal POCOs used by the custom stores. These are NOT EF entities.
    
    /// <summary>
    /// OAuth/OIDC Scope (Access to the resources/API).  
    /// Used to be included in tokens via authorizations/consent.
    /// This class represents an OAuth/OIDC scope registered in the identity system.
    /// It contains information about the scope, such as its ID, name, display name, description, resources, creation date, and update date.
    /// It also contains navigation properties for EF relationships to the CustomAuthorization and CustomToken classes.
    /// </summary>   
    public sealed class CustomScope
    {
        /// <summary>Database primary key.</summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Ex: "openid", "profile", "email", "offline_access", "modular-monolith-ddd-api"
        /// </summary>
        /// <summary>Unique scope name.</summary>
        public string Name { get; set; } = default!;
        /// <summary>Human-readable title for consent screens.</summary>
        public string? DisplayName { get; set; }
        /// <summary>Explanation of what access the scope provides.</summary>
        public string? Description { get; set; }
        /// <summary>
        /// JSON Array of API resources that this scope grants access to.
        /// Used to map the scope to specific API resources.
        /// When a token has this scope, it can access the listed API resources.
        /// e.g, ["api1", "api2", "api3"]
        /// </summary>
        public string? ResourcesJson { get; set; }
        /// <summary>Row creation timestamp in UTC.</summary>
        public DateTime CreatedAtUtc { get; set; }
        /// <summary>Row last update timestamp in UTC, if any.</summary>
        public DateTime? UpdatedAtUtc { get; set; }
    }
}