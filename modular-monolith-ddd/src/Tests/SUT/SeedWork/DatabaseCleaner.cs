// File: Tests/SUT/SeedWork/DatabaseCleaner.cs
// Summary: Utility for clearing state in the UserAccess schema between
// tests. Keep statements idempotent and safe when tables are empty.
namespace ModularMonolithDDD.Tests.SUT.SeedWork
{
    internal static class DatabaseCleaner
    {
        internal static async Task ClearAllData(IDbConnection connection)
        {
            await ClearUserAccess(connection);
        }

        private static async Task ClearUserAccess(IDbConnection connection)
        {
            var clearUserAccessSql =
                "DELETE FROM [useraccess].[InboxMessages] " +
                "DELETE FROM [useraccess].[InternalCommands] " +
                "DELETE FROM [useraccess].[OutboxMessages] " +
                "DELETE FROM [useraccess].[UserRoles] " +
                "DELETE FROM [useraccess].[Users] " +
                "DELETE FROM [useraccess].[Roles] " +
                "DELETE FROM [useraccess].[Permissions] " +
                "DELETE FROM [useraccess].[RolePermissions] ";

            await connection.ExecuteScalarAsync(clearUserAccessSql);
        }
    }
}
