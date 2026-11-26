DELETE FROM [users].[UserRoles]
WHERE [RoleCode] NOT IN (N'Administrator', N'Manager', N'Staff')
GO