namespace ModularMonolithDDD.Modules.UserAccess.Domain.Users
{
    public interface IUserRepository
    {        
        Task AddAsync(User user, CancellationToken cancellationToken);

        Task<User?> GetByExternalIdAsync(string externalId, CancellationToken ct);        
    }
}