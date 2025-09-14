namespace ModularMonolithDDD.Modules.UserAccess.Domain.Users
{
    /// <summary>
    /// Aggregate root and main entity of the UserAccess module.
    /// </summary>
    public class User : Entity, IAggregateRoot
    {
        public UserId Id { get; private set; }

        private string _login;

        private string _password;

        private string _email;


        private bool _isActive;


        private string _firstName;

        private string _lastName;

        private string _name;

        private List<UserRole> _roles;

        private User()
        {
            // Only for EF.
        }

        /// <summary>
        /// Factory method to create an Administrator user.
        /// Benefits:
        /// - Ensures proper role assignment (Administrator)
        /// - Encapsulates admin-specific creation logic
        /// - Provides clear intent and type safety
        /// - Centralizes business rules for admin user creation
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static User CreateAdmin(
            string login,
            string password,
            string email,
            string firstName,
            string lastName,
            string name)
        {
            return new User(
                Guid.NewGuid(),
                login,
                password,
                email,
                firstName,
                lastName,
                name,
                UserRole.Administrator);
        }

        /// <summary>
        /// Factory method to create a regular Member user.
        /// Benefits:
        /// - Ensures proper role assignment (Member)
        /// - Encapsulates member-specific creation logic
        /// - Provides clear intent and type safety
        /// - Centralizes business rules for member user creation
        /// - Maintains domain integrity by preventing invalid user states
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public static User CreateUser(
            Guid userId,
            string login,
            string password,
            string email,
            string firstName,
            string lastName)
        {
            return new User(
                userId,
                login,
                password,
                email,
                firstName,
                lastName,
                $"{firstName} {lastName}",
                UserRole.Member);
        }

        /// <summary>
        /// Private constructor to enforce the use of factory methods.
        /// This ensures proper object initialization and maintains domain integrity. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="name"></param>
        /// <param name="role"></param>
        private User(
            Guid id,
            string login,
            string password,
            string email,
            string firstName,
            string lastName,
            string name,
            UserRole role)
        {
			Id = new UserId(id);
            _login = login;
            _password = password;
            _email = email;
            _firstName = firstName;
            _lastName = lastName;
            _name = name;

            _isActive = true;

            _roles = [role];

            this.AddDomainEvent(new UserCreatedDomainEvent(Id));
        }
    }
}