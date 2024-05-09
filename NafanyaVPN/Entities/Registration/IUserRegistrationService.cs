using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.Registration;

public interface IUserRegistrationService
{
    Task<User?> GetIfRegisteredAsync(long telegramUserId);
    Task<User> RegisterUser(long telegramChatId, long telegramUserId, string telegramUserName);
}