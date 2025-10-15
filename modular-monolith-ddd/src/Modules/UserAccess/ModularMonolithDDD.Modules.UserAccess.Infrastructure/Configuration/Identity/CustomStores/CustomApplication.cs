namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity.CustomStores
{
    // Minimal POCOs used by the custom stores. These are NOT EF entities.

    /// <summary>
    /// OAuth/OIDC Client (Registered Application).
    /// One application can have many authorizations, tokens.
    /// Store OAuth clients information.
    /// This class represents an OAuth/OIDC client application registered in the identity system.
    /// It contains information about the client, such as its ID, client ID, client type, consent type, display name, client secret, redirect URIs, post-logout redirect URIs, permissions, requirements, creation date, and update date.
    /// It also contains navigation properties for EF relationships to the CustomAuthorization and CustomToken classes.
    /// </summary>
    public sealed class CustomApplication
    {
        /// <summary>Database primary key.</summary>
        public Guid Id { get; set; } = default!;

        /// <summary>Public identifier of the OAuth client (e.g., "spa").</summary>
        public string ClientId { get; set; } = default!;

        /// <summary>
        /// Client category: e.g., public (SPA/native) or confidential (server).
        /// Public client: Cannot keep a client secret securely.
        /// E.x: SPA/native mobile apps, desktop apps, smart TVs, device apps, etc.
        /// No client secret.        
        /// Grant type: 
        /// Allowed: Authorization Code+PKCE, Device Code, Refresh Token.
        /// Avoid: Implicit.
        /// Not allowed: Client Credentials.       
        /// CORS.
        /// 
        /// Confidential client: Can keep a client secret securely on the server.
        /// E.x: Server-side web apps (MVC, WebAPI), background services, etc.
        /// Has client secret or private key/client secret jwt.
        /// Grant type: 
        /// Authorization Code, Client Credentials, JWT Bearer, Refresh Token.
        /// Avoid: Resource Owner Password (RO), Implicit.
        /// No CORS (server-to-server).        
        /// </summary>
        public string ClientType { get; set; } = default!;

        /// <summary>
        /// Consent policy for this client (e.g., explicit/implicit/system).
        /// Explicit: Always prompt the user with a consent screen when requesting new/not-yet-granted scp: Used for3rd party applications.        
        /// Implicit: Skip the consent screen; assume the user has agreed to the requested scp: Used for trusted first-party/internal applications.
        /// System: Never prompts the user. Permissions are granted at the system level (admin/seed). Used for background services, etc.
        /// External: The consent is handled outside your Identity Provider. Used for enterprise SSO, etc.        
        /// </summary>
        public string? ConsentType { get; set; }

        /// <summary>Human-friendly name displayed on consent screens.</summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// Secret for confidential clients (hashed). Empty for public clients.
        /// Use Hashing Algorithms like BCrypt, Argon2, etc to hash the secret.
        /// Save: hash = BCrypt.HashPassword(secret);
        /// Check: BCrypt.Verify(secret, hash);
        /// </summary>
        public string? ClientSecret { get; set; }

        /// <summary>
        /// JSON Array of redirect URIs.
        /// Callback endpoints the authorization server are allowed to redirect to after login/consent.
        /// Where the client receives the authorization code (or relevant parameters) to continue the flow.
        /// e.g, ["https://example.com/callback", "https://example.com/callback2"]
        /// </summary>
        public string? RedirectUrisJson { get; set; }

        /// <summary>
        /// JSON Array of allowed URIs to redirect the user to after a successful logout.
        /// Return the user to the application after logout.
        /// e.g, ["https://example.com/post_logout_redirect", "https://example.com/post_logout_redirect2"]
        /// </summary>
        public string? PostLogoutRedirectUrisJson { get; set; }

        /// <summary>
        /// JSON Array of permissions (endpoints, grant types, scopes, response types, etc.).
        /// Permissions are the capabilities/actions/resources the client is allowed to access.
        /// e.g, ["ept:authorization", "ept:device_authorization", "ept:end_session", "ept:introspection", "ept:pushed_authorization", "ept:revocation", "ept:token", "ept:userinfo"]
        /// Endpoints: 
        /// The endpoints of Authorization Server (Identity Provider) (not API resources) are allowed to access. 
        /// e.g. ept:authorization, 
        /// ept:device_authorization,
        /// ept:end_session (logout),        
        /// ept:introspection, 
        /// ept:pushed_authorization,
        /// ept:revocation, 
        /// ept:token, 
        /// ept:userinfo,         
        /// etc.
        /// 
        /// Grant types: The grant types the client is allowed to use. 
        /// e.g. gt:authorization_code, 
        /// gt:client_credentials, 
        /// gt.gt:urn:ietf:params:oauth:grant-type:device_code,
        /// gt:implicit,
        /// gt:password,
        /// gt:refresh_token
        /// gt:urn:ietf:params:oauth:grant-type:token-exchange
        /// 
        /// Scopes: The scopes the client is allowed to access. 
        /// e.g. scp:profile, 
        /// scp:roles,
        /// scp:email, 
        /// scp:address, 
        /// scp:phone, 
        /// etc.
        /// 
        /// Response types: The response types the client is allowed to use. 
        /// e.g. rst:code, rst:token, rst:id_token, etc.       
        /// rst:code (Authorization Code grant type (+PKCE): 
        /// The client receives an authorization code to exchange for an access token.              
        /// rst:id_token (Implicit grant type): 
        /// The client receives an ID token to identify the user.           
        /// rst:code id_token (Hybrid): 
        /// The client receives an authorization code and an ID token to identify the user.        
        /// </summary>
        public string PermissionsJson { get; set; } = default!;

        /// <summary>
        /// JSON Array of requirements.
        /// Requirements are the conditions the client must meet to use the client.    
        /// e.g, ["requirements.pkce", "requirements.consent", "requirements.par", "requirements.request_object", "requirements.redirects", "requirements.client_auth", "requirements.nonce", "requirements.response_types", "requirements.refresh_token", "requirements.dpop"]
        /// 
        /// Require PKCE (S256) for authorization code; forbid plain:
        /// requirements.pkce (or server-specific flag). 
        /// Only allow authorization code if code_challenge_method=S256.
        /// The client must use a Proof Key for Code Exchange (PKCE) to secure the authorization code.
        /// 
        /// Require user consent for new scopes:
        /// requirements.consent
        /// The client must get the user's consent to use the client. Show consent UI for new scopes.
        /// 
        /// Require PAR (pushed authorization request): 
        /// requirements.par
        /// The client must be able to push the authorization request to the authorization server.
        /// Authorize requests only if initiated via pushed authorization request.
        /// 
        /// Require exact redirect URI match; HTTPS-only redirect URIs:
        /// requirements.request_object.signed or requirements.request_object.encrypted
        /// Only accept authorize requests carrying a signed/encrypted JWT.
        /// 
        /// Require exact HTTPS redirect:
        /// requirements.redirects.https_only or requirements.redirects.exact_match
        /// 
        /// Require confidential auth:
        /// requirements.client_auth.private_key_jwt
        /// Reject basic/post secrets.
        /// 
        /// Require nonce/jti:
        /// requirements.nonce (OIDC) and requirements.assertion.jti_exp (JWT assertions must include jti+exp)
        ///
        /// Restrict response types:
        /// requirements.response_types.code_only
        /// Reject implicit/hybrid.
        /// 
        /// Require refresh token rotation:
        /// requirements.refresh_token.rotation and requirements.refresh_token.reuse_detection
        /// And revocation on reuse.
        /// 
        /// Require DPoP:
        /// requirements.dpop
        /// Only accept DPoP-bound access tokens.                     
        /// </summary>
        public string? RequirementsJson { get; set; }

        /// <summary>Row creation timestamp in UTC.</summary>
        public DateTime CreatedAtUtc { get; set; }

        /// <summary>Row last update timestamp in UTC, if any.</summary>
        public DateTime? UpdatedAtUtc { get; set; }

        // Navigation properties for EF relationships

        // 1-to-many (optional) with Authorization
        // 1 Application can have 0 or many Authorizations.
        public ICollection<CustomAuthorization> Authorizations { get; set; } = new List<CustomAuthorization>();

        // 1-to-many (optional) with Token
        // 1 Application can have 0 or many Tokens.
        public ICollection<CustomToken> Tokens { get; set; } = new List<CustomToken>();
    }
}