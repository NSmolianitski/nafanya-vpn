﻿namespace NafanyaVPN.Entities.Users;

public interface IUserRepository
{
    Task<User> CreateAsync(User model);
    Task<List<User>> GetAllWithForeignKeysAsync();
    Task<User> GetByIdAsync(int id);
    Task<User?> TryGetByIdAsync(int id);
    Task<User> GetByTelegramIdAsync(long telegramId);
    Task<User?> TryGetByTelegramIdAsync(long telegramId);
    Task<bool> DeleteAsync(User model);
    Task<User> UpdateAsync(User model);
    Task UpdateAllAsync(IEnumerable<User> models);
}