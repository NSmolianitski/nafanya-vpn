using NafanyaVPN.Services.Abstractions;

namespace NafanyaVPN.Services.CommandHandlers.Commands.Messages;

public class SendInstructionCommand : ICommand<Telegram.Bot.Types.Message>
{
    private readonly IReplyService _replyService;

    public SendInstructionCommand(IReplyService replyService)
    {
        _replyService = replyService;
    }

    public async Task Execute(Telegram.Bot.Types.Message message)
    {
        var instruction = "Текст инструкции";
        await _replyService.SendTextWithMainKeyboardAsync(message.Chat.Id, $"{instruction}");
    }
}