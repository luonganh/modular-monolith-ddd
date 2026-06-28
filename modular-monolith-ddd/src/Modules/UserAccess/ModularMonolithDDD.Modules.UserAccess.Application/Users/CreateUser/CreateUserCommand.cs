namespace ModularMonolithDDD.Modules.UserAccess.Application.Users.CreateUser;

/// <summary>
/// Command to create a new regular user in the system.
/// This command creates a user with standard user privileges.
/// </summary>
public class CreateUserCommand : CommandBase
{
    /// <summary>
    /// Initializes a new instance of the CreateUserCommand class.
    /// </summary>
    /// <param name="userId">The unique identifier for the new user</param>
    /// <param name="login">The unique login identifier for the user</param>
    /// <param name="email">The email address of the user</param>
    /// <param name="firstName">The first name of the user</param>
    /// <param name="lastName">The last name of the user</param>
    /// <param name="password">The password for the user</param>
    public CreateUserCommand(
        Guid userId,
        string login,
        string email,
        string firstName,
        string lastName,
        string password)
    {
        UserId = userId;
        Login = login;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Password = password;
    }

    /// <summary>
    /// Gets the unique identifier for the new user.
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// Gets the unique login identifier for the user.
    /// </summary>
    public string Login { get; }

    /// <summary>
    /// Gets the email address of the user.
    /// </summary>
    public string Email { get; }

    /// <summary>
    /// Gets the first name of the user.
    /// </summary>
    public string FirstName { get; }

    /// <summary>
    /// Gets the last name of the user.
    /// </summary>
    public string LastName { get; }

    /// <summary>
    /// Gets the password for the user.
    /// </summary>
    public string Password { get; }
}