using ModularMonolithDDD.Modules.UserAccess.Application.Authentication;

namespace ModularMonolithDDD.Modules.UserAccess.Application.Users.GetUser
{
    internal class GetUserQueryHandler : IQueryHandler<GetUserQuery, AuthenticatedUserDto>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetUserQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<AuthenticatedUserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();

            const string userSql = $"""
                                   SELECT 
                                       [User].[Id]       as [{nameof(AuthenticatedUserDto.Id)}], 
                                       [User].[IsActive] as [{nameof(AuthenticatedUserDto.IsActive)}], 
                                       [User].[Login]    as [{nameof(AuthenticatedUserDto.Login)}], 
                                       [User].[Email]    as [{nameof(AuthenticatedUserDto.Email)}], 
                                       [User].[Name]     as [{nameof(AuthenticatedUserDto.Name)}]
                                   FROM [users].[v_Users] AS [User]
                                   WHERE [User].[Id] = @UserId;
                                   """;

            const string rolesSql = $"""
                                    SELECT [UserRole].[RoleCode] as [{nameof(UserRoleDto.RoleCode)}]
                                    FROM [users].[UserRoles] AS [UserRole]
                                    WHERE [UserRole].[UserId] = @UserId;
                                    """;

            var user = await connection.QuerySingleAsync<AuthenticatedUserDto>(userSql, new { request.UserId });
            var roles = await connection.QueryAsync<UserRoleDto>(rolesSql, new { request.UserId });
            user.Roles = roles.ToList();
            return user;
        }
    }
}