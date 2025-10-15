namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity
{
    public class IdTokenSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Identity token audience: The client ID of the application that will receive the token.
        /// API Resource ID
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;
    }
}
