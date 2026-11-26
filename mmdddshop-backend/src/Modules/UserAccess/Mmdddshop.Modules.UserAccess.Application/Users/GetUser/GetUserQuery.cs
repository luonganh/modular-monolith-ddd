using Mmdddshop.Modules.UserAccess.Application.Users.GetAuthenticatedUser;

namespace Mmdddshop.Modules.UserAccess.Application.Users.GetUser
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