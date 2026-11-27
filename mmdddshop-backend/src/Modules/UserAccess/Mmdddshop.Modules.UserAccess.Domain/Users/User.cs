using MediatR;

namespace Mmdddshop.Modules.UserAccess.Domain.Users
{
    /// <summary>
    /// Aggregate root and main entity of the UserAccess module.
    /// </summary>
    public class User : Entity, IAggregateRoot
    {
        public UserId Id { get; private set; } = default!;

        private string _login;

        private string? _email;

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
            _email = default!;
            _firstName = default!;
            _lastName = default!;
            _name = default!;
            _roles = default!;
            _externalId = default!;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// Private constructor to enforce the use of factory methods.
        /// This ensures proper object initialization and maintains domain integrity.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="login"></param>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="name"></param>
        /// <param name="role"></param>
        private User(
            Guid id,
            string login,
            string email,
            string firstName,
            string lastName,
            string name,
            UserRole role)
        {
            Id = new UserId(id);
            _login = login;
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
        public string? GetEmail() => _email;

        public bool GetIsActive() => _isActive;

        public string GetFirstName() => _firstName;

        public string GetLastName() => _lastName;

        public string GetName() => _name;

        public IReadOnlyList<UserRole> GetRoles() => _roles.AsReadOnly();

        public string GetExternalId() => _externalId;

        // Create new user from login of Keycloak Identity provider's user
        public static User CreateExternalUser(
            string externalId,
            List<string>? roles,
            string username,
            string? email,
            string? firstName,
            string? lastName,
            string? displayName)
        {
            var userRoles = (roles ?? [])
            .Where(UserRole.IsKnown)
            .Select(UserRole.From)
            .Distinct()
            .ToList();

            var userId = Guid.TryParse(externalId, out var id) ? id : Guid.NewGuid();
            return new User(
                userId,
                userRoles,
                externalId,
                username,
                NullIfBlank(email),
                firstName?.Trim() ?? string.Empty,
                lastName?.Trim() ?? string.Empty,
                NullIfBlank(displayName) ?? username);
        }

        private static string? NullIfBlank(string? value) =>
    string.IsNullOrWhiteSpace(value) ? null : value.Trim();

        private User(
           Guid id,
           List<UserRole> roles,
           string externalId,
           string username,
           string? email,
           string firstName,
           string lastName,
           string displayName)
        {
            Id = new UserId(id);
            _externalId = externalId;
            _login = username;
            _email = email;
            _firstName = firstName;
            _lastName = lastName;
            _name = displayName;

            _isActive = true;
            _roles = roles;
            this.AddDomainEvent(new UserCreatedDomainEvent(Id));
        }

        public void SyncProfile(List<string>? roles, string? email, string? displayName, string? firstName = null, string? lastName = null)
        {
            var changed = false;

            email = email?.Trim();
            displayName = displayName?.Trim();
            firstName = firstName?.Trim();
            lastName = lastName?.Trim();

            if (roles != null)
            {
                // Chỉ giữ role nghiệp vụ đã biết; role kỹ thuật của Keycloak bị bỏ qua.
                var incomingRoles = roles
                    .Where(UserRole.IsKnown)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                // Gỡ role không còn trong token (kể cả khi incomingRoles rỗng → gỡ hết).
                if (_roles.RemoveAll(role => !incomingRoles.Contains(role.Value)) > 0)
                {
                    changed = true;
                }

                var existingRoles = _roles
                    .Select(role => role.Value)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                foreach (var roleCode in incomingRoles.Except(existingRoles, StringComparer.OrdinalIgnoreCase))
                {
                    _roles.Add(UserRole.From(roleCode));
                    changed = true;
                }
            }

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
