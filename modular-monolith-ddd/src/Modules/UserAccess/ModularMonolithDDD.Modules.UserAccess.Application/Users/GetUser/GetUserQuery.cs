namespace ModularMonolithDDD.Modules.UserAccess.Application.Users.GetUser
{
    public class GetUserQuery : QueryBase<AuthenticatedUserDto>
    {
        public GetUserQuery(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }
}