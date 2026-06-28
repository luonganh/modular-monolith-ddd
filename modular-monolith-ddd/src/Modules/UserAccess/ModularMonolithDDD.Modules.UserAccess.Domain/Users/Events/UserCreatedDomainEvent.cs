namespace ModularMonolithDDD.Modules.UserAccess.Domain.Users.Events
{
    /// <summary>
    /// Domain event raised when a new user is created.
    /// </summary>
    public class UserCreatedDomainEvent : DomainEventBase
    {
        public UserCreatedDomainEvent(UserId id)
        {
            Id = id;
        }

        public new UserId Id { get; }
    }
}