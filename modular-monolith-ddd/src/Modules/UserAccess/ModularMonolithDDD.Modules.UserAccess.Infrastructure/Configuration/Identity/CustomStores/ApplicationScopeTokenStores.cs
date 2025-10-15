using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity.Constants;
using System.Globalization;
using System.Text.Json;

namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity.CustomStores
{


    /// <summary>
    /// Custom OpenIddict Application Store implementation using Dapper for SQL Server persistence.
    /// This store handles OAuth2/OpenID Connect client applications (like SPA, mobile apps, etc.).
    /// 
    /// Key responsibilities:
    /// - Store and retrieve OAuth2 client applications
    /// - Support client credentials flow and authorization code flow
    /// - Manage client secrets, redirect URIs, and permissions
    /// 
    /// Note: This is a minimal implementation focused on seeding and basic lookups.
    /// Many advanced methods are not implemented as they're not needed for current flows.
    /// </summary>
    public sealed class CustomApplicationStore : IOpenIddictApplicationStore<CustomApplication>
    {
        /// <summary>
        /// Factory for creating SQL Server database connections using Dapper.
        /// This is injected via dependency injection and provides connection management.
        /// </summary>
        private readonly ISqlConnectionFactory _connectionFactory;

        /// <summary>
        /// Initializes a new instance of the CustomApplicationStore.
        /// </summary>
        /// <param name="connectionFactory">Factory for creating database connections</param>
        public CustomApplicationStore(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Creates a new OAuth2 client application in the database.
        /// This is called during seeding to create default applications (like SPA client).
        /// </summary>
        /// <param name="application">The application entity to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public async ValueTask CreateAsync(CustomApplication application, CancellationToken cancellationToken)
        {
            // Get a database connection from the factory
            using var connection = _connectionFactory.GetOpenConnection();
            // Execute the INSERT SQL using Dapper with the application object as parameters
            await connection.ExecuteAsync(Sql.InsertApplication, application);
        }

        /// <summary>
        /// Finds an OAuth2 client application by its ClientId.
        /// This is used by OpenIddict during token requests to validate the client.
        /// </summary>
        /// <param name="identifier">The ClientId to search for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The found application or null if not found</returns>
        public async ValueTask<CustomApplication?> FindByClientIdAsync(string identifier, CancellationToken cancellationToken)
        {
            // Get a database connection from the factory
            using var connection = _connectionFactory.GetOpenConnection();
            // Query for the application using Dapper with parameterized query
            return await connection.QueryFirstOrDefaultAsync<CustomApplication>(Sql.GetApplicationByClientId, new { ClientId = identifier });
        }

        // =====================================================================================
        // MINIMAL IMPLEMENTATION FOR REQUIRED INTERFACE METHODS
        // =====================================================================================
        // These methods are required by IOpenIddictApplicationStore but not used in current flows.
        // They return default/empty values to satisfy the interface contract.

        /// <summary>
        /// Returns the total count of applications (not implemented - returns 0).
        /// </summary>
        public ValueTask<long> CountAsync(CancellationToken cancellationToken) => new(0);
        
        /// <summary>
        /// Returns the count of applications matching a query (not implemented - returns 0).
        /// </summary>
        public ValueTask<long> CountAsync<TResult>(Func<IQueryable<CustomApplication>, IQueryable<TResult>> query, CancellationToken cancellationToken) => new(0L);
        
        /// <summary>
        /// Deletes an application (not implemented - no-op).
        /// </summary>
        public ValueTask DeleteAsync(CustomApplication application, CancellationToken cancellationToken) => ValueTask.CompletedTask;
        
        /// <summary>
        /// Finds an application by its ID (not implemented - returns null).
        /// </summary>
        public ValueTask<CustomApplication?> FindByIdAsync(string identifier, CancellationToken cancellationToken) => new((CustomApplication?)null);
        
        /// <summary>
        /// Finds applications by post-logout redirect URI (not implemented - returns empty).
        /// </summary>
        public IAsyncEnumerable<CustomApplication> FindByPostLogoutRedirectUriAsync(string address, CancellationToken cancellationToken) => AsyncEnumerable<CustomApplication>.Empty;
        
        /// <summary>
        /// Finds applications by redirect URI (not implemented - returns empty).
        /// </summary>
        public IAsyncEnumerable<CustomApplication> FindByRedirectUriAsync(string address, CancellationToken cancellationToken) => AsyncEnumerable<CustomApplication>.Empty;
        
        /// <summary>
        /// Executes a custom query (not implemented - throws exception).
        /// </summary>
        public ValueTask<TResult> GetAsync<TState, TResult>(Func<IQueryable<CustomApplication>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken) => throw new NotImplementedException();
        
        /// <summary>
        /// Lists applications with pagination (not implemented - returns empty).
        /// </summary>
        public IAsyncEnumerable<CustomApplication> ListAsync(int? count, int? offset, CancellationToken cancellationToken) => AsyncEnumerable<CustomApplication>.Empty;
        
        /// <summary>
        /// Lists applications with custom query (not implemented - throws exception).
        /// </summary>
        public IAsyncEnumerable<TResult> ListAsync<TState, TResult>(Func<IQueryable<CustomApplication>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken) => throw new NotImplementedException();
        
        /// <summary>
        /// Updates an application (not implemented - no-op).
        /// </summary>
        public ValueTask UpdateAsync(CustomApplication application, CancellationToken cancellationToken) => ValueTask.CompletedTask;

        // =====================================================================================
        // PROPERTY GETTERS AND SETTERS
        // =====================================================================================
        // These methods provide access to application properties for OpenIddict framework.
        // Most are simple property accessors, some return empty collections for unused features.

        /// <summary>
        /// Gets the unique identifier of the application as a string.
        /// </summary>
        public ValueTask<string?> GetIdAsync(CustomApplication application, CancellationToken cancellationToken) => new(application.Id.ToString());
        
        /// <summary>
        /// Gets the OAuth2 client ID (public identifier for the application).
        /// </summary>
        public ValueTask<string?> GetClientIdAsync(CustomApplication application, CancellationToken cancellationToken) => new(application.ClientId);
        
        /// <summary>
        /// Gets the OAuth2 client type (e.g., "public" for SPA, "confidential" for server apps).
        /// </summary>
        public ValueTask<string?> GetClientTypeAsync(CustomApplication application, CancellationToken cancellationToken) => new(application.ClientType);
        
        /// <summary>
        /// Gets the consent type (how user consent is handled).
        /// </summary>
        public ValueTask<string?> GetConsentTypeAsync(CustomApplication application, CancellationToken cancellationToken) => new(application.ConsentType);
        
        /// <summary>
        /// Gets the display name of the application.
        /// </summary>
        public ValueTask<string?> GetDisplayNameAsync(CustomApplication application, CancellationToken cancellationToken) => new(application.DisplayName);
        
        /// <summary>
        /// Gets localized display names (not implemented - returns empty).
        /// </summary>
        public ValueTask<ImmutableDictionary<CultureInfo, string>> GetDisplayNamesAsync(CustomApplication application, CancellationToken cancellationToken) => new(ImmutableDictionary<CultureInfo, string>.Empty);

        /// <summary>
        /// Gets the permissions granted to this application (deserialize from JSON).
        /// </summary>
        public ValueTask<ImmutableArray<string>> GetPermissionsAsync(CustomApplication application, CancellationToken cancellationToken)
        {
            var list = string.IsNullOrWhiteSpace(application.PermissionsJson)
                ? Array.Empty<string>()
                : System.Text.Json.JsonSerializer.Deserialize<string[]>(application.PermissionsJson) ?? Array.Empty<string>();

            return new(ImmutableArray.CreateRange(list));
        }

        /// <summary>
        /// Gets post-logout redirect URIs (deserialize from JSON).
        /// </summary>
        public ValueTask<ImmutableArray<string>> GetPostLogoutRedirectUrisAsync(CustomApplication application, CancellationToken cancellationToken)
        {
            var list = string.IsNullOrWhiteSpace(application.PostLogoutRedirectUrisJson)
                ? Array.Empty<string>()
                : System.Text.Json.JsonSerializer.Deserialize<string[]>(application.PostLogoutRedirectUrisJson) ?? Array.Empty<string>();

            return new(ImmutableArray.CreateRange(list));
        }


        /// <summary>
        /// Gets custom properties (not implemented - returns empty).
        /// </summary>
        public ValueTask<ImmutableDictionary<string, JsonElement>> GetPropertiesAsync(CustomApplication application, CancellationToken cancellationToken) => new(ImmutableDictionary<string, JsonElement>.Empty);

        /// <summary>
        /// Gets redirect URIs for authorization code flow (deserialize from JSON).
        /// </summary>
        public ValueTask<ImmutableArray<string>> GetRedirectUrisAsync(CustomApplication application, CancellationToken cancellationToken)
        {
            var list = string.IsNullOrWhiteSpace(application.RedirectUrisJson)
                ? Array.Empty<string>()
                : System.Text.Json.JsonSerializer.Deserialize<string[]>(application.RedirectUrisJson) ?? Array.Empty<string>();

            return new(ImmutableArray.CreateRange(list));
        }

        /// <summary>
        /// Gets OAuth2 requirements (not implemented - returns empty).
        /// </summary>
        public ValueTask<ImmutableArray<string>> GetRequirementsAsync(CustomApplication application, CancellationToken cancellationToken) => new(ImmutableArray<string>.Empty);
        
        /// <summary>
        /// Gets application settings (not implemented - returns empty).
        /// </summary>
        public ValueTask<ImmutableDictionary<string, string>> GetSettingsAsync(CustomApplication application, CancellationToken cancellationToken) => new(ImmutableDictionary<string, string>.Empty);
        
        /// <summary>
        /// Creates a new empty application instance for seeding.
        /// </summary>
        public ValueTask<CustomApplication> InstantiateAsync(CancellationToken cancellationToken) => new(new CustomApplication());
        
        /// <summary>
        /// Sets the client ID (not implemented - no-op).
        /// </summary>
        public ValueTask SetClientIdAsync(CustomApplication application, string? identifier, CancellationToken cancellationToken) => ValueTask.CompletedTask;
        
        /// <summary>
        /// Sets the client type (not implemented - no-op).
        /// </summary>
        public ValueTask SetClientTypeAsync(CustomApplication application, string? type, CancellationToken cancellationToken) => ValueTask.CompletedTask;
        
        /// <summary>
        /// Sets the consent type (not implemented - no-op).
        /// </summary>
        public ValueTask SetConsentTypeAsync(CustomApplication application, string? type, CancellationToken cancellationToken) => ValueTask.CompletedTask;
        
        /// <summary>
        /// Sets the display name (not implemented - no-op).
        /// </summary>
        public ValueTask SetDisplayNameAsync(CustomApplication application, string? name, CancellationToken cancellationToken) => ValueTask.CompletedTask;
        
        /// <summary>
        /// Sets localized display names (not implemented - no-op).
        /// </summary>
        public ValueTask SetDisplayNamesAsync(CustomApplication application, ImmutableDictionary<CultureInfo, string> names, CancellationToken cancellationToken) => ValueTask.CompletedTask;
        
        /// <summary>
        /// Sets permissions (not implemented - no-op).
        /// </summary>
        public ValueTask SetPermissionsAsync(CustomApplication application, ImmutableArray<string> permissions, CancellationToken cancellationToken) => ValueTask.CompletedTask;
        
        /// <summary>
        /// Sets post-logout redirect URIs (not implemented - no-op).
        /// </summary>
        public ValueTask SetPostLogoutRedirectUrisAsync(CustomApplication application, ImmutableArray<string> uris, CancellationToken cancellationToken) => ValueTask.CompletedTask;
        
        /// <summary>
        /// Sets custom properties (not implemented - no-op).
        /// </summary>
        public ValueTask SetPropertiesAsync(CustomApplication application, ImmutableDictionary<string, JsonElement> properties, CancellationToken cancellationToken) => ValueTask.CompletedTask;
        
        /// <summary>
        /// Sets redirect URIs (not implemented - no-op).
        /// </summary>
        public ValueTask SetRedirectUrisAsync(CustomApplication application, ImmutableArray<string> uris, CancellationToken cancellationToken) => ValueTask.CompletedTask;
        
        /// <summary>
        /// Sets OAuth2 requirements (not implemented - no-op).
        /// </summary>
        public ValueTask SetRequirementsAsync(CustomApplication application, ImmutableArray<string> requirements, CancellationToken cancellationToken) => ValueTask.CompletedTask;
        
        /// <summary>
        /// Sets application settings (not implemented - no-op).
        /// </summary>
        public ValueTask SetSettingsAsync(CustomApplication application, ImmutableDictionary<string, string> settings, CancellationToken cancellationToken) => ValueTask.CompletedTask;
        
        /// <summary>
        /// Gets the client secret for confidential clients.
        /// </summary>
        public ValueTask<string?> GetClientSecretAsync(CustomApplication application, CancellationToken cancellationToken) => new(application.ClientSecret);
        
        /// <summary>
        /// Sets the client secret (not implemented - no-op).
        /// </summary>
        public ValueTask SetClientSecretAsync(CustomApplication application, string? secret, CancellationToken cancellationToken) => ValueTask.CompletedTask;
        
        /// <summary>
        /// Gets JSON Web Key Set for token validation (not implemented - returns null).
        /// </summary>
        public ValueTask<JsonWebKeySet?> GetJsonWebKeySetAsync(CustomApplication application, CancellationToken cancellationToken) => new((JsonWebKeySet?)null);
        
        /// <summary>
        /// Sets JSON Web Key Set (not implemented - no-op).
        /// </summary>
        public ValueTask SetJsonWebKeySetAsync(CustomApplication application, JsonWebKeySet? set, CancellationToken cancellationToken) => ValueTask.CompletedTask;
        
        /// <summary>
        /// Gets the application type (alias for client type).
        /// </summary>
        public ValueTask<string?> GetApplicationTypeAsync(CustomApplication application, CancellationToken cancellationToken) => new(application.ClientType);
        
        /// <summary>
        /// Sets the application type (not implemented - no-op).
        /// </summary>
        public ValueTask SetApplicationTypeAsync(CustomApplication application, string? type, CancellationToken cancellationToken) => ValueTask.CompletedTask;
    }

    public sealed class CustomScopeStore : IOpenIddictScopeStore<CustomScope>
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public CustomScopeStore(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async ValueTask CreateAsync(CustomScope scope, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.GetOpenConnection();
            await connection.ExecuteAsync(Sql.InsertScope, scope);
        }

        public async ValueTask<CustomScope?> FindByNameAsync(string name, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.GetOpenConnection();
            return await connection.QueryFirstOrDefaultAsync<CustomScope>(Sql.GetScopeByName, new { Name = name });
        }

        // Minimal implementation for required interface methods
        public ValueTask<long> CountAsync(CancellationToken cancellationToken) => new(0);
        public ValueTask<long> CountAsync<TResult>(Func<IQueryable<CustomScope>, IQueryable<TResult>> query, CancellationToken cancellationToken) => new(0L);
        public ValueTask DeleteAsync(CustomScope scope, CancellationToken cancellationToken) => ValueTask.CompletedTask;
        public ValueTask<CustomScope?> FindByIdAsync(string identifier, CancellationToken cancellationToken) => new((CustomScope?)null);
        public IAsyncEnumerable<CustomScope> FindByNamesAsync(ImmutableArray<string> names, CancellationToken cancellationToken) => AsyncEnumerable<CustomScope>.Empty;
        public IAsyncEnumerable<CustomScope> FindByResourceAsync(string resource, CancellationToken cancellationToken) => AsyncEnumerable<CustomScope>.Empty;
        public ValueTask<TResult> GetAsync<TState, TResult>(Func<IQueryable<CustomScope>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken) => throw new NotImplementedException();
        public IAsyncEnumerable<CustomScope> ListAsync(int? count, int? offset, CancellationToken cancellationToken) => AsyncEnumerable<CustomScope>.Empty;
        public IAsyncEnumerable<TResult> ListAsync<TState, TResult>(Func<IQueryable<CustomScope>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken) => throw new NotImplementedException();
        public ValueTask UpdateAsync(CustomScope scope, CancellationToken cancellationToken) => ValueTask.CompletedTask;

        // Additional required methods
        public ValueTask<string?> GetIdAsync(CustomScope scope, CancellationToken cancellationToken) => new(scope.Id.ToString());
        public ValueTask<string?> GetNameAsync(CustomScope scope, CancellationToken cancellationToken) => new(scope.Name);
        public ValueTask<string?> GetDisplayNameAsync(CustomScope scope, CancellationToken cancellationToken) => new(scope.DisplayName);
        public ValueTask<ImmutableDictionary<CultureInfo, string>> GetDisplayNamesAsync(CustomScope scope, CancellationToken cancellationToken) => new(ImmutableDictionary<CultureInfo, string>.Empty);
        public ValueTask<string?> GetDescriptionAsync(CustomScope scope, CancellationToken cancellationToken) => new(scope.Description);
        public ValueTask<ImmutableDictionary<CultureInfo, string>> GetDescriptionsAsync(CustomScope scope, CancellationToken cancellationToken) => new(ImmutableDictionary<CultureInfo, string>.Empty);
        public ValueTask<ImmutableDictionary<string, JsonElement>> GetPropertiesAsync(CustomScope scope, CancellationToken cancellationToken) => new(ImmutableDictionary<string, JsonElement>.Empty);
        public ValueTask<ImmutableArray<string>> GetResourcesAsync(CustomScope scope, CancellationToken cancellationToken) => new(ImmutableArray<string>.Empty);
        public ValueTask<CustomScope> InstantiateAsync(CancellationToken cancellationToken) => new(new CustomScope());
        public ValueTask SetDescriptionAsync(CustomScope scope, string? description, CancellationToken cancellationToken) => ValueTask.CompletedTask;
        public ValueTask SetDescriptionsAsync(CustomScope scope, ImmutableDictionary<CultureInfo, string> descriptions, CancellationToken cancellationToken) => ValueTask.CompletedTask;
        public ValueTask SetDisplayNameAsync(CustomScope scope, string? name, CancellationToken cancellationToken) => ValueTask.CompletedTask;
        public ValueTask SetDisplayNamesAsync(CustomScope scope, ImmutableDictionary<CultureInfo, string> names, CancellationToken cancellationToken) => ValueTask.CompletedTask;
        public ValueTask SetNameAsync(CustomScope scope, string? name, CancellationToken cancellationToken) => ValueTask.CompletedTask;
        public ValueTask SetPropertiesAsync(CustomScope scope, ImmutableDictionary<string, JsonElement> properties, CancellationToken cancellationToken) => ValueTask.CompletedTask;
        public ValueTask SetResourcesAsync(CustomScope scope, ImmutableArray<string> resources, CancellationToken cancellationToken) => ValueTask.CompletedTask;
    }

    public sealed class CustomAuthorizationStore : IOpenIddictAuthorizationStore<CustomAuthorization>
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public CustomAuthorizationStore(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async ValueTask CreateAsync(CustomAuthorization authorization, CancellationToken cancellationToken)
        {
            Console.WriteLine($"CreateAsync called with Status: {authorization.Status}, Type: {authorization.Type}");

            if (authorization.Id == Guid.Empty)
            {
                authorization.Id = Guid.NewGuid();
            }
            
            if (authorization.CreatedAtUtc == default)
            {
                authorization.CreatedAtUtc = DateTime.UtcNow;
            }
    
            // Set default values if missing
            if (string.IsNullOrEmpty(authorization.Status))
            {
                authorization.Status = OpenIddictConstants.Statuses.Valid;
                Console.WriteLine($"Set default Status: {authorization.Status}");
            }
            
            if (string.IsNullOrEmpty(authorization.Type))
            {
                authorization.Type = OpenIddictConstants.AuthorizationTypes.AdHoc;
                Console.WriteLine($"Set default Type: {authorization.Type}");
            }

            using var connection = _connectionFactory.GetOpenConnection();
            await connection.ExecuteAsync(Sql.InsertAuthorization, authorization);
            Console.WriteLine($"Authorization created successfully");
        }

        public async ValueTask<object> CreateAsync(OpenIddictAuthorizationDescriptor descriptor, CancellationToken cancellationToken)
        {
            Console.WriteLine($"CreateAsync(descriptor) called");
            Console.WriteLine($"  - ApplicationId: {descriptor.ApplicationId}");
            Console.WriteLine($"  - Subject: {descriptor.Subject}");
            Console.WriteLine($"  - Status: {descriptor.Status}");
            Console.WriteLine($"  - Type: {descriptor.Type}");
            Console.WriteLine($"  - Scopes: {string.Join(", ", descriptor.Scopes ?? Enumerable.Empty<string>())}");

            var authorization = new CustomAuthorization
            {
                Id = Guid.NewGuid(),
                ApplicationId = descriptor.ApplicationId != null ? Guid.Parse(descriptor.ApplicationId) : null,
                Subject = descriptor.Subject ?? throw new InvalidOperationException("Subject is required"),
                Status = descriptor.Status ?? OpenIddictConstants.Statuses.Valid,
                Type = descriptor.Type ?? OpenIddictConstants.AuthorizationTypes.AdHoc,
                ScopesJson = descriptor.Scopes != null ? System.Text.Json.JsonSerializer.Serialize(descriptor.Scopes) : null,
                Properties = descriptor.Properties != null ? System.Text.Json.JsonSerializer.Serialize(descriptor.Properties) : null,
                CreationDate = DateTime.UtcNow,
                CreatedAtUtc = DateTime.UtcNow
            };

            await CreateAsync(authorization, cancellationToken);
            return authorization;
        }

        public async ValueTask<CustomAuthorization?> FindByIdAsync(string identifier, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.GetOpenConnection();
            return await connection.QueryFirstOrDefaultAsync<CustomAuthorization>(Sql.GetAuthorizationById, new { Id = Guid.Parse(identifier) });
        }

        public async IAsyncEnumerable<CustomAuthorization> FindByApplicationIdAsync(string identifier, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.GetOpenConnection();
            var rows = await connection.QueryAsync<CustomAuthorization>(Sql.GetAuthorizationsByApplicationId, new { ApplicationId = Guid.Parse(identifier) });
            foreach (var row in rows)
            {
                yield return row;
            }
        }

        public async IAsyncEnumerable<CustomAuthorization> FindBySubjectAsync(string subject, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.GetOpenConnection();
            var rows = await connection.QueryAsync<CustomAuthorization>(Sql.GetAuthorizationsBySubject, new { Subject = subject });
            foreach (var row in rows)
            {
                yield return row;
            }
        }

        public async ValueTask UpdateAsync(CustomAuthorization authorization, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.GetOpenConnection();
            await connection.ExecuteAsync(Sql.UpdateAuthorization, authorization);
        }

        public async ValueTask DeleteAsync(CustomAuthorization authorization, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.GetOpenConnection();
            await connection.ExecuteAsync(Sql.DeleteAuthorization, new { authorization.Id });
        }

        // Minimal implementation for required interface methods
        public ValueTask<long> CountAsync(CancellationToken cancellationToken) => new(0);
        public ValueTask<long> CountAsync<TResult>(Func<IQueryable<CustomAuthorization>, IQueryable<TResult>> query, CancellationToken cancellationToken) => new(0L);
        public ValueTask<TResult> GetAsync<TState, TResult>(Func<IQueryable<CustomAuthorization>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken) => throw new NotImplementedException();
        public IAsyncEnumerable<CustomAuthorization> ListAsync(int? count, int? offset, CancellationToken cancellationToken) => AsyncEnumerable<CustomAuthorization>.Empty;
        public IAsyncEnumerable<TResult> ListAsync<TState, TResult>(Func<IQueryable<CustomAuthorization>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken) => throw new NotImplementedException();

        // Additional required methods
        public ValueTask<string?> GetIdAsync(CustomAuthorization authorization, CancellationToken cancellationToken) => new(authorization.Id.ToString());
        public ValueTask<string?> GetSubjectAsync(CustomAuthorization authorization, CancellationToken cancellationToken) => new(authorization.Subject);
        public ValueTask<string?> GetStatusAsync(CustomAuthorization authorization, CancellationToken cancellationToken) => new(authorization.Status);
        public ValueTask<string?> GetTypeAsync(CustomAuthorization authorization, CancellationToken cancellationToken) => new(authorization.Type);
        public ValueTask<DateTimeOffset?> GetCreationDateAsync(CustomAuthorization authorization, CancellationToken cancellationToken) => new(authorization.CreationDate);
        public ValueTask<ImmutableDictionary<string, JsonElement>> GetPropertiesAsync(CustomAuthorization authorization, CancellationToken cancellationToken) => new(ImmutableDictionary<string, JsonElement>.Empty);
        public ValueTask<string?> GetApplicationIdAsync(CustomAuthorization authorization, CancellationToken cancellationToken) => new(authorization.ApplicationId?.ToString());
        public ValueTask<ImmutableArray<string>> GetScopesAsync(CustomAuthorization authorization, CancellationToken cancellationToken) => new(ImmutableArray<string>.Empty);
        public ValueTask<CustomAuthorization> InstantiateAsync(CancellationToken cancellationToken) => new(new CustomAuthorization());
        public ValueTask SetApplicationIdAsync(CustomAuthorization authorization, string? identifier, CancellationToken cancellationToken)
        {
            authorization.ApplicationId = identifier != null ? Guid.Parse(identifier) : null;
            return ValueTask.CompletedTask;
        }
        public ValueTask SetCreationDateAsync(CustomAuthorization authorization, DateTimeOffset? date, CancellationToken cancellationToken)
        {
            authorization.CreationDate = date?.UtcDateTime;
            return ValueTask.CompletedTask;
        }
        public ValueTask SetPropertiesAsync(CustomAuthorization authorization, ImmutableDictionary<string, JsonElement> properties, CancellationToken cancellationToken)
        {
            authorization.Properties = properties.IsEmpty ? null : System.Text.Json.JsonSerializer.Serialize(properties);
            return ValueTask.CompletedTask;
        }
        public ValueTask SetScopesAsync(CustomAuthorization authorization, ImmutableArray<string> scopes, CancellationToken cancellationToken)
        {
            authorization.ScopesJson = scopes.IsDefaultOrEmpty ? null : System.Text.Json.JsonSerializer.Serialize(scopes);
            return ValueTask.CompletedTask;
        }
        public ValueTask SetStatusAsync(CustomAuthorization authorization, string? status, CancellationToken cancellationToken)
        {
            authorization.Status = status ?? OpenIddictConstants.Statuses.Valid;
            return ValueTask.CompletedTask;
        }
        public ValueTask SetSubjectAsync(CustomAuthorization authorization, string? subject, CancellationToken cancellationToken)
        {
            authorization.Subject = subject ?? throw new InvalidOperationException("Subject is required");
            return ValueTask.CompletedTask;
        }
        public ValueTask SetTypeAsync(CustomAuthorization authorization, string? type, CancellationToken cancellationToken)
        {
            authorization.Type = type ?? OpenIddictConstants.AuthorizationTypes.AdHoc;
            return ValueTask.CompletedTask;
        }

        // Additional required methods
        public IAsyncEnumerable<CustomAuthorization> FindAsync(string? subject, string? client, string? status, string? type, ImmutableArray<string>? scopes, CancellationToken cancellationToken) => AsyncEnumerable<CustomAuthorization>.Empty;
        public ValueTask<long> PruneAsync(DateTimeOffset threshold, CancellationToken cancellationToken) => new(0);
        public ValueTask<long> RevokeAsync(string? subject, string? client, string? status, string? type, CancellationToken cancellationToken) => new(0);
        public ValueTask<long> RevokeByApplicationIdAsync(string identifier, CancellationToken cancellationToken) => new(0);
        public ValueTask<long> RevokeBySubjectAsync(string subject, CancellationToken cancellationToken) => new(0);
    }

    public sealed class CustomTokenStore : IOpenIddictTokenStore<CustomToken>
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public CustomTokenStore(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public ValueTask<long> CountAsync(CancellationToken cancellationToken) => new(0);
        public ValueTask<long> CountAsync<TResult>(Func<IQueryable<CustomToken>, IQueryable<TResult>> query, CancellationToken cancellationToken) => new(0L);

        public async ValueTask CreateAsync(CustomToken token, CancellationToken cancellationToken)
        {
            Console.WriteLine($"CreateAsync(CustomToken) called with Status: {token.Status}, Type: {token.Type}");
    
            // Set required fields if missing
            if (token.Id == Guid.Empty)
            {
                token.Id = Guid.NewGuid();
                Console.WriteLine($"Set default Id: {token.Id}");
            }
            
            if (token.CreatedAtUtc == default)
            {
                token.CreatedAtUtc = DateTime.UtcNow;
                Console.WriteLine($"Set default CreatedAtUtc: {token.CreatedAtUtc}");
            }

            // Set default values if missing
            if (string.IsNullOrEmpty(token.Status))
            {
                token.Status = OpenIddictConstants.Statuses.Valid;
                Console.WriteLine($"Set default Status: {token.Status}");
            }
            
            if (string.IsNullOrEmpty(token.Type))
            {
                token.Type = OidcConstants.TokenTypes.AuthorizationCode;
                Console.WriteLine($"Set default Type: {token.Type}");
            }

            try
            {
                using var connection = _connectionFactory.GetOpenConnection();
                await connection.ExecuteAsync(Sql.InsertToken, token, commandTimeout: 30);
                Console.WriteLine($"Token created successfully");
            }
            catch (SqlException ex)
            {
                // Log thêm context: id, type, status...
                Console.WriteLine($"[TokenInsert] SqlError {ex.Number}: {ex.Message}. TokenId={token.Id}, Type={token.Type}, Status={token.Status}");
                throw; // bắt buộc rethrow, không được nuốt lỗi
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TokenInsert] Unexpected error: {ex.Message}");
                throw;
            }
        }

        private static string NormalizeTokenType(string? type, string @default = "authorization_code")
        {
            const string prefix = "urn:openiddict:params:oauth:token-type:";
            if (string.IsNullOrWhiteSpace(type))
                return @default;

            if (type.StartsWith(prefix, StringComparison.Ordinal))
            {
                var shortName = type.Substring(prefix.Length);
                return string.IsNullOrWhiteSpace(shortName) ? @default : shortName;
            }

            return type; // đã là short name

        }
        public async ValueTask DeleteAsync(CustomToken token, CancellationToken cancellationToken)
        {
            // Soft-revoke instead of hard delete to maintain auditability and prevent token re-use.
            using var connection = _connectionFactory.GetOpenConnection();
            await connection.ExecuteAsync(Sql.RevokeToken, new { token.Id });
        }

        public async ValueTask<CustomToken?> FindByIdAsync(string identifier, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.GetOpenConnection();
            return await connection.QueryFirstOrDefaultAsync<CustomToken>(Sql.GetTokenById, new { Id = Guid.Parse(identifier) });
        }

        public async ValueTask<CustomToken?> FindByReferenceIdAsync(string identifier, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.GetOpenConnection();
            return await connection.QueryFirstOrDefaultAsync<CustomToken>(Sql.GetTokenByReferenceId, new { ReferenceId = identifier });
        }

        public async IAsyncEnumerable<CustomToken> FindByApplicationIdAsync(string identifier, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.GetOpenConnection();
            var rows = await connection.QueryAsync<CustomToken>(Sql.GetTokensByApplicationId, new { ApplicationId = Guid.Parse(identifier) });
            foreach (var row in rows)
            {
                yield return row;
            }
        }

        public async IAsyncEnumerable<CustomToken> FindByAuthorizationIdAsync(string identifier, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.GetOpenConnection();
            var rows = await connection.QueryAsync<CustomToken>(Sql.GetTokensByAuthorizationId, new { AuthorizationId = Guid.Parse(identifier) });
            foreach (var row in rows)
            {
                yield return row;
            }
        }

        public async IAsyncEnumerable<CustomToken> FindBySubjectAsync(string subject, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.GetOpenConnection();
            var rows = await connection.QueryAsync<CustomToken>(Sql.GetTokensBySubject, new { Subject = subject });
            foreach (var row in rows)
            {
                yield return row;
            }
        }

        public ValueTask<TResult> GetAsync<TState, TResult>(Func<IQueryable<CustomToken>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
            => throw new NotImplementedException();

        public IAsyncEnumerable<CustomToken> ListAsync(int? count, int? offset, CancellationToken cancellationToken) => AsyncEnumerable<CustomToken>.Empty;
        public IAsyncEnumerable<TResult> ListAsync<TState, TResult>(Func<IQueryable<CustomToken>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken) => throw new NotImplementedException();

        public async ValueTask<long> PruneAsync(DateTimeOffset threshold, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.GetOpenConnection();
            var affectedRows = await connection.ExecuteAsync(Sql.PruneTokens, new { Threshold = threshold.UtcDateTime });
            return affectedRows;
        }

        public async ValueTask UpdateAsync(CustomToken token, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.GetOpenConnection();
            await connection.ExecuteAsync(Sql.UpdateToken, token);
        }

        // Additional required methods
        public ValueTask<string?> GetIdAsync(CustomToken token, CancellationToken cancellationToken) => new(token.Id.ToString());
        public ValueTask<string?> GetSubjectAsync(CustomToken token, CancellationToken cancellationToken) => new(token.Subject);
        public ValueTask<string?> GetStatusAsync(CustomToken token, CancellationToken cancellationToken) => new(token.Status);
        public ValueTask<string?> GetTypeAsync(CustomToken token, CancellationToken cancellationToken) => new(token.Type);
        public ValueTask<DateTimeOffset?> GetCreationDateAsync(CustomToken token, CancellationToken cancellationToken) => new(token.CreationDate);
        public ValueTask<DateTimeOffset?> GetExpirationDateAsync(CustomToken token, CancellationToken cancellationToken) => new(token.ExpirationDate);
        public ValueTask<DateTimeOffset?> GetRedemptionDateAsync(CustomToken token, CancellationToken cancellationToken) => new(token.RedemptionDate);
        public ValueTask<string?> GetReferenceIdAsync(CustomToken token, CancellationToken cancellationToken) => new(token.ReferenceId);
        public ValueTask<string?> GetPayloadAsync(CustomToken token, CancellationToken cancellationToken) => new(token.Payload);
        public ValueTask<ImmutableDictionary<string, JsonElement>> GetPropertiesAsync(CustomToken token, CancellationToken cancellationToken) => new(ImmutableDictionary<string, JsonElement>.Empty);
        public ValueTask<string?> GetApplicationIdAsync(CustomToken token, CancellationToken cancellationToken) => new(token.ApplicationId?.ToString());
        public ValueTask<string?> GetAuthorizationIdAsync(CustomToken token, CancellationToken cancellationToken) => new(token.AuthorizationId?.ToString());
        public ValueTask<CustomToken> InstantiateAsync(CancellationToken cancellationToken) => new(new CustomToken());
        public ValueTask SetApplicationIdAsync(CustomToken token, string? identifier, CancellationToken cancellationToken)
        {
            token.ApplicationId = identifier != null ? Guid.Parse(identifier) : null;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetAuthorizationIdAsync(CustomToken token, string? identifier, CancellationToken cancellationToken)
        {
            token.AuthorizationId = identifier != null ? Guid.Parse(identifier) : null;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetCreationDateAsync(CustomToken token, DateTimeOffset? date, CancellationToken cancellationToken)
        {
            token.CreationDate = date?.UtcDateTime;
            token.CreatedAtUtc = DateTime.UtcNow; // Set default here
            return ValueTask.CompletedTask;
        }

        public ValueTask SetExpirationDateAsync(CustomToken token, DateTimeOffset? date, CancellationToken cancellationToken)
        {
            token.ExpirationDate = date?.UtcDateTime;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetPayloadAsync(CustomToken token, string? payload, CancellationToken cancellationToken)
        {
            token.Payload = payload;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetPropertiesAsync(CustomToken token, ImmutableDictionary<string, JsonElement> properties, CancellationToken cancellationToken)
        {
            token.Properties = properties.IsEmpty ? null : System.Text.Json.JsonSerializer.Serialize(properties);
            return ValueTask.CompletedTask;
        }

        public ValueTask SetRedemptionDateAsync(CustomToken token, DateTimeOffset? date, CancellationToken cancellationToken)
        {
            token.RedemptionDate = date?.UtcDateTime;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetReferenceIdAsync(CustomToken token, string? identifier, CancellationToken cancellationToken)
        {
            token.ReferenceId = identifier;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetStatusAsync(CustomToken token, string? status, CancellationToken cancellationToken)
        {
            token.Status = status ?? OpenIddictConstants.Statuses.Valid;
            return ValueTask.CompletedTask;
        }

        public ValueTask SetSubjectAsync(CustomToken token, string? subject, CancellationToken cancellationToken)
        {
            token.Subject = subject ?? throw new InvalidOperationException("Subject is required");
            return ValueTask.CompletedTask;
        }

        public ValueTask SetTypeAsync(CustomToken token, string? type, CancellationToken cancellationToken)
        {
            token.Type = NormalizeTokenType(type, "authorization_code");
            return ValueTask.CompletedTask;
        }

        public IAsyncEnumerable<CustomToken> FindAsync(string? subject, string? client, string? status, string? type, CancellationToken cancellationToken) => AsyncEnumerable<CustomToken>.Empty;
        public ValueTask<long> RevokeAsync(string? subject, string? client, string? status, string? type, CancellationToken cancellationToken) => new(0);
        public ValueTask<long> RevokeByApplicationIdAsync(string identifier, CancellationToken cancellationToken) => new(0);
        public ValueTask<long> RevokeByAuthorizationIdAsync(string identifier, CancellationToken cancellationToken) => new(0);
        public ValueTask<long> RevokeBySubjectAsync(string subject, CancellationToken cancellationToken) => new(0);
    }

    // Helper class for IAsyncEnumerable
    internal class AsyncEnumerable<T> : IAsyncEnumerable<T>
    {
        private readonly IEnumerable<T> _source;

        public AsyncEnumerable(IEnumerable<T> source)
        {
            _source = source;
        }

        public static IAsyncEnumerable<T> Empty => new AsyncEnumerable<T>(Enumerable.Empty<T>());

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new AsyncEnumerator<T>(_source.GetEnumerator());
        }
    }

    internal class AsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;

        public AsyncEnumerator(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
        }

        public T Current => _enumerator.Current;

        public ValueTask DisposeAsync()
        {
            _enumerator.Dispose();
            return ValueTask.CompletedTask;
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(_enumerator.MoveNext());
        }
    }
}


