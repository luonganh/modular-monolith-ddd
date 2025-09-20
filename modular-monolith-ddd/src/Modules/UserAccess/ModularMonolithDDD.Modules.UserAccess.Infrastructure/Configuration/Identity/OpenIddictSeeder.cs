using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity.Stores;

namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity
{
	/// <summary>
	/// Provides a startup seeding routine for OpenIddict artifacts used by the UserAccess module.
	/// Seeds a default API scope and a public SPA client so developers can authenticate quickly in dev.
	/// </summary>
	public static class OpenIddictSeeder
	{
		/// <summary>
		/// Ensures the presence of the default <c>api</c> scope and a public SPA client.
		/// Values like client id and redirect URIs can be overridden via configuration keys:
		/// <c>Auth:Spa:ClientId</c>, <c>Auth:Spa:RedirectUri</c>, <c>Auth:Spa:PostLogoutRedirectUri</c>.
		/// </summary>
		/// <param name="services">The application service provider used to resolve OpenIddict managers.</param>
		/// <param name="configuration">Configuration source for SPA client overrides.</param>
		public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
		{
            // Resolve OpenIddict managers from a scoped service provider.
            // using var scope = services.CreateScope();

            // Client manager for the SPA client
            var appStore = services.GetRequiredService<IOpenIddictApplicationStore<CustomApplication>>();

            // Scope manager for the default API scope
            var scopeStore = services.GetRequiredService<IOpenIddictScopeStore<CustomScope>>();
            var ct = CancellationToken.None;

            // Ensure the default API scope exists (used by resource APIs).
            var name = "modular-monolith-ddd-api";
            var existingScope = await scopeStore.FindByNameAsync(name, ct);
            if (existingScope is null)
            {
                // Create the default API scope
                var scopeEntity = new CustomScope
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    DisplayName = "Modular Monolith DDD API scope",
                    Description = "API scope for the Modular Monolith DDD API",
                    // Set default resources for the API scope
                    ResourcesJson = System.Text.Json.JsonSerializer.Serialize(new[] { "modular-monolith-ddd-api" }),
                    CreatedAtUtc = DateTime.UtcNow
                };
                await scopeStore.CreateAsync(scopeEntity, ct);
            }
           
			// Register a public SPA client configured for authorization code + PKCE.

            // Client name, default is "spa"
			var clientId = configuration["Auth:Spa:ClientId"] ?? "spa";
			// Client redirect URI after user login, default is "http://localhost:5173/callback"
			var redirectUri = configuration["Auth:Spa:RedirectUri"] ?? "http://localhost:5173/callback";
			// Client post logout redirect URI after user logout, default is "http://localhost:5173/"
			var postLogoutRedirectUri = configuration["Auth:Spa:PostLogoutRedirectUri"] ?? "http://localhost:5173/";

            // Check if the client already exists
            var existingClient = await appStore.FindByClientIdAsync(clientId, ct);
            if (existingClient is not null) return;

            var app = new CustomApplication
            {
                Id = Guid.NewGuid(),
                ClientId = clientId, // Client name
                ClientType = OpenIddictConstants.ClientTypes.Public,  // Public client (SPA)
                ConsentType = OpenIddictConstants.ConsentTypes.Implicit, // Consent type: no need consent screen
                DisplayName = "SPA Client", // Client display name

                //// Client type for client credentials flow
                //ClientType = OpenIddictConstants.ClientTypes.Confidential,

                // JSON-encoded collections
                RedirectUrisJson = System.Text.Json.JsonSerializer.Serialize(new[] { redirectUri }),
                PostLogoutRedirectUrisJson = System.Text.Json.JsonSerializer.Serialize(new[] { postLogoutRedirectUri }),
                PermissionsJson = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    // Endpoints - Callable Endpoints
				    OpenIddictConstants.Permissions.Endpoints.Authorization, // /connect/authorize
					OpenIddictConstants.Permissions.Endpoints.Token,  // /connect/token    
					OpenIddictConstants.Permissions.Endpoints.EndSession, // /connect/endsession

                    // Grant types - Allowed flows 
					OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode, // Authorization Code flow
					OpenIddictConstants.Permissions.GrantTypes.RefreshToken, // Refresh Token flow
                    // OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                    
                    // Response types / PKCE for public clients
				    OpenIddictConstants.Permissions.ResponseTypes.Code,

                    // Scopes - Access permissions
					OpenIddictConstants.Permissions.Scopes.Profile, // User profile						
                    OpenIddictConstants.Permissions.Scopes.Email, // User email
                    OpenIddictConstants.Permissions.Prefixes.Scope + OpenIddictConstants.Scopes.OfflineAccess, // Offline access                    
                    "modular-monolith-ddd-api"
                }),
                RequirementsJson = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange // PKCE cho SPA
                }),
                CreatedAtUtc = DateTime.UtcNow
            };            
		}
	}
}


