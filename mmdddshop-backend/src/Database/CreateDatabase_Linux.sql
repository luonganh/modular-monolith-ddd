IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '$(DB_NAME)')
BEGIN
    CREATE DATABASE [$(DB_NAME)]
    CONTAINMENT = NONE;
    PRINT 'Database $(DB_NAME) created.';    
END
ELSE
BEGIN
    PRINT 'Database $(DB_NAME) already exists.';
END
GO


IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = '$(APP_USER)')
BEGIN
    CREATE LOGIN [$(APP_USER)] WITH PASSWORD = '$(APP_PASSWORD)';
END
ELSE
BEGIN
    ALTER LOGIN [$(APP_USER)] WITH PASSWORD = '$(APP_PASSWORD)';
    PRINT 'Login $(APP_USER) already exists. - password synced.';
END  
GO   

USE [$(DB_NAME)]
GO
    
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = '$(APP_USER)')
BEGIN
    CREATE USER [$(APP_USER)] FOR LOGIN [$(APP_USER)] WITH DEFAULT_SCHEMA = dbo;    
END
ELSE
BEGIN
    PRINT 'User $(APP_USER) already exists.';
END

USE [$(DB_NAME)]
GO

IF NOT EXISTS (
    SELECT * 
    FROM sys.database_role_members drm
    JOIN sys.database_principals r ON drm.role_principal_id = r.principal_id
    JOIN sys.database_principals u ON drm.member_principal_id = u.principal_id
    WHERE r.name = 'db_owner' AND u.name = '$(APP_USER)')
    BEGIN
        ALTER ROLE db_owner ADD MEMBER [$(APP_USER)];       
    END    

USE [$(DB_NAME)]
GO  