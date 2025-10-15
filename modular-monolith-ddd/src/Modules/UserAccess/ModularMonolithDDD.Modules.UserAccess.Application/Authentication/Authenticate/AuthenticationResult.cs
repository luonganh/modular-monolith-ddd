namespace ModularMonolithDDD.Modules.UserAccess.Application.Authentication.Authenticate;

/// <summary>
/// Result of a login operation following grzybek pattern.
/// Contains authentication status, error message, and user information.
/// </summary>
public class AuthenticationResult
{
    /// <summary>
    /// Initializes a new instance of the LoginResult class for failed authentication.
    /// </summary>
    /// <param name="authenticationError">The authentication error message</param>
    public AuthenticationResult(string authenticationError)
    {
        IsAuthenticated = false;
        AuthenticationError = authenticationError;
    }

    /// <summary>
    /// Initializes a new instance of the LoginResult class for successful authentication.
    /// </summary>
    /// <param name="user">The authenticated user information</param>
    public AuthenticationResult(UserDto user)
    {
        IsAuthenticated = true;
        User = user;
    }

    /// <summary>
    /// Gets a value indicating whether the authentication was successful.
    /// </summary>
    public bool IsAuthenticated { get; }

    /// <summary>
    /// Gets the authentication error message if authentication failed.
    /// </summary>
    public string? AuthenticationError { get; }

    /// <summary>
    /// Gets the authenticated user information if authentication was successful.
    /// </summary>
    public UserDto? User { get; }
}

