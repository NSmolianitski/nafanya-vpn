using NafanyaVPN.Services.Abstractions;
using Telegram.Bot.Types;

namespace NafanyaVPN.Services.CommandHandlers.Commands.Messages;

public class SendHelloCommand(IReplyService replyService) : ICommand<Message>
{
    public async Task Execute(Message message)
    {
        
        await replyService.SendHelloAsync(message.Chat.Id);
    }
}