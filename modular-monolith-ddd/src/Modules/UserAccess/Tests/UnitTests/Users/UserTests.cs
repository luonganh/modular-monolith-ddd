// File: Modules/UserAccess/Tests/UnitTests/Users/UserTests.cs
// Purpose: Unit-level checks for `User` aggregate invariants and
// constructor validation.
namespace ModularMonolithDDD.Modules.UserAccess.Tests.UnitTests.Users
{
    public class UserTests : TestBase
    {
        [Test]
        public void User_WhenCreated_ShouldHaveCorrectProperties()
        {
            // Arrange
            var userId = UserId.Of(Guid.NewGuid());
            var login = "testuser";
            var password = "password123";
            var name = "Test User";
            var email = "test@example.com";

            // Act
            var user = new User(userId, login, password, name, email);

            // Assert
            Assert.That(user.Id, Is.EqualTo(userId));
            Assert.That(user.Login, Is.EqualTo(login));
            Assert.That(user.Name, Is.EqualTo(name));
            Assert.That(user.Email, Is.EqualTo(email));
        }

        [Test]
        public void User_WhenCreatedWithEmptyLogin_ShouldThrowException()
        {
            // Arrange
            var userId = UserId.Of(Guid.NewGuid());
            var login = "";
            var password = "password123";
            var name = "Test User";
            var email = "test@example.com";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                new User(userId, login, password, name, email));
        }

        [Test]
        public void User_WhenCreatedWithInvalidEmail_ShouldThrowException()
        {
            // Arrange
            var userId = UserId.Of(Guid.NewGuid());
            var login = "testuser";
            var password = "password123";
            var name = "Test User";
            var email = "invalid-email";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                new User(userId, login, password, name, email));
        }

        [Test]
        public void User_WhenCreatedWithEmptyPassword_ShouldThrowException()
        {
            // Arrange
            var userId = UserId.Of(Guid.NewGuid());
            var login = "testuser";
            var password = "";
            var name = "Test User";
            var email = "test@example.com";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                new User(userId, login, password, name, email));
        }

        [Test]
        public void User_WhenCreatedWithEmptyName_ShouldThrowException()
        {
            // Arrange
            var userId = UserId.Of(Guid.NewGuid());
            var login = "testuser";
            var password = "password123";
            var name = "";
            var email = "test@example.com";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => 
                new User(userId, login, password, name, email));
        }
    }
}
