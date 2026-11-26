namespace Mmdddshop.API.Configuration.Authorization
{
    /// <summary>
    /// Authorization policy names, named by capability rather than by role.
    /// Controllers use [Authorize(Policy = Policies.X)]; the role (or, later, permission)
    /// requirements behind each policy are defined in Program.cs.
    /// </summary>
    public static class Policies
    {
        public const string ManageUsers = nameof(ManageUsers);

        public const string ManageOrders = nameof(ManageOrders);

        public const string ViewOrders = nameof(ViewOrders);
    }
}
