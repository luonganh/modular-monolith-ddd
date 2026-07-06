IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ModularMonolithDDDShop')
BEGIN
    CREATE DATABASE [ModularMonolithDDDShop]
    CONTAINMENT = NONE;
    PRINT 'Database ModularMonolithDDDShop created.';    
END
ELSE
BEGIN
    PRINT 'Database ModularMonolithDDDShop already exists.';
END
GO

USE [ModularMonolithDDDShop]
GO

IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'mmdddshop')
BEGIN
    CREATE LOGIN mmdddshop WITH PASSWORD = 'Alo1234567@@';
END
ELSE
BEGIN
    PRINT 'Login mmdddshop already exists.';
END  
GO   

USE [ModularMonolithDDDShop]
GO
    
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'mmdddshop')
BEGIN
    CREATE USER mmdddshop FOR LOGIN mmdddshop WITH DEFAULT_SCHEMA = dbo;    
END
ELSE
BEGIN
    PRINT 'User mmdddshop already exists.';
END

USE [ModularMonolithDDDShop]
GO

IF NOT EXISTS (
    SELECT * 
    FROM sys.database_role_members drm
    JOIN sys.database_principals r ON drm.role_principal_id = r.principal_id
    JOIN sys.database_principals u ON drm.member_principal_id = u.principal_id
    WHERE r.name = 'db_owner' AND u.name = 'mmdddshop')
    BEGIN
        ALTER ROLE db_owner ADD MEMBER mmdddshop;       
    END    

USE [ModularMonolithDDDShop]
GO     