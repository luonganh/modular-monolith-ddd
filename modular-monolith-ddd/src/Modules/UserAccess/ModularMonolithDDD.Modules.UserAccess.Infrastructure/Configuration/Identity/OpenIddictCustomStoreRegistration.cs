using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity.Stores;

namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity
{
    /// <summary>
    /// Registers OpenIddict (core, server, validation) for the UserAccess module using Custom Stores.
    /// Uses Dapper-based stores for SQL Server persistence instead of EF Core.
    /// </summary>
    public static class OpenIddictCustomStoreRegistration
    {
        /// <summary>
        /// Adds and configures OpenIddict with Custom Stores using Dapper for SQL Server.
        /// </summary>
        public static IServiceCollection AddUserAccessAuthenticationCustomStores(
            this IServiceCollection services, IConfiguration config)
        {
            services.AddOpenIddict()
                // Configure OpenIddict core services and data storage
                .AddCore(o =>
                {
                    // Set up OpenIddict Core with Custom Stores
                    // Note: Custom stores are registered via DI below, not through UseCustomStores()
                })
                .AddServer(o =>
                {
                    // Expose standard OIDC endpoints.
                    o.SetAuthorizationEndpointUris("/connect/authorize")
                     .SetTokenEndpointUris("/connect/token")
                     .SetEndSessionEndpointUris("/connect/endsession");

                    // Enable Authorization Code + PKCE for public clients and Client Credentials for service-to-service.
                    o.AllowAuthorizationCodeFlow().RequireProofKeyForCodeExchange();

                    // Enable Client Credentials flow for service-to-service communication
                    o.AllowClientCredentialsFlow();

                    // Enable Refresh Token flow for refreshing access tokens
                    o.AllowRefreshTokenFlow();

                    // Set the access token lifetime to 10 minutes
                    o.SetAccessTokenLifetime(TimeSpan.FromMinutes(10));

                    // Set the refresh token lifetime to 30 days
                    o.SetRefreshTokenLifetime(TimeSpan.FromDays(30));

                    // Register scopes used in the system
                    o.RegisterScopes(        
                        OpenIddictConstants.Scopes.OpenId,                
                        OpenIddictConstants.Scopes.Profile,
                        OpenIddictConstants.Scopes.Email,
                        OpenIddictConstants.Scopes.OfflineAccess,
                        "modular-monolith-ddd-api");

                    // Development certificates for signing and encryption (replace in production).
                    o.AddDevelopmentEncryptionCertificate()
                     .AddDevelopmentSigningCertificate();

                    // Integrate with ASP.NET Core endpoint pipeline
                    o.UseAspNetCore()
                     .EnableAuthorizationEndpointPassthrough()
                     .EnableTokenEndpointPassthrough()
                     .EnableEndSessionEndpointPassthrough();
                })
                .AddValidation(o =>
                {
                    // Validate tokens issued by this local OpenIddict server.
                    o.UseLocalServer();

                    // Configure OpenIddict to work with ASP.NET Core pipeline
                    o.UseAspNetCore();
                });

            // Register custom store implementations with proper generic types
            services.AddScoped<IOpenIddictApplicationStore<CustomApplication>, CustomApplicationStore>();
            services.AddScoped<IOpenIddictScopeStore<CustomScope>, CustomScopeStore>();
            services.AddScoped<IOpenIddictAuthorizationStore<CustomAuthorization>, CustomAuthorizationStore>();
            services.AddScoped<IOpenIddictTokenStore<CustomToken>, CustomTokenStore>();
                       
            return services;
        }
    }

}
