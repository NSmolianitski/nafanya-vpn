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

        string subscriptionStatusMessage;
        if (subscription.HasExpired)
            subscriptionStatusMessage = "истекла";
        else if (subscription.RenewalDisabled)
            subscriptionStatusMessage = "активна, продление отключено";
        else
            subscriptionStatusMessage = "активна, продление включено";
        
        var subscriptionMessage = user.Subscription.HasExpired 
            ? "-" 
            : user.Subscription.EndDateTime.ToString(CultureInfo.InvariantCulture);
        
        await replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, 
            $"Остаток средств: {user.MoneyInRoubles}{PaymentConstants.CurrencySymbol}\n" +
            $"Состояние подписки: {subscriptionStatusMessage}\n" +
            $"Следующее продление подписки: {subscriptionMessage}");
    }
}