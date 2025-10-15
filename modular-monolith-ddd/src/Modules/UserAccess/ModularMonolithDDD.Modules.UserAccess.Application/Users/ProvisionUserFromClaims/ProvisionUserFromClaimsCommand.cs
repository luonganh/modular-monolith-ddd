namespace ModularMonolithDDD.Modules.UserAccess.Application.Users.ProvisionUserFromClaims
{
    public class ProvisionUserFromClaimsCommand : CommandBase
    {
        public ProvisionUserFromClaimsCommand(
        string externalId,       
        string username,
        string email,
        string firstName,
        string lastName,
        string displayName,
        List<string> roles)
        {
            ExternalId = externalId;           
            Username = username;
            Email = email;
            DisplayName = displayName;
            FirstName = firstName;
            LastName = lastName;
            Roles = roles;
        }

        public string ExternalId { get; }
        //public string IdentityProvider { get; }
        public string Username { get; }
        public string Email { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string DisplayName { get; }
        public List<string> Roles { get; }
    }
}
