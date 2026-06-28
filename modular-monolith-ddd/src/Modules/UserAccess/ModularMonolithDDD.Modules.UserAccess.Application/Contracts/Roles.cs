namespace ModularMonolithDDD.Modules.UserAccess.Application.Contracts
{
    /// <summary>
    /// Defines the available user roles in the system.
    /// Contains constants for role names to ensure consistency across the application.
    /// </summary>
    public class Roles
    {
        /// <summary>
        /// Administrator role with full system access.
        /// </summary>
        public const string Admin = "Admin";

        /// <summary>
        /// Standard user role with limited access.
        /// </summary>
        public const string User = "User";
    }
}
