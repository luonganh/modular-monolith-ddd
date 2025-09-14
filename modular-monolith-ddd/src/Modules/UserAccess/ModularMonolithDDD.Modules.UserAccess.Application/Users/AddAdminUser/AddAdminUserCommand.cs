namespace ModularMonolithDDD.Modules.UserAccess.Application.Users.AddAdminUser
{
    /// <summary>
    /// Command to add a new administrator user to the system.
    /// This command creates a user with administrative privileges.
    /// </summary>
    public class AddAdminUserCommand : CommandBase
    {
        /// <summary>
        /// Initializes a new instance of the AddAdminUserCommand class.
        /// </summary>
        /// <param name="login">The unique login identifier for the admin user</param>
        /// <param name="password">The password for the admin user</param>
        /// <param name="firstName">The first name of the admin user</param>
        /// <param name="lastName">The last name of the admin user</param>
        /// <param name="name">The display name of the admin user</param>
        /// <param name="email">The email address of the admin user</param>
        public AddAdminUserCommand(
           string login,
           string password,
           string firstName,
           string lastName,
           string name,
           string email)
        {
            Login = login;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Name = name;
            Email = email;
        }

        /// <summary>
        /// Gets the unique login identifier for the admin user.
        /// </summary>
        public string Login { get; }

        /// <summary>
        /// Gets the first name of the admin user.
        /// </summary>
        public string FirstName { get; }

        /// <summary>
        /// Gets the last name of the admin user.
        /// </summary>
        public string LastName { get; }

        /// <summary>
        /// Gets the display name of the admin user.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the email address of the admin user.
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Gets the password for the admin user.
        /// </summary>
        public string Password { get; }
    }
}
