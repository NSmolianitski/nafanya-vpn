using NafanyaVPN.Entities.SubscriptionPlans;
using NafanyaVPN.Entities.Subscriptions;

namespace NafanyaVPN.Entities.Users;

public class UserService(
    IUserRepository userRepository,
    ISubscriptionPlanService subscriptionPlanService
) : IUserService
{
    public async Task<User> AddAsync(long telegramChatId, long telegramUserId, string telegramUserName)
    {
        var userModel = await CreateNewUserModel(telegramChatId, telegramUserId, telegramUserName);
        return await userRepository.CreateAsync(userModel);
    }

    public async Task<List<User>> GetAllWithForeignKeysAsync()
    {
        return await userRepository.GetAllWithForeignKeysAsync();
    }

    public async Task<User> GetByIdAsync(int id)
    {
        var user = await TryGetByIdAsync(id);
        if (user is null)
            throw new NullReferenceException($"User with id: \"{id}\" does not exist");

        return user;
    }

    public async Task<User?> TryGetByIdAsync(int id)
    {
        var user = await userRepository.TryGetByIdAsync(id);
        return user;
    }
    
    public async Task<User> GetByTelegramIdAsync(long telegramUserId)
    {
        var user = await TryGetByTelegramIdAsync(telegramUserId);
        if (user is null)
            throw new NullReferenceException($"User with telegramUserId: \"{telegramUserId}\" does not exist");

        return user;
    }

    public async Task<User?> TryGetByTelegramIdAsync(long telegramUserId)
    {
        var user = await userRepository.TryGetByTelegramIdAsync(telegramUserId);
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

    private async Task<User> CreateNewUserModel(long telegramChatId, long telegramUserId, string telegramUserName)
    {
        var defaultSubscriptionPlan = await subscriptionPlanService
            .GetByNameAsync(SubscriptionPlanTypes.Default.ToString());

        var subscription = new SubscriptionBuilder()
            .WithNowCreatedAtUpdatedAt()
            .WithSubscriptionPlan(defaultSubscriptionPlan)
            .WithHasExpired(true)
            .WithRenewalDisabled(false)
            .WithEndNotificationsDisabled(false)
            .WithRenewalNotificationsDisabled(false)
            .WithEndNotificationPerformed(false)
            .Build();
        
        var user = new UserBuilder()
            .WithNowCreatedAtUpdatedAt()
            .WithTelegramChatId(telegramChatId)
            .WithTelegramUserId(telegramUserId)
            .WithTelegramUserName(telegramUserName)
            .WithMoneyInRoubles(0.0m)
            .WithSubscription(subscription)
            .WithTelegramState(string.Empty)
            .Build();

        return user;
    }
}