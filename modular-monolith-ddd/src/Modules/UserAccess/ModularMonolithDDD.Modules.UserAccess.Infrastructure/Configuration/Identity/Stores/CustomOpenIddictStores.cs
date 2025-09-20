namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity.Stores
{
    // Minimal POCOs used by the custom stores. These are NOT EF entities.

    /// <summary>
    /// Store OAuth clients information.
    /// </summary>
    public sealed class CustomApplication
    {
        public Guid Id { get; set; }
        public string ClientId { get; set; }
        public string ClientType { get; set; }
        public string ConsentType { get; set; }
        public string DisplayName { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUrisJson { get; set; }
        public string PostLogoutRedirectUrisJson { get; set; }
        public string PermissionsJson { get; set; }
        public string RequirementsJson { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }

        // Navigation properties for EF relationships
        public ICollection<CustomAuthorization> Authorizations { get; set; } = new List<CustomAuthorization>();
        public ICollection<CustomToken> Tokens { get; set; } = new List<CustomToken>();
    }

    /// <summary>
    /// Store OAuth scopes, permissions information.
    /// </summary>
    public sealed class CustomScope
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string ResourcesJson { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }
    }

    /// <summary>
    /// Store authorizations (grants) information.
    /// </summary>
    public sealed class CustomAuthorization
    {
        public Guid Id { get; set; }
        public Guid? ApplicationId { get; set; }
        public string Subject { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string ScopesJson { get; set; }
        public string Properties { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }

        // Navigation properties for EF relationships
        public CustomApplication Application { get; set; }
        public ICollection<CustomToken> Tokens { get; set; } = new List<CustomToken>();
    }

    /// <summary>
    /// Store access and refresh tokens information.   
    /// </summary>
    public sealed class CustomToken
    {
        public Guid Id { get; set; }
        public Guid? ApplicationId { get; set; }
        public Guid? AuthorizationId { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Subject { get; set; }
        public string ReferenceId { get; set; }
        public string Payload { get; set; }
        public string Properties { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? RedemptionDate { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }

        // Navigation properties for EF relationships
        public CustomApplication Application { get; set; }
        public CustomAuthorization Authorization { get; set; }
    }

    internal static class Sql
    {
        public const string InsertApplication = @"INSERT INTO users.OpenIddictApplications
            (Id, ClientId, ClientType, ConsentType, DisplayName, ClientSecret, RedirectUrisJson, PostLogoutRedirectUrisJson, PermissionsJson, RequirementsJson)
            VALUES (@Id, @ClientId, @ClientType, @ConsentType, @DisplayName, @ClientSecret, @RedirectUrisJson, @PostLogoutRedirectUrisJson, @PermissionsJson, @RequirementsJson)";

        public const string GetApplicationByClientId = @"SELECT TOP 1 * FROM users.OpenIddictApplications WHERE ClientId = @ClientId";

        public const string InsertScope = @"INSERT INTO users.OpenIddictScopes
            (Id, Name, DisplayName, Description, ResourcesJson)
            VALUES (@Id, @Name, @DisplayName, @Description, @ResourcesJson)";

        public const string GetScopeByName = @"SELECT TOP 1 * FROM users.OpenIddictScopes WHERE Name = @Name";

        // Authorizations
        public const string InsertAuthorization = @"INSERT INTO users.OpenIddictAuthorizations
            (Id, ApplicationId, Subject, Status, Type, ScopesJson, Properties, CreationDate, CreatedAtUtc)
            VALUES (@Id, @ApplicationId, @Subject, @Status, @Type, @ScopesJson, @Properties, @CreationDate, @CreatedAtUtc)";

        public const string GetAuthorizationById = @"SELECT TOP 1 * FROM users.OpenIddictAuthorizations WHERE Id = @Id";
        public const string GetAuthorizationsByApplicationId = @"SELECT * FROM users.OpenIddictAuthorizations WHERE ApplicationId = @ApplicationId";
        public const string GetAuthorizationsBySubject = @"SELECT * FROM users.OpenIddictAuthorizations WHERE Subject = @Subject";
        public const string UpdateAuthorization = @"UPDATE users.OpenIddictAuthorizations 
            SET ApplicationId = @ApplicationId, Subject = @Subject, Status = @Status, Type = @Type, 
                ScopesJson = @ScopesJson, Properties = @Properties, CreationDate = @CreationDate, UpdatedAtUtc = @UpdatedAtUtc
            WHERE Id = @Id";
        public const string DeleteAuthorization = @"DELETE FROM users.OpenIddictAuthorizations WHERE Id = @Id";

        // Tokens
        public const string InsertToken = @"INSERT INTO users.OpenIddictTokens
            (Id, ApplicationId, AuthorizationId, Type, Status, Subject, ReferenceId, Payload, Properties, CreationDate, ExpirationDate, RedemptionDate)
            VALUES(@Id, @ApplicationId, @AuthorizationId, @Type, @Status, @Subject, @ReferenceId, @Payload, @Properties, @CreationDate, @ExpirationDate, @RedemptionDate)";

        public const string GetTokenById = @"SELECT TOP 1 * FROM users.OpenIddictTokens WHERE Id = @Id";
        public const string GetTokenByReferenceId = @"SELECT TOP 1 * FROM users.OpenIddictTokens WHERE ReferenceId = @ReferenceId";
        public const string GetTokensBySubject = @"SELECT * FROM users.OpenIddictTokens WHERE Subject = @Subject";
        public const string GetTokensByApplicationId = @"SELECT * FROM users.OpenIddictTokens WHERE ApplicationId = @ApplicationId";
        public const string GetTokensByAuthorizationId = @"SELECT * FROM users.OpenIddictTokens WHERE AuthorizationId = @AuthorizationId";

        public const string UpdateToken = @"UPDATE users.OpenIddictTokens SET
            ApplicationId = @ApplicationId,
            AuthorizationId = @AuthorizationId,
            Type = @Type,
            Status = @Status,
            Subject = @Subject,
            ReferenceId = @ReferenceId,
            Payload = @Payload,
            Properties = @Properties,
            CreationDate = @CreationDate,
            ExpirationDate = @ExpirationDate,
            RedemptionDate = @RedemptionDate,
            UpdatedAtUtc = SYSUTCDATETIME()
            WHERE Id = @Id";

        public const string DeleteToken = @"DELETE FROM users.OpenIddictTokens WHERE Id = @Id";

        public const string RevokeToken = @"UPDATE users.OpenIddictTokens SET
            Status = 'revoked',
            RedemptionDate = SYSUTCDATETIME(),
            UpdatedAtUtc = SYSUTCDATETIME()
            WHERE Id = @Id";

        public const string PruneTokens = @"DELETE FROM users.OpenIddictTokens
            WHERE (ExpirationDate IS NOT NULL AND ExpirationDate < @Threshold)
               OR (Status = 'revoked')";
    }
}


