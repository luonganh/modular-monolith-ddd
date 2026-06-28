namespace ModularMonolithDDD.Modules.UserAccess.Domain.Users
{
    /// <summary>
    /// User role value object.
    /// </summary>
    public class UserRole : ValueObject
    {       
        public static UserRole Administrator => new UserRole(nameof(Administrator));

        public static UserRole Manager => new UserRole(nameof(Manager));

        public static UserRole Staff => new UserRole(nameof(Staff));


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
                "Manager" => Manager,
                "Staff" => Staff,
                _ => new UserRole(value) // valid because inside the class
            };
        }
    }
}