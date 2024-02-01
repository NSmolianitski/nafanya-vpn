using NafanyaVPN.Services.Abstractions;
using Telegram.Bot.Types;

namespace NafanyaVPN.Services.CommandHandlers.Commands.Messages;

public class SendHelloCommand : ICommand<Message>
{
    private readonly IReplyService _replyService;

    public SendHelloCommand(IReplyService replyService)
    {
        _replyService = replyService;
    }

    public async Task Execute(Message message)
    {
        
        await _replyService.SendHelloAsync(message.Chat.Id);
    }
}