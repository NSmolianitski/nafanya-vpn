using NafanyaVPN.Entities.Outline;
using NafanyaVPN.Entities.Payments;
using NafanyaVPN.Entities.Subscriptions;
using NafanyaVPN.Entities.Users;
using NafanyaVPN.Entities.Withdraws;
using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.Constants;

namespace NafanyaVPN.Entities.SubscriptionPlans;

public class SubscriptionRenewService(
    IOutlineService outlineService,
    ISubscriptionDateTimeService dateTimeService,
    ISubscriptionService subscriptionService,
    IWithdrawService withdrawService,
    IReplyService replyService,
    ILogger<SubscriptionRenewService> logger)
    : ISubscriptionRenewService
{
    public async Task RenewAllNonExpiredAsync()
    {
        var nonExpiredSubscriptions = await subscriptionService.GetAllNonExpiredAsync();
        var extendedSubscriptions = new List<Subscription>();
        foreach (var subscription in nonExpiredSubscriptions)
        {
            var expiredForReal = dateTimeService.HasSubscriptionExpired(subscription);
            if (!expiredForReal)
                continue;
            
            await StopSubscriptionWithoutDbSaveAsync(subscription);
            
            if (!subscription.RenewalDisabled && IsEnoughMoneyForRenewal(subscription))
            {
                await RenewSubscriptionWithoutDbSaveAsync(subscription);
                extendedSubscriptions.Add(subscription);
            }
            else
            {
                var stopMessage = subscription.RenewalDisabled
                    ? "Подписка истекла. Продлите нажатием на кнопку \"Обновить подписку\""
                    : "Подписка истекла. Пополните счёт.";
                await replyService.SendTextWithMainKeyboardAsync(subscription.User.TelegramUserId, 
                    subscription, stopMessage);
            }
        }

        await subscriptionService.UpdateAllAsync(extendedSubscriptions);
        
        foreach (var subscription in extendedSubscriptions
                     .Where(subscription => !subscription.RenewalNotificationsDisabled))
        {
            await SendRenewalNotificationAsync(subscription);
        }
    }

    private async Task SendRenewalNotificationAsync(Subscription subscription)
    {
        if (subscription.RenewalNotificationsDisabled)
            return;
        
        await replyService.SendTextWithMainKeyboardAsync(subscription.User.TelegramUserId, subscription,
            $"Подписка продлена до {subscription.EndDateTime.ToString(TelegramConstants.DateTimeFormat)}. " +
            $"Списано {subscription.SubscriptionPlan.CostInRoubles}{PaymentConstants.CurrencySymbol}.\n" +
            $"Ваш баланс: {subscription.User.MoneyInRoubles}{PaymentConstants.CurrencySymbol}");
    }

    private bool IsEnoughMoneyForRenewal(Subscription subscription)
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
        
        withdrawService.CreateWithoutSaving(subscription);
        
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

    public async Task RenewIfEnoughMoneyAsync(User user)
    {
        if (user.OutlineKey is null)
            await outlineService.CreateOutlineKeyForUser(user);
        
        var subscription = user.Subscription;
        
        if (IsEnoughMoneyForRenewal(subscription))
        {
            await RenewSubscriptionWithoutDbSaveAsync(subscription);
            await subscriptionService.UpdateAsync(subscription);
            await SendRenewalNotificationAsync(subscription);
        }
    }

    private void LogSubscriptionCancellation(User user, decimal subscriptionPrice)
    {
        logger.LogInformation("Ключ отключён. " +
                              "На счёте {TelegramUserName}({TelegramUserId}) недостаточно средств. " +
                              "Стоимость подписки: {SubscriptionPrice}. Текущий баланс: {MoneyInRoubles}{CurrencySymbol}", 
            user.TelegramUserName,
            user.TelegramUserId,
            subscriptionPrice, 
            user.MoneyInRoubles,
            PaymentConstants.CurrencySymbol);
    }

    private void LogSubscriptionExtension(User user, decimal subscriptionPrice)
    {
        logger.LogInformation("Со счёта {TelegramUserName}({TelegramUserId}) " +
                              "списано {SubscriptionPrice} рублей. Осталось: {MoneyInRoubles}{CurrencySymbol}", 
            user.TelegramUserName, 
            user.TelegramUserId, 
            subscriptionPrice, 
            user.MoneyInRoubles,
            PaymentConstants.CurrencySymbol);
    }
}