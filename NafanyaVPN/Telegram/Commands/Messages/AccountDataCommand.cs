using System.Globalization;
using NafanyaVPN.Entities.Payments;
using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.Constants;
using NafanyaVPN.Telegram.DTOs;
using NafanyaVPN.Utils;

namespace NafanyaVPN.Telegram.Commands.Messages;

public class AccountDataCommand(IReplyService replyService)
    : ICommand<MessageDto>
{
    public async Task Execute(MessageDto data)
    {
        var user = data.User;
        var subscription = user.Subscription;

        var statusMessage = subscription.HasExpired
            ? "🔴 отключена"
            : "🟢 активна";

        var renewalMessage = subscription.RenewalDisabled
            ? "⏹ отключено"
            : "🔄 включено";
        
        var renewalDate = subscription.HasExpired
            ? "-" 
            : DateTimeUtils.GetSubEndString(subscription);
        
        await replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, user.Subscription, 
            $"<b>Остаток средств:</b> 💰 {user.MoneyInRoubles}{PaymentConstants.CurrencySymbol}\n" +
            $"<b>Состояние подписки:</b> {statusMessage}\n" +
            $"<b>Автопродление подписки:</b> {renewalMessage}\n" +
            $"<b>Стоимость подписки (за 30 дней):</b> 🎟️ {subscription.SubscriptionPlan.CostInRoubles}" +
            $"{PaymentConstants.CurrencySymbol}\n" +
            $"<b>Окончание подписки:</b> 🗓 {renewalDate}");
    }
}