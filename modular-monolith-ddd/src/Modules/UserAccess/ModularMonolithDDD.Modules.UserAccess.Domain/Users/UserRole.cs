namespace ModularMonolithDDD.Modules.UserAccess.Domain.Users
{
    /// <summary>
    /// User role value object.
    /// </summary>
    public class UserRole : ValueObject
    {
        public static UserRole Member => new UserRole(nameof(Member));

        public static UserRole Administrator => new UserRole(nameof(Administrator));

        public string Value { get; }

        private UserRole(string value)
        {
            this.Value = value;
        }

        public static UserRole From(string value)
        {
            return value switch
            {
                "Administrator" => Administrator,
                "Member" => Member,
                _ => new UserRole(value) // hợp lệ vì đang ở bên trong class
            };
        }
    }
}