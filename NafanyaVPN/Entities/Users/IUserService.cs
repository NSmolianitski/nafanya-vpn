﻿namespace NafanyaVPN.Entities.Users;

public interface IUserService
{
    Task<User> AddAsync(long telegramUserId, string telegramUserName);
    Task<List<User>> GetAllWithOutlineKeysAsync();
    Task<User> GetAsync(long telegramUserId);
    Task<User?> TryGetAsync(long telegramUserId);
    Task UpdateAsync(User user);
    Task UpdateAllAsync(IEnumerable<User> users);
}