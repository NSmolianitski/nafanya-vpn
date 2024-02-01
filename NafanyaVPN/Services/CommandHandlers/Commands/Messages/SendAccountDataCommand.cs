using NafanyaVPN.Services.Abstractions;

namespace NafanyaVPN.Services.CommandHandlers.Commands.Messages;

public class SendAccountDataCommand : ICommand<Telegram.Bot.Types.Message>
{
    private readonly IReplyService _replyService;
    private readonly IUserService _userService;

    public SendAccountDataCommand(IReplyService replyService, IUserService userService)
    {
        _replyService = replyService;
        _userService = userService;
    }

    public async Task Execute(Telegram.Bot.Types.Message message)
    {
        var telegramUser = message.From;
        var user = await _userService.GetAsync(telegramUser!.Id);

        await _replyService.SendTextWithMainKeyboardAsync(message.Chat.Id, $"Остаток средств: {user.MoneyInRoubles}");
    }
}