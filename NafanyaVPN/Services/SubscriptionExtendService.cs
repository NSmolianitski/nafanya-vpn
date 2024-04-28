using NafanyaVPN.Constants;
using NafanyaVPN.Models;
using NafanyaVPN.Services.Abstractions;

namespace NafanyaVPN.Services;

public class SubscriptionExtendService(
    IUserService userService,
    ISubscriptionService subscriptionService,
    IOutlineService outlineService,
    ISubscriptionDateTimeService dateTimeService,
    ILogger<SubscriptionExtendService> logger)
    : ISubscriptionExtendService
{
    public async Task ExtendForAllUsers()
    {
        DateTime newSubscriptionEndDate = dateTimeService.GetNewSubscriptionEndDate();
        
        Subscription defaultSubscription = await subscriptionService.GetAsync(DatabaseConstants.Default);

        if (defaultSubscription.NextUpdateTime > dateTimeService.Now())
        {
            logger.LogInformation($"Стандартная подписка пока не может обновиться. " +
                                   $"Это не ошибка (может появляться при запуске приложения). " +
                                   $"Следующее обновление: {defaultSubscription.NextUpdateTime}");
            return;
        }

        defaultSubscription.NextUpdateTime = newSubscriptionEndDate;
        await subscriptionService.UpdateAsync(defaultSubscription);
        
        var users = await userService.GetAllAsync();
        foreach (var user in users)
        {
            var subscriptionPrice = user.Subscription.DailyCostInRoubles;
            if (subscriptionPrice > user.MoneyInRoubles)
            {
                if (user.OutlineKey is null)
                    continue;
                
                var keyId = user.OutlineKey!.Id;
                outlineService.DisableKey(keyId);
                logger.LogInformation($"Ключ деактивирован. " +
                                       $"На счёте {user.TelegramUserName}({user.TelegramUserId}) " +
                                       $"недостаточно средств. Стоимость подписки: {subscriptionPrice}. " +
                                       $"Текущий баланс: {user.MoneyInRoubles}");
            }
            else
            {
                user.MoneyInRoubles -= subscriptionPrice;
                user.SubscriptionEndDate = newSubscriptionEndDate;
                logger.LogInformation($"Со счёта {user.TelegramUserName}({user.TelegramUserId}) " +
                                       $"списано {subscriptionPrice} рублей. Осталось: {user.MoneyInRoubles}");
            }
        }

        await userService.UpdateAllAsync(users);
    }

    public async Task TryExtendForUser(User user)
    {
        var subscriptionPrice = user.Subscription.DailyCostInRoubles;
        if (subscriptionPrice > user.MoneyInRoubles)
        {
            var keyId = user.OutlineKey!.Id;
            outlineService.DisableKey(keyId);
        }
        else
        {
            user.MoneyInRoubles -= subscriptionPrice;
            user.SubscriptionEndDate = user.Subscription.NextUpdateTime;
        }

        await userService.UpdateAsync(user);
    }
}