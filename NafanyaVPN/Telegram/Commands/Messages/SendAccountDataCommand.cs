using System.Globalization;
using NafanyaVPN.Entities.Payments;
using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.DTOs;

namespace NafanyaVPN.Telegram.Commands.Messages;

public class SendAccountDataCommand(IReplyService replyService)
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
        
        var renewalDate = subscription.HasExpired || subscription.RenewalDisabled
            ? "-" 
            : user.Subscription.EndDateTime.ToString("HH:mm dd/MM/yyyy", CultureInfo.InvariantCulture);
        
        await replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, 
            $"<b>Остаток средств:</b> 💰 {user.MoneyInRoubles}{PaymentConstants.CurrencySymbol}\n" +
            $"<b>Состояние подписки:</b> {statusMessage}\n" +
            $"<b>Продление подписки:</b> {renewalMessage}\n" +
            $"<b>Стоимость подписки (за 30 дней):</b> 🔖 {subscription.SubscriptionPlan.CostInRoubles}\n" +
            $"<b>Следующее продление подписки:</b> 📅 {renewalDate}");
    }
}