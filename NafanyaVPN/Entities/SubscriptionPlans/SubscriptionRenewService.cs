using NafanyaVPN.Entities.Outline;
using NafanyaVPN.Entities.Payments;
using NafanyaVPN.Entities.Subscriptions;
using NafanyaVPN.Entities.Users;
using NafanyaVPN.Entities.Withdraws;
using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.Constants;
using NafanyaVPN.Utils;

namespace NafanyaVPN.Entities.SubscriptionPlans;

public class SubscriptionRenewService : ISubscriptionRenewService
{
    private readonly IOutlineService _outlineService; 
    private readonly ISubscriptionDateTimeService _dateTimeService; 
    private readonly ISubscriptionService _subscriptionService; 
    private readonly IWithdrawService _withdrawService; 
    private readonly IReplyService _replyService; 
    private readonly ILogger<SubscriptionRenewService> _logger;
    
    private readonly TimeSpan _timeUntilEndNotification;

    public SubscriptionRenewService(
        IConfiguration configuration, 
        IOutlineService outlineService, 
        ISubscriptionDateTimeService dateTimeService, 
        ISubscriptionService subscriptionService, 
        IWithdrawService withdrawService, 
        IReplyService replyService, 
        ILogger<SubscriptionRenewService> logger)
    {
        _outlineService = outlineService;
        _dateTimeService = dateTimeService;
        _subscriptionService = subscriptionService;
        _withdrawService = withdrawService;
        _replyService = replyService;
        _logger = logger;
        
        var subscriptionConfig = configuration.GetRequiredSection(SubscriptionConstants.Subscription);
        _timeUntilEndNotification = TimeSpan.Parse(subscriptionConfig[SubscriptionConstants.DaysBeforeEndNotification]!);
    }

    public async Task RenewAllNonExpiredAsync()
    {
        var nonExpiredSubscriptions = await _subscriptionService.GetAllNonExpiredAsync();
        var extendedSubscriptions = new List<Subscription>();
        foreach (var subscription in nonExpiredSubscriptions)
        {
            await NotifyAboutEndIfNecessaryWithoutDbSaving(subscription);
            
            var expiredForReal = _dateTimeService.HasSubscriptionExpired(subscription);
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
                await _replyService.SendTextWithMainKeyboardAsync(subscription.User.TelegramUserId, 
                    subscription, stopMessage);
            }
        }

        await _subscriptionService.UpdateAllAsync(extendedSubscriptions);
        
        foreach (var subscription in extendedSubscriptions
                     .Where(subscription => !subscription.RenewalNotificationsDisabled))
        {
            await SendRenewalNotificationAsync(subscription);
        }
    }

    private async Task NotifyAboutEndIfNecessaryWithoutDbSaving(Subscription subscription)
    {
        if (subscription.EndNotificationsDisabled || 
            subscription.EndNotificationPerformed || 
            !IsItTimeForNotification(subscription.EndDateTime))
        {
            return;
        }

        subscription.EndNotificationPerformed = true;
        await _replyService.SendTextWithMainKeyboardAsync(subscription.User.TelegramUserId, subscription,
            $"Подписка скоро закончится. " +
            $"Дата окончания: {DateTimeUtils.GetSubEndString(subscription)}");
    }

    private bool IsItTimeForNotification(DateTime subscriptionEndDateTime)
    {
        var timeUntilEnd = DateTimeUtils.GetTimeUntilDateTime(subscriptionEndDateTime);
        return timeUntilEnd <= _timeUntilEndNotification;
    }
    
    private async Task SendRenewalNotificationAsync(Subscription subscription)
    {
        if (subscription.RenewalNotificationsDisabled)
            return;
        
        await _replyService.SendTextWithMainKeyboardAsync(subscription.User.TelegramUserId, subscription,
            $"Подписка продлена до {DateTimeUtils.GetSubEndString(subscription)}. " +
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
        subscription.EndDateTime = _dateTimeService.GetNewSubscriptionEndDateTime();
        subscription.HasExpired = false;
        subscription.EndNotificationPerformed = false;
        
        _withdrawService.CreateWithoutSaving(subscription);
        
        if (!user.OutlineKey.Enabled)
            await _outlineService.EnableKeyAsync(user.OutlineKey!.Id);
            
        LogSubscriptionExtension(user, subscriptionPrice);
    }

    private async Task StopSubscriptionWithoutDbSaveAsync(Subscription subscription)
    {
        var subscriptionPrice = subscription.SubscriptionPlan.CostInRoubles;
        var user = subscription.User;

        subscription.HasExpired = true;
        
        if (user.OutlineKey.Enabled)
            await _outlineService.DisableKeyAsync(user.OutlineKey!.Id);
        
        LogSubscriptionCancellation(user, subscriptionPrice);
    }

    public async Task RenewIfEnoughMoneyAsync(User user)
    {
        if (user.OutlineKey is null)
            await _outlineService.CreateOutlineKeyForUser(user);
        
        var subscription = user.Subscription;
        
        if (IsEnoughMoneyForRenewal(subscription))
        {
            await RenewSubscriptionWithoutDbSaveAsync(subscription);
            await _subscriptionService.UpdateAsync(subscription);
            await SendRenewalNotificationAsync(subscription);
        }
    }

    private void LogSubscriptionCancellation(User user, decimal subscriptionPrice)
    {
        _logger.LogInformation("Ключ отключён. " +
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
        _logger.LogInformation("Со счёта {TelegramUserName}({TelegramUserId}) " +
                              "списано {SubscriptionPrice} рублей. Осталось: {MoneyInRoubles}{CurrencySymbol}", 
            user.TelegramUserName, 
            user.TelegramUserId, 
            subscriptionPrice, 
            user.MoneyInRoubles,
            PaymentConstants.CurrencySymbol);
    }
}