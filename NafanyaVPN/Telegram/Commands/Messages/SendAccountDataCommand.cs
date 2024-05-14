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
            ? "\ud83d\udd34 отключена"
            : "\ud83d\udfe2 активна";

        var renewalMessage = subscription.RenewalDisabled
            ? "\u23f9 отключено"
            : "\ud83d\udd04 включено";
        
        var renewalDate = subscription.HasExpired || subscription.RenewalDisabled
            ? "-" 
            : user.Subscription.EndDateTime.ToString("HH:mm dd/MM/yyyy", CultureInfo.InvariantCulture);
        
        await replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, 
            $"<b>Остаток средств:</b> \ud83d\udcb0{user.MoneyInRoubles}{PaymentConstants.CurrencySymbol}\n" +
            $"<b>Состояние подписки:</b> {statusMessage}\n" +
            $"<b>Продление подписки:</b> {renewalMessage}\n" +
            $"<b>Следующее продление подписки:</b> {renewalDate}");
    }
}