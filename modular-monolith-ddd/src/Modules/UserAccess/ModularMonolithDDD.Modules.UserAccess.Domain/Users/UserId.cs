namespace ModularMonolithDDD.Modules.UserAccess.Domain.Users
{
    /// <summary>
    /// User id value object.
    /// </summary>
    public class UserId : TypedIdValueBase
    {
        public UserId(Guid value)
            : base(value)
        {
        }
    }
}