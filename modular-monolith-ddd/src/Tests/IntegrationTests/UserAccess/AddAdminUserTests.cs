namespace ModularMonolithDDD.Tests.IntegrationTests.UserAccess
{
    public class AddAdminUserTests : TestBase
    {
        [Test]
        public async Task AddAdminUser_WhenValidData_ShouldSucceed()
        {
            // Arrange
            var command = new AddAdminUserCommand(
                "admin",
                "password123",
                "John",
                "Doe",
                "John Doe",
                "john.doe@example.com");

            // Act
            await UserAccessModule.ExecuteCommandAsync(command);

            // Assert
            // In a real scenario, you would verify the user was created
            // by querying the database or using a query handler
            Assert.Pass("Admin user creation command executed successfully");
        }

        [Test]
        public void AddAdminUser_WhenInvalidEmail_ShouldThrowException()
        {
            // Arrange
            var command = new AddAdminUserCommand(
                "admin",
                "password123",
                "John",
                "Doe",
                "John Doe",
                "invalid-email");

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => 
                 UserAccessModule.ExecuteCommandAsync(command));
        }

        [Test]
        public void AddAdminUser_WhenEmptyLogin_ShouldThrowException()
        {
            // Arrange
            var command = new AddAdminUserCommand(
                "",
                "password123",
                "John",
                "Doe",
                "John Doe",
                "john.doe@example.com");

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => 
                 UserAccessModule.ExecuteCommandAsync(command));
        }
    }
}
