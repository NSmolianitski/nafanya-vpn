using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Messages;

public class SendPaymentSumChooseCommand : ICommand<Message>
{
    private readonly IReplyService _replyService;
    private readonly InlineKeyboardMarkup _replyMarkup;
    private readonly ILogger<SendPaymentSumChooseCommand> _logger;

    public SendPaymentSumChooseCommand(IReplyService replyService, ILogger<SendPaymentSumChooseCommand> logger)
    {
        _replyService = replyService;
        _logger = logger;
        var firstRowButtons = new []
        {
            InlineKeyboardButton.WithCallbackData("100", "payment_sum+100"),
            InlineKeyboardButton.WithCallbackData("200", "payment_sum+200"),
            InlineKeyboardButton.WithCallbackData("300", "payment_sum+300")
        };
        var secondRowButtons = new []
        {
            InlineKeyboardButton.WithCallbackData("500", "payment_sum+500"),
            InlineKeyboardButton.WithCallbackData("1000", "payment_sum+1000"),
            InlineKeyboardButton.WithCallbackData("Ввести вручную", "enter_custom_payment_sum")
        };
        _replyMarkup = new InlineKeyboardMarkup(new [] { firstRowButtons, secondRowButtons });
    }

    public async Task Execute(Message message)
    {
        await _replyService.SendTextWithMarkupAsync(message.Chat.Id, "Выберите сумму:", _replyMarkup);
    }
}