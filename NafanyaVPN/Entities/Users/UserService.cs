using Microsoft.EntityFrameworkCore;
using NafanyaVPN.Database;
using NafanyaVPN.Entities.Subscription;

namespace NafanyaVPN.Entities.Users;

public class UserService(
    IUserRepository userRepository,
    ISubscriptionService subscriptionService
) : IUserService
{
    public async Task<User> AddAsync(long telegramUserId, string telegramUserName)
    {
        var userModel = await CreateNewUserModel(telegramUserId, telegramUserName);
        return await userRepository.CreateAsync(userModel);
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await userRepository.GetAll().ToListAsync();
    }

    public async Task<User> GetAsync(long telegramUserId)
    {
        var user = await TryGetAsync(telegramUserId);
        if (user is null)
            throw new NullReferenceException($"User with telegramUserId: \"{telegramUserId}\" does not exist");

        return user;
    }

    public async Task<User?> TryGetAsync(long telegramUserId)
    {
        var user = await userRepository.GetAll().FirstOrDefaultAsync(u => u.TelegramUserId == telegramUserId);
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        await userRepository.UpdateAsync(user);
    }

    public async Task UpdateAllAsync(IEnumerable<User> users)
    {
        await userRepository.UpdateAllAsync(users);
    }

    private async Task<User> CreateNewUserModel(long telegramUserId, string telegramUserName)
    {
        var defaultSubscription = await subscriptionService.GetAsync(DatabaseConstants.Default);

        var user = new User
        {
            TelegramUserId = telegramUserId,
            TelegramUserName = telegramUserName,
            MoneyInRoubles = 0,
            Subscription = defaultSubscription,
            TelegramState = ""
        };

        return user;
    }
}