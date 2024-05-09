namespace NafanyaVPN.Entities.Users;

public interface IUserService
{
    Task<User> AddAsync(long telegramChatId, long telegramUserId, string telegramUserName);
    Task<List<User>> GetAllWithForeignKeysAsync();
    Task<User> GetByIdAsync(int id);
    Task<User?> TryGetByIdAsync(int id);
    Task<User> GetByTelegramIdAsync(long telegramUserId);
    Task<User?> TryGetByTelegramIdAsync(long telegramUserId);
    Task UpdateAsync(User user);
    Task UpdateAllAsync(IEnumerable<User> users);
}