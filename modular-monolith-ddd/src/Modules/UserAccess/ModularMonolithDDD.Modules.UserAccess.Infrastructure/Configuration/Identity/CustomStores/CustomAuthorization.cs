namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity.CustomStores
{
    // Minimal POCOs used by the custom stores. These are NOT EF entities.
       
    /// <summary>
    /// OAuth/OIDC Authorization (Grant).
    /// One authorization can have many tokens.
    /// Server (issuer) issues authorization record between user and application (client) that containes the scopes.   
    /// Consent: The agreement of the user to grant the scopes to the application (client).
    /// This class represents an OAuth/OIDC authorization registered in the identity system.
    /// It contains information about the authorization, such as its ID, application ID, subject, status, type, scopes, properties, creation date, and update date.
    /// It also contains navigation properties for EF relationships to the CustomApplication and CustomToken classes.
    /// </summary>
    public sealed class CustomAuthorization
    {
        /// <summary>Database primary key.</summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Client (Application Id (not ClientId) of Registered application).   
        /// In fact, ApplicationId is always not null. 
        /// </summary>
        public Guid? ApplicationId { get; set; }

        /// <summary>   
        /// The user identifier (User Id) the grant belongs to, ~"sub" claim in the JWT token.
        /// It links the grant and issued tokens to the correct user; used for user-based lookups. 
        /// It may be null in the client-credentials flow (no user).
        /// </summary>
        public string Subject { get; set; } = default!;

        /// <summary>
        /// Status of the authorization grant (whether it can be used to issue tokens).
        /// For authorization grants, typically only valid/revoked are used.
        /// valid: authorization grant is active and can be used to issue tokens.
        /// revoked: authorization grant has been revoked and cannot be used to issue new tokens; existing tokens may still be valid until expiry.
        /// </summary>
        public string Status { get; set; } = default!;

        /// <summary>
        /// The type of authorization grant (permanent or ad-hoc).
        /// permanent: 
        /// long-lived grant that can be used multiple times to issue tokens (typically for refresh token flow).
        /// Authorization grant is permanent and can be used to issue tokens indefinitely.
        /// 
        /// ad-hoc: 
        /// Temporary grant used only once (typically for authorization code flow).
        /// Authorization grant is ad-hoc and can be used to issue tokens for a limited time.
        /// </summary>
        public string Type { get; set; } = default!;

        /// <summary>
        /// JSON Array of scope list granted in the authorization grant.
        /// Common scopes:
        /// "openid": OpenID Connect scope
        /// "profile": basic profile information
        /// "email": email address
        /// "offline_access": refresh token
        /// "modular-monolith-ddd-api": custom API scope
        /// e.g, ["openid", "profile", "email", "offline_access", "modular-monolith-ddd-api"]
        /// </summary>
        public string? ScopesJson { get; set; }

        /// <summary>
        /// JSON object of additional serialized properties/metadata (for the authorization grant).         
        /// Properties contains:
        /// Session information: {"session_id": "abc123", "ip_address": "192.168.1.1"}
        /// Custom claims: {"tenant_id": "tenant1", "role": "admin"}
        /// Audit info: {"created_by": "admin", "source": "web_app"}
        /// Token metadata: {"nonce": "xyz789", "code_challenge": "abc123"}
        /// Used for tracking, audit, or storing supplementary information not present in the JWT payload.       
        /// </summary>
        public string? Properties { get; set; }

        /// <summary>Logical creation timestamp (UTC) of the authorization/grant.</summary>
        public DateTime? CreationDate { get; set; }

        /// <summary>Row creation timestamp (UTC).</summary>
        public DateTime CreatedAtUtc { get; set; }

        /// <summary>Row last update timestamp (UTC), if any.</summary>
        public DateTime? UpdatedAtUtc { get; set; }

        // Navigation properties for EF relationships

        // many-to-1 (optional) with Application
        // Many authorizations can belong to 0 or 1 application.
        // In fact, Authorization always belongs to an Application.
        public CustomApplication? Application { get; set; }
        
    }
}