﻿namespace ModularMonolithDDD.Modules.UserAccess.Domain.Users
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
    }
}