namespace ModularMonolithDDD.Modules.UserAccess.Application.Users.ProvisionUserFromClaims
{
    internal class ProvisionUserFromClaimsCommandHandler : ICommandHandler<ProvisionUserFromClaimsCommand>
    {
        private readonly IUserRepository _users;
        public ProvisionUserFromClaimsCommandHandler(IUserRepository users)
        {
            _users = users;
        }

        public async Task Handle(ProvisionUserFromClaimsCommand request, CancellationToken cancellationToken)
        {
            var user = await _users.GetByExternalIdAsync(request.ExternalId, cancellationToken);
            if (user == null)
            {
                user = User.CreateExternalUser(request.ExternalId, request.Roles, request.Username, request.Email, request.FirstName, request.LastName, request.DisplayName);
                var userRoles = request.Roles.FirstOrDefault() ?? "Member";

                await _users.AddAsync(user, cancellationToken);
            }
            else
            {
                user.SyncProfile(request.Email, request.DisplayName);
            }            
        }
    }
}
