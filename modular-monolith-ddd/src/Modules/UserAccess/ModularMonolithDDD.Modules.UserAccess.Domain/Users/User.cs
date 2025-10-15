using MediatR;

namespace ModularMonolithDDD.Modules.UserAccess.Domain.Users
{
    /// <summary>
    /// Aggregate root and main entity of the UserAccess module.
    /// </summary>
    public class User : Entity, IAggregateRoot
    {
        public UserId Id { get; private set; } = default!;

        private string _login;

        private string _password;

        private string _email;


        private bool _isActive;


        private string _firstName;

        private string _lastName;

        private string _name;
        private string _externalId;

        private List<UserRole> _roles;

        private User()
        {
            // Only for EF.            
            _login = default!;
            _password = default!;
            _email = default!;
            _firstName = default!;
            _lastName = default!;
            _name = default!;
            _roles = default!;
            _externalId = default!;
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
            _externalId = Id.Value.ToString();
            _isActive = true;

            _roles = [role];

            this.AddDomainEvent(new UserCreatedDomainEvent(Id));
        }

        // Public methods to access private fields (following DDD pattern)
        public string GetEmail() => _email;
        public string GetPassword() => _password;
        public bool GetIsActive() => _isActive;
        public string GetFirstName() => _firstName;
        public string GetLastName() => _lastName;
        public string GetName() => _name;
        public IReadOnlyList<UserRole> GetRoles() => _roles.AsReadOnly();
        public string GetExternalId() => _externalId;

        // Create new user from login of Keycloak Identity provider's user
        public static User CreateExternalUser(
            //Guid id,
            string externalId,
            List<string> roles,
            string username,
            string email,
            string firstName,
            string lastName,
            string displayName
        )
        {
            var userRole = UserRole.From(roles.FirstOrDefault());
            //UserRole userRole;
            //foreach (var role in roles)
            //{
            //    switch (role)
            //    {
            //        case "Administrator":
            //            {
            //                userRole = new UserRole("");
            //            break;
            //            }

            //        case "Member":
            //            {
                            
            //            break;
            //            }
            //        default:
            //            break;
            //    }
            //}
            //var role = roles.FirstOrDefault();
            

            if (Guid.TryParse(externalId, out Guid id))
            {
                return new User(
                id,
                userRole,
                externalId,
                username,
                email,
                firstName,
                lastName,
                displayName);
            }
            else
            {
                return new User(
                Guid.NewGuid(),
                userRole,
                externalId,
                username,
                email,
                firstName,
                lastName,
                displayName);
            }
        }

        private User(
            Guid id,
            UserRole role,
            string externalId,
            string username, 
            string email,
            string firstName,
            string lastName,
            string displayName            
            )
        {
            Id = new UserId(id);
            _externalId = externalId;
            _login = username;
            _email = email;
            _firstName = firstName;
            _lastName = lastName;
            _name = displayName;
            _password = string.Empty;
            _isActive = true;
            _roles = [role];
            this.AddDomainEvent(new UserCreatedDomainEvent(Id));
        }
        
        public void SyncProfile(string? email, string? displayName, string? firstName = null, string? lastName = null)
        {
            var changed = false;

            if (!string.IsNullOrWhiteSpace(email) &&
                !string.Equals(email, _email, StringComparison.OrdinalIgnoreCase))
            {
                _email = email;
                changed = true;
            }

            if (!string.IsNullOrWhiteSpace(displayName) &&
                !string.Equals(displayName, _name, StringComparison.Ordinal))
            {
                _name = displayName;
                changed = true;
            }

            if (!string.IsNullOrWhiteSpace(firstName) &&
                !string.Equals(firstName, _firstName, StringComparison.Ordinal))
            {
                _firstName = firstName;
                changed = true;
            }

            if (!string.IsNullOrWhiteSpace(lastName) &&
                !string.Equals(lastName, _lastName, StringComparison.Ordinal))
            {
                _lastName = lastName;
                changed = true;
            }

            if (changed)
            {
                // Optional: raise domain events (UserProfileUpdatedDomainEvent) to stream/outbox.
                this.AddDomainEvent(new UserProfileUpdatedDomainEvent(Id));
            }
        }
    }
}