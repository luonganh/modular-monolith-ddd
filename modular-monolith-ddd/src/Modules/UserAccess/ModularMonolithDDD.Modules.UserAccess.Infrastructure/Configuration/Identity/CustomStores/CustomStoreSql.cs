namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity.CustomStores
{    
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


