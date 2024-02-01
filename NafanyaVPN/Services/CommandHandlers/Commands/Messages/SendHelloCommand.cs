using NafanyaVPN.Services.Abstractions;

namespace NafanyaVPN.Services.CommandHandlers.Commands.Messages;

public class SendHelloCommand : ICommand<Telegram.Bot.Types.Message>
{
    private readonly IReplyService _replyService;

    public SendHelloCommand(IReplyService replyService)
    {
        _replyService = replyService;
    }

    public async Task Execute(Telegram.Bot.Types.Message message)
    {
        await _replyService.SendHelloAsync(message.Chat.Id);
    }
}