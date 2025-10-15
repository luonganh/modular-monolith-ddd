namespace ModularMonolithDDD.Modules.UserAccess.Application.Authentication
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<Claim> Claims { get; set; } = new();
        public string Password { get; set; } = string.Empty;
        public List<UserRoleDto> Roles { get; set; } = new();
    }
}
