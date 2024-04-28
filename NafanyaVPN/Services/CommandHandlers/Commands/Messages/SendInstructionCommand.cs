using NafanyaVPN.Services.Abstractions;

namespace NafanyaVPN.Services.CommandHandlers.Commands.Messages;

public class SendInstructionCommand(IReplyService replyService) : ICommand<Telegram.Bot.Types.Message>
{
    public async Task Execute(Telegram.Bot.Types.Message message)
    {
        var instruction = "Текст инструкции";
        await replyService.SendTextWithMainKeyboardAsync(message.Chat.Id, $"{instruction}");
    }
}