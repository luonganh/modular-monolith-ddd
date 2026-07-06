namespace ModularMonolithDDD.Modules.UserAccess.Domain.Users.Events
{
    /// <summary>
    /// Domain event raised when an user is updated.
    /// </summary>
    public class UserProfileUpdatedDomainEvent : DomainEventBase
    {
        public UserProfileUpdatedDomainEvent(UserId id)
        {
            Id = id;
        }

        public new UserId Id { get; }
    }
}