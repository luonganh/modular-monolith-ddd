namespace ModularMonolithDDD.Modules.UserAccess.Application.Users.CreateUser;

/// <summary>
/// Handler for the CreateUserCommand.
/// Processes the command to create a new regular user in the system.
/// </summary>
internal class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
{
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of the CreateUserCommandHandler class.
    /// </summary>
    /// <param name="userRepository">The repository for user data operations</param>
    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Handles the CreateUserCommand by creating a new regular user.
    /// </summary>
    /// <param name="command">The command containing user details</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        // Create a new regular user with the provided details
        var user = User.CreateUser(
            command.UserId,
            command.Login,
            command.Password,
            command.Email,
            command.FirstName,
            command.LastName);

        // Save the user to the repository
        await _userRepository.AddAsync(user, CancellationToken.None);
    }
}