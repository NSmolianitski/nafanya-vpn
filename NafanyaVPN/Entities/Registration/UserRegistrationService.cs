using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.Registration;

public class UserRegistrationService(IUserService userService) : IUserRegistrationService
{
    public async Task<User?> GetIfRegisteredAsync(long telegramUserId)
    {
        return await userService.TryGetAsync(telegramUserId);
    }

    public async Task<User> RegisterUser(long telegramUserId, string telegramUserName)
    {
        return await userService.AddAsync(telegramUserId, telegramUserName);
    }
}