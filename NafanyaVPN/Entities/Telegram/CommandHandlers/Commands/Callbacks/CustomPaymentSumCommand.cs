using NafanyaVPN.Entities.Telegram.Abstractions;
using NafanyaVPN.Entities.Telegram.CommandHandlers.DTOs;
using NafanyaVPN.Entities.Telegram.Constants;
using NafanyaVPN.Entities.Users;
using NafanyaVPN.Utils;
using Telegram.Bot.Types.ReplyMarkups;

namespace NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Callbacks;

public class CustomPaymentSumCommand(
    IUserService userService,
    IReplyService replyService,
    ILogger<CustomPaymentSumCommand> logger)
    : ICommand<CallbackQueryDto>
{
    private readonly InlineKeyboardMarkup _replyMarkup = Markups.CustomPaymentSum;
    private readonly ILogger<CustomPaymentSumCommand> _logger = logger;

    public async Task Execute(CallbackQueryDto data)
    {
        var user = await userService.GetAsync(data.User.Id);

        decimal currentSum;
        if (string.IsNullOrWhiteSpace(user.TelegramState))
        {
            currentSum = 0;
        }
        else
        {
            currentSum = StringUtils.GetPaymentSumFromTelegramState(user.TelegramState);
            currentSum += StringUtils.ParseSum(data.Payload);
        }

        if (currentSum < 0)
            currentSum = 0;

        var answerText = $"Выбранная сумма: {currentSum}";

        user.TelegramState = $"{CallbackConstants.CustomPaymentSum}" +
                             $"{CallbackConstants.SplitSymbol}" +
                             $"{currentSum}";
        await userService.UpdateAsync(user);

        if (answerText.Equals(data.Message.Text)) // На случай, если сумма не изменилась (иначе будет Exception)
            return;
        
        await replyService.EditMessageWithMarkupAsync(data.Message, answerText, _replyMarkup);
    }
}