using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
			using var scope = services.CreateScope();

            // Client manager for the SPA client
			var appManager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            // Scope manager for the default API scope
			var scopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();

			// Ensure the default API scope exists (used by resource APIs).
			if (await scopeManager.FindByNameAsync("modular-monolith-ddd-api") is null)
			{
                // Create the default API scope
				await scopeManager.CreateAsync(new OpenIddictScopeDescriptor
				{
					Name = "modular-monolith-ddd-api",
					DisplayName = "Modular Monolith DDD API scope",
                    Description = "API scope for the Modular Monolith DDD API",
                    
                    // Set default resources for the API scope
                    Resources = { "modular-monolith-ddd-api" }
				});
			}

			// Register a public SPA client configured for authorization code + PKCE.

            // Client name, default is "spa"
			var spaClientId = configuration["Auth:Spa:ClientId"] ?? "spa";
			// Client redirect URI after user login, default is "http://localhost:3001/callback"
			var spaRedirectUri = configuration["Auth:Spa:RedirectUri"] ?? "http://localhost:3001/callback";
            // Client post logout redirect URI after user logout, default is "http://localhost:3001/"
            var spaPostLogoutRedirectUri = configuration["Auth:Spa:PostLogoutRedirectUri"] ?? "http://localhost:3001/";

			// Check if the client already exists
			if (await appManager.FindByClientIdAsync(spaClientId) is null)
			{
                // Create a new client descriptor
				var descriptor = new OpenIddictApplicationDescriptor
				{
					ClientId = spaClientId, // Client name
					ConsentType = OpenIddictConstants.ConsentTypes.Implicit, // Consent type: no need consent screen
					DisplayName = "SPA Client", // Client display name
					ClientType = OpenIddictConstants.ClientTypes.Public, // Public client (SPA)
                    
                    //// Client type for client credentials flow
                    //ClientType = OpenIddictConstants.ClientTypes.Confidential,

					RedirectUris = { new Uri(spaRedirectUri) }, // URL callback
					PostLogoutRedirectUris = { new Uri(spaPostLogoutRedirectUri) }, // URL after user logout
					Permissions =
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
					}
				};

				// Save client to database through OpenIddict manager
				await appManager.CreateAsync(descriptor);
			}
		}
	}
}


