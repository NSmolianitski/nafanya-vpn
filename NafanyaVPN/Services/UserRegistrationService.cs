using NafanyaVPN.Services.Abstractions;

namespace NafanyaVPN.Services;

public class UserRegistrationService : IUserRegistrationService
{
    private readonly IUserService _userService;

    public UserRegistrationService(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<bool> IsRegistered(long telegramUserId)
    {
        var user = await _userService.TryGetAsync(telegramUserId);
        return user is not null;
    }

    public async Task RegisterUser(long telegramUserId, string telegramUserName)
    {
        await _userService.AddAsync(telegramUserId, telegramUserName);
    }
}