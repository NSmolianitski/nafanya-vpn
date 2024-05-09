using System.Globalization;
using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.DTOs;

namespace NafanyaVPN.Telegram.Commands.Messages;

public class SendAccountDataCommand(IReplyService replyService)
    : ICommand<MessageDto>
{
    public async Task Execute(MessageDto data)
    {
        var user = data.User;
        
        string subscriptionMessage;
        if (user.OutlineKey == null || !user.OutlineKey!.Enabled) // TODO: заменить OutlineKey на Subscription
            subscriptionMessage = "Подписка неактивна";
        else
            subscriptionMessage = user.Subscription.EndDateTime.ToString(CultureInfo.InvariantCulture);
        
        await replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, 
            $"Остаток средств: {user.MoneyInRoubles}\n" +
            $"Следующее обновление подписки: {subscriptionMessage}");
    }
}