using NafanyaVPN.Entities.Outline;
using NafanyaVPN.Entities.Payments;
using NafanyaVPN.Entities.SubscriptionPlans;
using NafanyaVPN.Entities.Users;
using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.DTOs;

namespace NafanyaVPN.Telegram.Commands.Messages;

public class OutlineKeyCommand(
    IReplyService replyService,
    IOutlineService outlineService,
    ISubscriptionDateTimeService subscriptionDateTimeService,
    ISubscriptionExtendService subscriptionExtendService)
    : ICommand<MessageDto>
{
    public async Task Execute(MessageDto data)
    {
        var user = data.User;
        if (user.OutlineKey is null)
        {
            await outlineService.CreateOutlineKeyForUser(user);
            await subscriptionExtendService.TryRenewForUserAsync(user);
        }
        
        if (!subscriptionDateTimeService.HasSubscriptionExpired(user.Subscription))
        {
            await replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, $"{user.OutlineKey!.AccessUrl}");
        }
        else
        {
            await replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id,
                $"Ваш ключ временно отключён из-за недостатка средств. На счёте: {user.MoneyInRoubles}{PaymentConstants.CurrencySymbol}");
        }
    }
}