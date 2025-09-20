/*
    OpenIddict custom stores - SQL Server schema (for Dapper-based stores)

    Tables:
      - OpenIddictApplications
      - OpenIddictScopes
      - OpenIddictTokens

    Notes:
      - Variable-length collections (redirect URIs, permissions, resources, etc.) are stored as JSON (NVARCHAR(MAX)).
      - Use UTC for all datetime columns.
*/

USE [ModularMonolithDDD]
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

IF OBJECT_ID(N'users.OpenIddictApplications', N'U') IS NULL
BEGIN
    CREATE TABLE users.OpenIddictApplications
    (
        Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_OpenIddictApplications_Id DEFAULT NEWID(),

        -- OAuth client identifier (public information)
        ClientId NVARCHAR(100) NOT NULL,

        -- Public/Confidential
        ClientType NVARCHAR(50) NOT NULL,

        -- Implicit/External/Explicit/System
        ConsentType NVARCHAR(50) NULL,

        DisplayName NVARCHAR(200) NULL,

        -- Confidential clients only (hashed or encrypted in your app if needed)
        ClientSecret NVARCHAR(MAX) NULL,

        -- JSON arrays/objects
        RedirectUrisJson NVARCHAR(MAX) NULL,
        PostLogoutRedirectUrisJson NVARCHAR(MAX) NULL,
        PermissionsJson NVARCHAR(MAX) NULL,
        RequirementsJson NVARCHAR(MAX) NULL,

        CreatedAtUtc DATETIME2(0) NOT NULL CONSTRAINT DF_OpenIddictApplications_CreatedAtUtc DEFAULT SYSUTCDATETIME(),
        UpdatedAtUtc DATETIME2(0) NULL,

        CONSTRAINT PK_OpenIddictApplications PRIMARY KEY CLUSTERED (Id ASC)
    );

    CREATE UNIQUE NONCLUSTERED INDEX UX_OpenIddictApplications_ClientId
        ON users.OpenIddictApplications (ClientId ASC);
END
GO

IF OBJECT_ID(N'users.OpenIddictScopes', N'U') IS NULL
BEGIN
    CREATE TABLE users.OpenIddictScopes
    (
        Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_OpenIddictScopes_Id DEFAULT NEWID(),

        Name NVARCHAR(200) NOT NULL,
        DisplayName NVARCHAR(200) NULL,
        Description NVARCHAR(1000) NULL,

        -- Space-separated or JSON array of resources; we store JSON
        ResourcesJson NVARCHAR(MAX) NULL,

        CreatedAtUtc DATETIME2(0) NOT NULL CONSTRAINT DF_OpenIddictScopes_CreatedAtUtc DEFAULT SYSUTCDATETIME(),
        UpdatedAtUtc DATETIME2(0) NULL,

        CONSTRAINT PK_OpenIddictScopes PRIMARY KEY CLUSTERED (Id ASC)
    );

    CREATE UNIQUE NONCLUSTERED INDEX UX_OpenIddictScopes_Name
        ON users.OpenIddictScopes (Name ASC);
    END
    GO

    IF OBJECT_ID(N'users.OpenIddictAuthorizations', N'U') IS NULL
    BEGIN
        CREATE TABLE users.OpenIddictAuthorizations
        (
            Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_OpenIddictAuthorizations_Id DEFAULT NEWID(),

            ApplicationId UNIQUEIDENTIFIER NULL,
            Subject NVARCHAR(200) NOT NULL,
            Status NVARCHAR(50) NOT NULL,
            Type NVARCHAR(50) NOT NULL,

            ScopesJson NVARCHAR(MAX) NULL,
            Properties NVARCHAR(MAX) NULL,

            CreationDate DATETIME2(0) NULL,

            CreatedAtUtc DATETIME2(0) NOT NULL CONSTRAINT DF_OpenIddictAuthorizations_CreatedAtUtc DEFAULT SYSUTCDATETIME(),
            UpdatedAtUtc DATETIME2(0) NULL,

            CONSTRAINT PK_OpenIddictAuthorizations PRIMARY KEY CLUSTERED (Id ASC),
            CONSTRAINT FK_OpenIddictAuthorizations_Application
                FOREIGN KEY (ApplicationId) REFERENCES users.OpenIddictApplications (Id)
                    ON DELETE CASCADE
        );

        CREATE NONCLUSTERED INDEX IX_OpenIddictAuthorizations_ApplicationId
            ON users.OpenIddictAuthorizations (ApplicationId ASC);

        CREATE NONCLUSTERED INDEX IX_OpenIddictAuthorizations_Subject
            ON users.OpenIddictAuthorizations (Subject ASC);

        CREATE NONCLUSTERED INDEX IX_OpenIddictAuthorizations_Status
            ON users.OpenIddictAuthorizations (Status ASC);
    END
    GO

    IF OBJECT_ID(N'users.OpenIddictTokens', N'U') IS NULL
BEGIN
    CREATE TABLE users.OpenIddictTokens
    (
        Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_OpenIddictTokens_Id DEFAULT NEWID(),

        -- Optional link to application/client
        ApplicationId UNIQUEIDENTIFIER NULL,

        -- Optional link to an authorization (if you store it)
        AuthorizationId UNIQUEIDENTIFIER NULL,

        -- Token metadata
        Type NVARCHAR(50) NOT NULL,           -- access_token, refresh_token, device_code, etc.
        Status NVARCHAR(50) NULL,             -- active, redeemed, revoked, etc.
        Subject NVARCHAR(200) NULL,           -- user/subject identifier
        ReferenceId NVARCHAR(100) NULL,       -- hashed reference token id (unique when present)

        -- Payload/Primitives
        Payload NVARCHAR(MAX) NULL,           -- serialized payload if you need to store it
        Properties NVARCHAR(MAX) NULL,        -- optional JSON metadata

        -- Timestamps (UTC)
        CreationDate DATETIME2(0) NULL,
        ExpirationDate DATETIME2(0) NULL,
        RedemptionDate DATETIME2(0) NULL,

        CreatedAtUtc DATETIME2(0) NOT NULL CONSTRAINT DF_OpenIddictTokens_CreatedAtUtc DEFAULT SYSUTCDATETIME(),
        UpdatedAtUtc DATETIME2(0) NULL,

        CONSTRAINT PK_OpenIddictTokens PRIMARY KEY CLUSTERED (Id ASC),
        CONSTRAINT FK_OpenIddictTokens_Application
            FOREIGN KEY (ApplicationId) REFERENCES users.OpenIddictApplications (Id)
                ON DELETE SET NULL
    );

    -- For fast lookup by client
    CREATE NONCLUSTERED INDEX IX_OpenIddictTokens_ApplicationId
        ON users.OpenIddictTokens (ApplicationId ASC);

    -- For querying tokens by subject
    CREATE NONCLUSTERED INDEX IX_OpenIddictTokens_Subject
        ON users.OpenIddictTokens (Subject ASC);

    -- Uniqueness for reference tokens
    CREATE UNIQUE NONCLUSTERED INDEX UX_OpenIddictTokens_ReferenceId
        ON users.OpenIddictTokens (ReferenceId ASC)
        WHERE ReferenceId IS NOT NULL;
END
GO


