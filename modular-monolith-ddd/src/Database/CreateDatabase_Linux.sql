IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ModularMonolithDDD')
BEGIN
    CREATE DATABASE [ModularMonolithDDD]
    CONTAINMENT = NONE;
    PRINT 'Database ModularMonolithDDD created.';    
END
ELSE
BEGIN
    PRINT 'Database ModularMonolithDDD already exists.';
END
GO

USE [ModularMonolithDDD]
GO

IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'app_admin')
BEGIN
    CREATE LOGIN app_admin WITH PASSWORD = 'Alo1234567@@';
END
ELSE
BEGIN
    PRINT 'Login app_admin already exists.';
END  
GO   

USE [ModularMonolithDDD]
GO
    
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'app_admin')
BEGIN
    CREATE USER app_admin FOR LOGIN app_admin WITH DEFAULT_SCHEMA = dbo;    
END
ELSE
BEGIN
    PRINT 'User app_admin already exists.';
END

USE [ModularMonolithDDD]
GO

IF NOT EXISTS (
    SELECT * 
    FROM sys.database_role_members drm
    JOIN sys.database_principals r ON drm.role_principal_id = r.principal_id
    JOIN sys.database_principals u ON drm.member_principal_id = u.principal_id
    WHERE r.name = 'db_owner' AND u.name = 'app_admin')
    BEGIN
        ALTER ROLE db_owner ADD MEMBER app_admin;       
    END    

USE [ModularMonolithDDD]
GO     