﻿using NafanyaVPN.Entities.SubscriptionPlans;
using NafanyaVPN.Entities.Subscriptions;
using NafanyaVPN.Entities.Users;
using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.Constants;
using NafanyaVPN.Telegram.DTOs;

namespace NafanyaVPN.Telegram.Commands.Callbacks;

public class ToggleRenewalCommand(
    IUserService userService, 
    ISubscriptionService subscriptionService, 
    ISubscriptionRenewService subscriptionRenewService, 
    IReplyService replyService)
    : ICommand<CallbackQueryDto>
{
    public async Task Execute(CallbackQueryDto data)
    {
        var user = await userService.GetByTelegramIdAsync(data.User.Id);
        var subscription = user.Subscription;
        subscription.RenewalDisabled = !subscription.RenewalDisabled;
        
        await subscriptionService.UpdateAsync(subscription);
        var replyMarkup = InlineMarkups.CreateSettingsMarkup(subscription.RenewalDisabled,
            subscription.RenewalNotificationsDisabled, subscription.EndNotificationsDisabled);
        
        await replyService.EditMessageWithMarkupAsync(data.Message, MainKeyboardConstants.Settings, replyMarkup);

        // Обновление подписки в случае, если пользователь включает автопродление и подписка истекла
        if (subscription is { HasExpired: true, RenewalDisabled: false })
            await subscriptionRenewService.RenewIfEnoughMoneyAsync(user);
    }
}