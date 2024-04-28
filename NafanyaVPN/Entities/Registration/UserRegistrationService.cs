using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.Registration;

public class UserRegistrationService(IUserService userService) : IUserRegistrationService
{
    public async Task<bool> IsRegistered(long telegramUserId)
    {
        var user = await userService.TryGetAsync(telegramUserId);
        return user is not null;
    }

    public async Task RegisterUser(long telegramUserId, string telegramUserName)
    {
        await userService.AddAsync(telegramUserId, telegramUserName);
    }
}