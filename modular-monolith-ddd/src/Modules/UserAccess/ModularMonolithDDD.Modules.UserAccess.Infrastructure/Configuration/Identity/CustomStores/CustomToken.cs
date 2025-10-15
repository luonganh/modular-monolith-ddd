namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity.CustomStores
{
    // Minimal POCOs used by the custom stores. These are NOT EF entities.           

    /// <summary>
    /// Access token/refresh token/reference token. 
    /// One token can belongs to many applications, many authorizations.
    /// </summary>
    public sealed class CustomToken
    {
        /// <summary>Database primary key.</summary>
        public Guid Id { get; set; }

        /// <summary>FK to the client application that owns this token.</summary>
        public Guid? ApplicationId { get; set; }

        /// <summary>FK to the authorization grant this token is bound to.</summary>
        public Guid? AuthorizationId { get; set; }

        /// <summary>Token type (access, refresh, reference, etc.).
        /// The token kind; the server uses it to issue/validate/revoke/prune appropriately.
        /// Common values: access_token, refresh_token, authorization_code, device_code, user_code, reference_token.       
        /// </summary>
        public string Type { get; set; } = default!;

        /// <summary>
        /// The token’s lifecycle state, used for validation/revocation/pruning.       
        /// Common values (OpenIddict): valid, revoked, inactive, redeemed, rejected.       
        /// Valid: active and usable. Tokens can be validated; grants can issue new tokens.
        /// Revoked: explicitly cancelled. Cannot be used; existing tokens tied to it should be rejected or will expire soon.
        /// Inactive: created but not yet usable (e.g., pre-validated, pending activation).
        /// Redeemed: one-time artifact already used (e.g., authorization code, device/user code); cannot be reused.
        /// Rejected: refused due to policy/validation failure (e.g., invalid parameters, disallowed client).
        /// OpenIddict uses valid/revoked; others may have additional states.
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// The User identifier the token is about, matching the JWT sub claim (JWT "sub").       
        /// For client-credentials tokens, it may be null (no end-user).Null for client credentials grants.
        /// </summary>
        public string? Subject { get; set; }

        /// <summary>Lookup key for reference tokens.</summary>
        public string? ReferenceId { get; set; }

        /// <summary>       
        /// Encoded JWT string or JSON object of token payload.     
        /// OpenIddict typically stores JWT directly in the Payload -> Jwt String.
        /// JSON object is only used when storing complex metadata or reference token data.
        /// e.g, "eyJh..." or {"access_token": "...", "token_type": "Bearer"}
        /// </summary>
        public string? Payload { get; set; }

        /// <summary>
        /// JSON object of additional serialized properties/metadata (for the token).
        /// Properties contains:
        /// Session information: {"session_id": "abc123", "ip_address": "192.168.1.1"}
        /// Custom claims: {"tenant_id": "tenant1", "role": "admin"}
        /// Audit info: {"created_by": "admin", "source": "web_app"}
        /// Token metadata: {"nonce": "xyz789", "code_challenge": "abc123"}
        /// Used for tracking, audit, or storing supplementary information not present in the JWT payload.       
        /// </summary>
        public string? Properties { get; set; }

        /// <summary>Issued-at timestamp (UTC).</summary>
        public DateTime? CreationDate { get; set; }

        /// <summary>Expiration timestamp (UTC).</summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>Consumption timestamp (UTC) for one-time tokens.</summary>
        public DateTime? RedemptionDate { get; set; }

        /// <summary>Row creation timestamp (UTC).</summary>
        public DateTime CreatedAtUtc { get; set; }

        /// <summary>Row last update timestamp (UTC), if any.</summary>
        public DateTime? UpdatedAtUtc { get; set; }

        // Navigation properties for EF relationships

        // many-to-1 (optional) with Application
        // Many tokens can belong to 0 or 1 application.
        public CustomApplication? Application { get; set; }        
    }
}