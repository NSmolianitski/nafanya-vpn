using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.DTOs;

namespace NafanyaVPN.Telegram.Commands.Messages;

public class BackToMainMenuCommand(
    IReplyService replyService) 
    : ICommand<MessageDto>
{
    public async Task Execute(MessageDto data)
    {
        await replyService.SendTextWithMainKeyboardAsync(data.Message.Chat.Id, data.User.Subscription, "Назад");
    }
}