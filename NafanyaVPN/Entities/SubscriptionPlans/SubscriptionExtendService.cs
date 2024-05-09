using NafanyaVPN.Entities.Outline;
using NafanyaVPN.Entities.Subscriptions;
using NafanyaVPN.Entities.Users;

namespace NafanyaVPN.Entities.SubscriptionPlans;

public class SubscriptionExtendService(
    IOutlineService outlineService,
    ISubscriptionDateTimeService dateTimeService,
    ISubscriptionService subscriptionService,
    ILogger<SubscriptionExtendService> logger)
    : ISubscriptionExtendService
{
    public async Task RenewAllNonExpiredAsync()
    {
        var nonExpiredSubscriptions = await subscriptionService.GetAllNonExpiredAsync();
        var extendedSubscriptions = new List<Subscription>();
        foreach (var subscription in nonExpiredSubscriptions)
        {
            var subscriptionWasExtended = await TryRenewWithoutDbSavingAsync(subscription);
            if (subscriptionWasExtended)
                extendedSubscriptions.Add(subscription);
        }

        await subscriptionService.UpdateAllAsync(extendedSubscriptions);
    }

    private async Task<bool> TryRenewWithoutDbSavingAsync(Subscription subscription)
    {
        var subscriptionHasExpired = dateTimeService.HasSubscriptionExpired(subscription);
        if (!subscriptionHasExpired)
            return false;
        
        if (CanSubscriptionBeRenewedByOwner(subscription))
            await RenewSubscriptionWithoutDbSaveAsync(subscription);
        else
            await StopSubscriptionWithoutDbSaveAsync(subscription);

        return true;
    }
    
    private bool CanSubscriptionBeRenewedByOwner(Subscription subscription)
    {
        var subscriptionPrice = subscription.SubscriptionPlan.CostInRoubles;
        var ownerMoney = subscription.User.MoneyInRoubles;
        return ownerMoney >= subscriptionPrice;
    }

    private async Task RenewSubscriptionWithoutDbSaveAsync(Subscription subscription)
    {
        var subscriptionPrice = subscription.SubscriptionPlan.CostInRoubles;
        var user = subscription.User;

        user.MoneyInRoubles -= subscriptionPrice;
        subscription.EndDateTime = dateTimeService.GetNewSubscriptionEndDateTime();
        subscription.HasExpired = false;
        
        if (!user.OutlineKey.Enabled)
            await outlineService.EnableKeyAsync(user.OutlineKey!.Id);
            
        LogSubscriptionExtension(user, subscriptionPrice);
    }

    private async Task StopSubscriptionWithoutDbSaveAsync(Subscription subscription)
    {
        var subscriptionPrice = subscription.SubscriptionPlan.CostInRoubles;
        var user = subscription.User;

        subscription.HasExpired = true;
        
        if (user.OutlineKey.Enabled)
            await outlineService.DisableKeyAsync(user.OutlineKey!.Id);
            
        LogSubscriptionCancellation(user, subscriptionPrice);
    }

    public async Task TryRenewForUserAsync(User user)
    {
        if (user.OutlineKey is null)
            await outlineService.CreateOutlineKeyForUser(user);
        
        var subscription = user.Subscription;
        
        var subscriptionExtended = await TryRenewWithoutDbSavingAsync(subscription);
        if (subscriptionExtended)
            await subscriptionService.UpdateAsync(subscription);
    }

    private void LogSubscriptionCancellation(User user, decimal subscriptionPrice)
    {
        logger.LogInformation("Ключ отключён. " +
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