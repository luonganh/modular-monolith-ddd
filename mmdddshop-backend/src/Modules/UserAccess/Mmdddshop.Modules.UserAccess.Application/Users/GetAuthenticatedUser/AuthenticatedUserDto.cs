namespace Mmdddshop.Modules.UserAccess.Application.Users.GetAuthenticatedUser
{
    public class AuthenticatedUserDto
    {
        public Guid Id { get; set; }

        public bool IsActive { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Login { get; set; } = string.Empty;

        public string? Email { get; set; }

        public List<Claim> Claims { get; set; } = new();

        public List<UserRoleDto> Roles { get; set; } = new();   
    }
}