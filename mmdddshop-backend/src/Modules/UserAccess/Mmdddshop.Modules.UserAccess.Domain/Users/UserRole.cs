namespace Mmdddshop.Modules.UserAccess.Domain.Users
{
    /// <summary>
    /// User role value object.
    /// </summary>
    public class UserRole : ValueObject
    {       
        public static UserRole Administrator => new UserRole(nameof(Administrator));

        public static UserRole Manager => new UserRole(nameof(Manager));

        public static UserRole Staff => new UserRole(nameof(Staff));

        public static readonly IReadOnlySet<string> Known =
    new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Administrator", "Manager", "Staff" };

        public static bool IsKnown(string value) => Known.Contains(value);
       
        public string Value { get; }

        private UserRole(string value)
        {
            this.Value = value;
        }

        public static UserRole From(string value)
        {
            if (string.Equals(value, nameof(Administrator), StringComparison.OrdinalIgnoreCase)) return Administrator;
            if (string.Equals(value, nameof(Manager), StringComparison.OrdinalIgnoreCase)) return Manager;
            if (string.Equals(value, nameof(Staff), StringComparison.OrdinalIgnoreCase)) return Staff;
            throw new ArgumentException($"Unknown user role '{value}'.", nameof(value));
        }       
    }
}