using NafanyaVPN.Entities.Outline;
using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.SubscriptionPlans;

public class SubscriptionExtendService(
    IUserService userService,
    IOutlineService outlineService,
    ISubscriptionDateTimeService dateTimeService,
    ILogger<SubscriptionExtendService> logger)
    : ISubscriptionExtendService
{
    public async Task TryExtendForAllUsers()
    {
        var users = await userService.GetAllWithForeignKeysAsync();
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
        // TODO: изменить user.OutlineKey.Enabled на Subscription.Enabled,
        // TODO: добавить User'у List<Subscription> и изменить Subscription на SubscriptionPlan (или что-то такое)
        
        if (user.OutlineKey is null) 
            return false;
        
        var subscriptionPrice = user.SubscriptionPlan.CostInRoubles;
        var subscriptionExpired = dateTimeService.IsSubscriptionHasExpired(user.SubscriptionEndDate);
        if (!subscriptionExpired)
            return false;
        
        var userHasEnoughMoney = user.MoneyInRoubles >= subscriptionPrice;
        if (userHasEnoughMoney)
        {
            user.MoneyInRoubles -= subscriptionPrice;
            user.SubscriptionEndDate = dateTimeService.GetNewSubscriptionEndDate();
            if (!user.OutlineKey.Enabled)
                await outlineService.EnableKeyAsync(user.OutlineKey!.Id);
            
            LogSubscriptionExtension(user, subscriptionPrice);
        }
        else
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