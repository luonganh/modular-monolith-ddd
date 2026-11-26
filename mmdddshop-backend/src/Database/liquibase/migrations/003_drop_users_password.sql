-- View v_Users đang SELECT cột Password nên phải sửa view trước khi drop cột
ALTER VIEW [users].[v_Users]
AS
SELECT
    [User].[Id],
    [User].[IsActive],
    [User].[Login],
    [User].[Email],
    [User].[Name]
FROM [users].[Users] AS [User]
GO

ALTER TABLE [users].[Users] DROP COLUMN [Password]
GO
