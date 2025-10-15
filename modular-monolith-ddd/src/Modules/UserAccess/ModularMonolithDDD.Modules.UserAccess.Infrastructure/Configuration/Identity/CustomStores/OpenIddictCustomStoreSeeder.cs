using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity.CustomStores
{
	/// <summary>
	/// Provides a startup seeding routine for OpenIddict artifacts used by the UserAccess module.
	/// Seeds a default API scope and a public SPA client so developers can authenticate quickly in dev.
	/// </summary>
	public static class OpenIddictCustomStoreSeeder
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

            var identity = configuration.GetSection("Identity").Get<IdentitySettings>();

            // Ensure the default API scope exists (used by resource APIs).
            var scope = identity?.Scope;
            if (scope == null)
            {
                throw new InvalidOperationException("Identity scope configuration is null");
            }
            var existingScope = await scopeStore.FindByNameAsync(scope.Name, ct);
            if (existingScope is null)
            {
                // Create the default API scope
                var scopeEntity = new CustomScope
                {
                    Id = Guid.NewGuid(),
                    Name = scope.Name,
                    DisplayName = scope.DisplayName,
                    Description = scope.Description,
                    // Set default resources for the API scope
                    ResourcesJson = System.Text.Json.JsonSerializer.Serialize(new[] { scope.Resources.FirstOrDefault() }),
                    CreatedAtUtc = DateTime.UtcNow
                };
                // Persist the scope using the custom scope store
                await scopeStore.CreateAsync(scopeEntity, ct);
            }

            // Register a public SPA client configured for authorization code + PKCE.

            var client = identity?.Client;
            // Client name, default is "spa"
            var clientId = client?.ClientId ?? "admin-spa";
			// Client redirect URI after user login, default is "http://localhost:3000/callback"			
            // Read multi-URIs from config (comma-separated) with defaults
            var redirectUris =
                (configuration["Identity:Client:RedirectUris"] ?? 
                client?.RedirectUri ?? "http://localhost:3000/callback")
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Distinct()
                .ToArray();

			// Client post logout redirect URI after user logout, default is "http://localhost:3000/"			
            var postLogoutRedirectUris =
                (configuration["Identity:Client:PostLogoutRedirectUris"] ?? 
                client?.PostLogoutRedirectUri ?? "http://localhost:3000/")
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Distinct()
                .ToArray();
                
            // Check if the client already exists
            var existingClient = await appStore.FindByClientIdAsync(clientId, ct);
            if (existingClient is not null) {
                existingClient.RedirectUrisJson = System.Text.Json.JsonSerializer.Serialize(redirectUris);
                existingClient.PostLogoutRedirectUrisJson = System.Text.Json.JsonSerializer.Serialize(postLogoutRedirectUris);
                await appStore.UpdateAsync(existingClient, ct);
                return;
            }

            // If the client does not exist, create a new one
            var newClient = new CustomApplication
            {
                Id = Guid.NewGuid(),
                ClientId = clientId, // Client name
                ClientType = OpenIddictConstants.ClientTypes.Public,  // Public client (SPA)
                ConsentType = OpenIddictConstants.ConsentTypes.Implicit, // Consent type: no need consent screen
                DisplayName = client?.DisplayName ?? "Admin SPA", // Client display name

                //// Client type for client credentials flow
                //ClientType = OpenIddictConstants.ClientTypes.Confidential,

                // JSON-encoded collections
                RedirectUrisJson = System.Text.Json.JsonSerializer.Serialize(redirectUris),
                PostLogoutRedirectUrisJson = System.Text.Json.JsonSerializer.Serialize(postLogoutRedirectUris),

                // Client permissions                
                PermissionsJson = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    // Endpoints - Callable Endpoints
				    OpenIddictConstants.Permissions.Endpoints.Authorization, // /connect/authorize
					OpenIddictConstants.Permissions.Endpoints.Token,  // /connect/token    
					OpenIddictConstants.Permissions.Endpoints.EndSession, // /connect/endsession

                    // Grant types - Allowed flows 
					OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode, // Authorization Code flow

                    // Client can send refresh token requests
					OpenIddictConstants.Permissions.GrantTypes.RefreshToken, // Refresh Token flow
                    // OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                    
                    // Response types / PKCE for public clients
				    OpenIddictConstants.Permissions.ResponseTypes.Code,

                    // Scopes - Access permissions
                    // Client is allowed to request which scopes                    
                    OpenIddictConstants.Permissions.Scopes.Profile, // Client is allowed to request 'profile'					
                    OpenIddictConstants.Permissions.Scopes.Email, // Client is allowed to request 'email'

                    // Enable refresh token functionality for offline access  
                    // Offline access is needed for:
                    // Mobile apps - User doesn't want to log in again
                    // Desktop apps - Running in background
                    // Long-running processes - Running all day
                    // User experience - No interruption
                    OpenIddictConstants.Permissions.Prefixes.Scope + OpenIddictConstants.Scopes.OfflineAccess, 
                    // API scope permission
                    OpenIddictConstants.Permissions.Prefixes.Scope + scope.Name
                }),
                RequirementsJson = System.Text.Json.JsonSerializer.Serialize(new[]
                {
                    OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange // PKCE for SPA
                }),
                CreatedAtUtc = DateTime.UtcNow
            };

            // Persist the application using the custom application store
            await appStore.CreateAsync(newClient, ct);
		}
	}
}


