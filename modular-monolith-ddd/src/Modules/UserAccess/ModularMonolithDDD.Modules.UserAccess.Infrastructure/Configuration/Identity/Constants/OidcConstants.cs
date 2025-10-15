namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Configuration.Identity.Constants
{
   public static class OidcConstants
   {
        public static class TokenTypes
        {
            public const string AuthorizationCode = "authorization_code";
            public const string AccessToken = "access_token";
            public const string RefreshToken = "refresh_token";
            public const string IdToken = "id_token";
        }

        public static class Statuses
        {
            public const string Valid = "valid";
            public const string Invalid = "invalid";
        }
    }
}
