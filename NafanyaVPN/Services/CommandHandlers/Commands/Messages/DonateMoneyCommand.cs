using NafanyaVPN.Services.Abstractions;
using Telegram.Bot.Types;

namespace NafanyaVPN.Services.CommandHandlers.Commands.Messages;

public class DonateMoneyCommand : ICommand<Message>
{
    private readonly IPaymentService _paymentService;
    private readonly IReplyService _replyService;
    private readonly IUserService _userService;
    private readonly ILogger<DonateMoneyCommand> _logger;

    public DonateMoneyCommand(IReplyService replyService, IUserService userService, IPaymentService paymentService, 
        ILogger<DonateMoneyCommand> logger)
    {
        _replyService = replyService;
        _userService = userService;
        _logger = logger;
        _paymentService = paymentService;
    }

    public async Task Execute(Message message)
    {
        var telegramUser = message.From;
        
        var user = await _userService.GetAsync(telegramUser!.Id);
        const int sum = 100;
        user.MoneyInRoubles += sum;

        await _userService.UpdateAsync(user);

        _logger.LogInformation($"{user.TelegramUserName}({user.TelegramUserId}) пополнил счёт на {sum}. " +
                               $"Текущий баланс: {user.MoneyInRoubles}");
        
        await _replyService.SendTextWithMainKeyboardAsync(message.Chat.Id,
            $"Счёт пополнен. Остаток средств: {user.MoneyInRoubles} рублей.");
    }
}