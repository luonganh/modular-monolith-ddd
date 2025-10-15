namespace ModularMonolithDDD.API.Modules.UserAccess
{
    /// <summary>
    /// Data Transfer Object (DTO) for user authentication requests.
    /// Contains the login credentials and return URL needed for the authentication process.
    /// </summary>
    public class AuthenticateRequest
    {
        /// <summary>
        /// Gets or sets the user's login identifier (username or email).
        /// </summary>
        public string Login { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's password for authentication.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the URL to redirect the user after successful authentication (used in OAuth flow).
        /// </summary>
        public string ReturnUrl { get; set; } = string.Empty;
    }
}
