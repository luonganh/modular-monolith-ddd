USE [ModularMonolithDDD]
GO

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'app')
BEGIN
    EXEC ('CREATE SCHEMA app AUTHORIZATION dbo;');
    PRINT 'Schema app created.';
END
ELSE
BEGIN
    PRINT 'Schema app already exists.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'users')
BEGIN
    EXEC ('CREATE SCHEMA users AUTHORIZATION dbo;');
    PRINT 'Schema users created.';
END
ELSE
BEGIN
    PRINT 'Schema users already exists.';
END
GO