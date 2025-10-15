namespace ModularMonolithDDD.Modules.UserAccess.Application.Authentication.Authenticate;

/// <summary>
/// Handler for the AuthenticateCommand following grzybek pattern with Dapper.
/// </summary>
internal class AuthenticateCommandHandler : ICommandHandler<AuthenticateCommand, AuthenticationResult>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    /// <summary>
    /// Initializes a new instance of the AuthenticateCommandHandler class.
    /// </summary>
    /// <param name="sqlConnectionFactory">The SQL connection factory for database operations</param>
    internal AuthenticateCommandHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    /// <summary>
    /// Handles the AuthenticateCommand by authenticating the user following grzybek pattern.
    /// </summary>
    /// <param name="request">The command containing login credentials</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task representing the asynchronous operation with AuthenticationResult</returns>
    public async Task<AuthenticationResult> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();

        const string sql = $"""
        SELECT 
           [User].[Id] as [{nameof(UserDto.Id)}],
           [User].[Login] as [{nameof(UserDto.Login)}],
           [User].[Name] as [{nameof(UserDto.Name)}],
           [User].[Email] as [{nameof(UserDto.Email)}],
           [User].[IsActive] as [{nameof(UserDto.IsActive)}],
           [User].[Password] as [{nameof(UserDto.Password)}]
        FROM [users].[v_Users] AS [User] 
        WHERE [User].[Login] = @Login;
       
        SELECT [RoleCode] as [{nameof(UserRoleDto.RoleCode)}]
        FROM [users].[UserRoles] 
        WHERE [UserId] = (SELECT [Id] FROM [users].[v_Users] WHERE [Login] = @Login);
        """;

        using var multi = await connection.QueryMultipleAsync(sql, new { request.Login });
        var user = await multi.ReadSingleOrDefaultAsync<UserDto>();
        var roles = await multi.ReadAsync<UserRoleDto>();

        if (user == null)
        {
            return new AuthenticationResult("Incorrect login or password");
        }

        if (!user.IsActive)
        {
            return new AuthenticationResult("User is not active");
        }

        if (!PasswordManager.VerifyHashedPassword(user.Password, request.Password))
        {
            return new AuthenticationResult("Incorrect login or password");
        }

        user.Roles = roles.ToList();
        
        user.Claims =
        [
            new Claim(CustomClaimTypes.Name, user.Name),
            new Claim(CustomClaimTypes.Email, user.Email)
        ];

        return new AuthenticationResult(user);
    }

}
