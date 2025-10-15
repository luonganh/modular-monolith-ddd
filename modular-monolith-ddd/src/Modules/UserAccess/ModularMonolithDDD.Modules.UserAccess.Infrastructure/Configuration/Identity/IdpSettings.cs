namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity
{
    /// <summary>
    /// Identity Provider settings for authentication endpoints
    /// </summary>
    public class IdpSettings
    {
        /// <summary>
        /// Identity Provider login URL
        /// https://localhost:5000/login
        /// </summary>
        public string IdpLoginUrl { get; set; } = string.Empty;

        /// <summary>
        /// Identity Provider base URL
        /// https://localhost:5000
        /// </summary>
        public string IdpBaseUrl { get; set; } = string.Empty;

        /// <summary>
        /// Identity Provider authorization endpoint
        /// https://localhost:5000/connect/authorize
        /// </summary>
        public string IdpAuthorizationEndpoint { get; set; } = string.Empty;

        /// <summary>
        /// Identity Provider token endpoint
        /// https://localhost:5000/connect/token
        /// </summary>
        public string IdpTokenEndpoint { get; set; } = string.Empty;
    }
}
