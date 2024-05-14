using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.DTOs;

namespace NafanyaVPN.Telegram.Commands.Messages;

public class HelloCommand(IReplyService replyService) : ICommand<MessageDto>
{
    public async Task Execute(MessageDto data)
    {
        await replyService.SendHelloAsync(data.Message.Chat.Id);
    }
}