namespace ModularMonolithDDD.Modules.UserAccess.Infrastructure.Domain.Users
{
	/// <summary>
	/// Repository implementation for User entities using Entity Framework Core.
	/// Provides data access operations for the User domain entity.
	/// </summary>
	public class UserRepository : IUserRepository
	{
		private readonly UserAccessContext _userAccessContext;

		/// <summary>
		/// Initializes a new instance of the UserRepository.
		/// </summary>
		/// <param name="userAccessContext">The Entity Framework context for database operations.</param>
		public UserRepository(UserAccessContext userAccessContext)
		{
			_userAccessContext = userAccessContext;
		}

		/// <summary>
		/// Adds a new user to the database asynchronously.
		/// The user will be persisted when SaveChanges is called on the context.
		/// </summary>
		/// <param name="user">The user entity to add to the database.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		public async Task AddAsync(User user)
		{
			await _userAccessContext.Users.AddAsync(user);
		}
	}
}
