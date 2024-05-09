using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.Registration;

public class UserRegistrationService(IUserService userService) : IUserRegistrationService
{
    public async Task<User?> GetIfRegisteredAsync(long telegramUserId)
    {
        return await userService.TryGetByTelegramIdAsync(telegramUserId);
    }

    public async Task<User> RegisterUser(long telegramChatId, long telegramUserId, string telegramUserName)
    {
        return await userService.AddAsync(telegramChatId, telegramUserId, telegramUserName);
    }
}