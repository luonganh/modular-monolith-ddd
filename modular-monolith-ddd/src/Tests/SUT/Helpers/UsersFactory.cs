namespace ModularMonolithDDD.Tests.SUT.Helpers
{
    internal static class UsersFactory
    {
        public static async Task GivenAdmin(
            IUserAccessModule userAccessModule,
            string login,
            string password,
            string name,
            string firstName,
            string lastName,
            string email)
        {
            await userAccessModule.ExecuteCommandAsync(new AddAdminUserCommand(
                login,
                password,
                firstName,
                lastName,
                name,
                email));
        }

        public static async Task<Guid> GivenUser(
            IUserAccessModule userAccessModule,
            string connectionString,
            string login,
            string password,
            string firstName,
            string lastName,
            string email)
        {
            // For now, we'll use the same command as admin
            // In a real scenario, you might have a different command for regular users
            await userAccessModule.ExecuteCommandAsync(new AddAdminUserCommand(
                login,
                password,
                firstName,
                lastName,
                firstName + " " + lastName, // name
                email));

            // Return a mock user ID for now
            return Guid.NewGuid();
        }
    }
}
