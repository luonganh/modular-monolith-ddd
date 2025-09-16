namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity
{
    /// <summary>
    /// Registers OpenIddict (core, server, validation) for the UserAccess module.
    /// Configures endpoints, flows, scopes and development certificates.
    /// </summary>
    public static class OpenIddictRegistration
    {
        /// <summary>
        /// Adds and configures OpenIddict with EF Core stores based on <c>UserAccessContext</c>,
        /// sets up Authorization Code + PKCE and Client Credentials flows, and enables
        /// ASP.NET Core host integration for endpoints passthrough.
        /// </summary>
        public static IServiceCollection AddUserAccessAuthentication(
            this IServiceCollection services, IConfiguration config)
        {
            services.AddOpenIddict()
                // Configure OpenIddict core services and data storage
                .AddCore(o =>
                {
                    // Set up OpenIddict Core with Entity Framework Core integration
                    // This configures OpenIddict to use EF Core for storing:
                    // - OAuth2 applications and clients
                    // - Authorization codes and tokens
                    // - Scopes and user consents
                    // UserAccessContext will be used as the database context for these operations
                    o.UseEntityFrameworkCore().UseDbContext<UserAccessContext>();                   
                })
                .AddServer(o =>
                {
                    // Expose standard OIDC endpoints.
                    o.SetAuthorizationEndpointUris("/connect/authorize") // authentication endpoint
                     .SetTokenEndpointUris("/connect/token") // token endpoint
                     .SetEndSessionEndpointUris("/connect/endsession"); // end session endpoint/logout endpoint

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
                    // - Profile: User profile
                    // - Email: User email
                    // - OfflineAccess: Offline access
                    // - modular-monolith-ddd-api: API scope
                    // Define the API scope that clients can request access to
                    o.RegisterScopes(                        
                        OpenIddictConstants.Scopes.Profile,
                        OpenIddictConstants.Scopes.Email,
                        OpenIddictConstants.Scopes.OfflineAccess,
                        "modular-monolith-ddd-api");

                    // Development certificates for signing and encryption (replace in production).
                    o.AddDevelopmentEncryptionCertificate()
                     .AddDevelopmentSigningCertificate();

                    // Integrate with ASP.NET Core endpoint pipeline and bubble responses through the MVC stack.
                    o.UseAspNetCore()

                    // Enable passthrough so ASP.NET Core can process authorization endpoint (/connect/authorize)
                     .EnableAuthorizationEndpointPassthrough()

                     // Enable passthrough so ASP.NET Core can process token endpoint (/connect/token)
                     .EnableTokenEndpointPassthrough()

                     // Enable passthrough so ASP.NET Core can process end session endpoint (/connect/endsession)
                     .EnableEndSessionEndpointPassthrough();
                })
                .AddValidation(o =>
                {
                    // Validate tokens issued by this local OpenIddict server.
                    o.UseLocalServer();

                    // Configure OpenIddict to work with ASP.NET Core pipeline
                    o.UseAspNetCore();
                });
                       
            return services;
        }
    }
}
