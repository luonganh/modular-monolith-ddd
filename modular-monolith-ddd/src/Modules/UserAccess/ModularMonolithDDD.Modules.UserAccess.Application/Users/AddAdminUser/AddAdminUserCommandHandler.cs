namespace ModularMonolithDDD.Modules.UserAccess.Application.Users.AddAdminUser
{
    /// <summary>
    /// Handler for the AddAdminUserCommand.
    /// Processes the command to create a new administrator user in the system.
    /// </summary>
    public class AddAdminUserCommandHandler : ICommandHandler<AddAdminUserCommand>
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the AddAdminUserCommandHandler class.
        /// </summary>
        /// <param name="userRepository">The repository for user data operations</param>
        public AddAdminUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Handles the AddAdminUserCommand by creating a new admin user.
        /// </summary>
        /// <param name="command">The command containing admin user details</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public async Task Handle(AddAdminUserCommand command, CancellationToken cancellationToken)
        {
            // Hash the password for security
            var password = PasswordManager.HashPassword(command.Password);

            // Create a new admin user with the provided details
            var user = User.CreateAdmin(
                command.Login,
                password,
                command.Email,
                command.FirstName,
                command.LastName,
                command.Name);

            // Save the user to the repository
            await _userRepository.AddAsync(user);
        }
    }
}
