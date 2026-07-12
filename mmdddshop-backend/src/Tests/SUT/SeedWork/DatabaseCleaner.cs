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
                "DELETE FROM [users].[InboxMessages] " +
                "DELETE FROM [users].[InternalCommands] " +
                "DELETE FROM [users].[OutboxMessages] " +
                "DELETE FROM [users].[UserRoles] " +
                "DELETE FROM [users].[Users] " +
                "DELETE FROM [users].[Roles] " +
                "DELETE FROM [users].[Permissions] " +
                "DELETE FROM [users].[RolePermissions] ";

            await connection.ExecuteScalarAsync(clearUserAccessSql);
        }
    }
}
