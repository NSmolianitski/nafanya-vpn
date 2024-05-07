using NafanyaVPN.Database;
using NafanyaVPN.Entities.Outline;
using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.Subscription;

public class SubscriptionExtendService(
    IUserService userService,
    ISubscriptionService subscriptionService,
    IOutlineService outlineService,
    ISubscriptionDateTimeService dateTimeService,
    ILogger<SubscriptionExtendService> logger)
    : ISubscriptionExtendService
{
    public async Task TryExtendForAllUsers()
    {
        DateTime newSubscriptionEndDate = dateTimeService.GetNewSubscriptionEndDate();
        
        Subscription defaultSubscription = await subscriptionService.GetAsync(DatabaseConstants.Default);

        defaultSubscription.NextUpdateTime = newSubscriptionEndDate;
        await subscriptionService.UpdateAsync(defaultSubscription);
        
        var users = await userService.GetAllWithOutlineKeysAsync();
        var extendedUsers = new List<User>();
        foreach (var user in users)
        {
            var subscriptionExtended = await TryExtendForUserWithoutDbSavingAsync(user);
            if (subscriptionExtended)
                extendedUsers.Add(user);
        }

        await userService.UpdateAllAsync(extendedUsers);
    }

    private async Task<bool> TryExtendForUserWithoutDbSavingAsync(User user)
    {
        if (user.OutlineKey is null)
            return false;
        
        var subscriptionPrice = user.Subscription.CostInRoubles;
        
        if (user.MoneyInRoubles >= subscriptionPrice)
        {
            user.MoneyInRoubles -= subscriptionPrice;
            user.SubscriptionEndDate = dateTimeService.GetNewSubscriptionEndDate();
            if (!user.OutlineKey.Enabled)
                await outlineService.EnableKeyAsync(user.OutlineKey!.Id);
            
            LogSubscriptionExtension(user, subscriptionPrice);
        }
        else if (user.OutlineKey.Enabled)
        {
            var keyId = user.OutlineKey!.Id;
            await outlineService.DisableKeyAsync(keyId);
            
            LogSubscriptionCancellation(user, subscriptionPrice);
        }

        return true;
    }

    public async Task TryExtendForUser(User user)
    {
        var subscriptionExtended = await TryExtendForUserWithoutDbSavingAsync(user);
        if (subscriptionExtended)
            await userService.UpdateAsync(user);
    }

    private void LogSubscriptionCancellation(User user, decimal subscriptionPrice)
    {
        logger.LogInformation("Ключ деактивирован. " +
                              "На счёте {TelegramUserName}({TelegramUserId}) недостаточно средств. " +
                              "Стоимость подписки: {SubscriptionPrice}. Текущий баланс: {MoneyInRoubles}", 
            user.TelegramUserName,
            user.TelegramUserId,
            subscriptionPrice, 
            user.MoneyInRoubles);
    }

    private void LogSubscriptionExtension(User user, decimal subscriptionPrice)
    {
        logger.LogInformation("Со счёта {TelegramUserName}({TelegramUserId}) " +
                              "списано {SubscriptionPrice} рублей. Осталось: {MoneyInRoubles}", 
            user.TelegramUserName, 
            user.TelegramUserId, 
            subscriptionPrice, 
            user.MoneyInRoubles);
    }
}