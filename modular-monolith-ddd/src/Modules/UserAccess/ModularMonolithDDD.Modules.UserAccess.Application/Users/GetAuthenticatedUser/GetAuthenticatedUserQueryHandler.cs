namespace ModularMonolithDDD.Modules.UserAccess.Application.Users.GetAuthenticatedUser
{
    internal class GetAuthenticatedUserQueryHandler : IQueryHandler<GetAuthenticatedUserQuery, AuthenticatedUserDto>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        private readonly IExecutionContextAccessor _executionContextAccessor;

        public GetAuthenticatedUserQueryHandler(
            ISqlConnectionFactory sqlConnectionFactory,
            IExecutionContextAccessor executionContextAccessor)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _executionContextAccessor = executionContextAccessor;
        }

        public async Task<AuthenticatedUserDto> Handle(GetAuthenticatedUserQuery request, CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();

            const string sql = $"""
                                SELECT 
                                    [User].[Id] as [{nameof(AuthenticatedUserDto.Id)}], 
                                    [User].[IsActive] as [{nameof(AuthenticatedUserDto.IsActive)}], 
                                    [User].[Login] as [{nameof(AuthenticatedUserDto.Login)}], 
                                    [User].[Email] as [{nameof(AuthenticatedUserDto.Email)}], 
                                    [User].[Name] as [{nameof(AuthenticatedUserDto.Name)}],
                                    [UserRole].[RoleCode] as [{nameof(UserRoleDto.RoleCode)}]
                                FROM [users].[v_Users] AS [User] 
                                LEFT JOIN [users].[UserRoles] AS [UserRole] ON [UserRole].[UserId] = [User].[Id]
                                WHERE [User].[Id] = @UserId;
                                """;

            return await connection.QuerySingleAsync<AuthenticatedUserDto>(sql, new
            {
                _executionContextAccessor.UserId
            });
        }
    }
}
