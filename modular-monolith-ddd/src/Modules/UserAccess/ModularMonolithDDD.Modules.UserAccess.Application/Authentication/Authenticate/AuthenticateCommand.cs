namespace ModularMonolithDDD.Modules.UserAccess.Application.Authentication.Authenticate;

/// <summary>
/// Command to authenticate a user and return access token.
/// This command handles user login and generates JWT tokens for API access.
/// </summary>
public class AuthenticateCommand : CommandBase<AuthenticationResult>
{
    /// <summary>
    /// Initializes a new instance of the LoginCommand class.
    /// </summary>
    /// <param name="login">The login (username/email) of the user</param>
    /// <param name="password">The password of the user</param>
    public AuthenticateCommand(string login, string password)
    {
        Login = login;
        Password = password;
    }

    /// <summary>
    /// Gets the login (username/email) of the user.
    /// </summary>
    public string Login { get; }

    /// <summary>
    /// Gets the password of the user.
    /// </summary>
    public string Password { get; }
}
