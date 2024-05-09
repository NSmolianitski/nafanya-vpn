using NafanyaVPN.Entities.Users;
using NafanyaVPN.Telegram.Abstractions;
using NafanyaVPN.Telegram.Constants;
using NafanyaVPN.Telegram.DTOs;
using NafanyaVPN.Utils;
using Telegram.Bot.Types.ReplyMarkups;

namespace NafanyaVPN.Telegram.Commands.Callbacks;

public class CustomPaymentSumCommand(
    IUserService userService,
    IReplyService replyService,
    ILogger<CustomPaymentSumCommand> logger)
    : ICommand<CallbackQueryDto>
{
    private readonly InlineKeyboardMarkup _replyMarkup = InlineMarkups.CustomPaymentSum;
    private readonly InlineKeyboardMarkup _replyMarkupHighBorder = InlineMarkups.CustomPaymentSumHighBorder;
    private readonly InlineKeyboardMarkup _replyMarkupLowBorder = InlineMarkups.CustomPaymentSumLowBorder;
    private readonly ILogger<CustomPaymentSumCommand> _logger = logger;
    
    private const decimal MinPaymentSum = 2;
    private const decimal MaxPaymentSum = 5000;
    private const string CurrencyShort = "руб.";

    public async Task Execute(CallbackQueryDto data)
    {
        var user = await userService.GetAsync(data.User.Id);

        var currentSum = GetCurrentSum(user.TelegramState, data.Payload);

        var answerText = $"Выбранная сумма: {currentSum} {CurrencyShort}";
        if (answerText.Equals(data.Message.Text)) // На случай, если сумма не изменилась (иначе будет Exception)
            return;
        
        user.TelegramState = $"{CallbackConstants.CustomPaymentSum}" +
                             $"{CallbackConstants.SplitSymbol}" +
                             $"{currentSum}";
        await userService.UpdateAsync(user);
        
        PrepareResponseData(currentSum, ref answerText, out var replyMarkup);

        await replyService.EditMessageWithMarkupAsync(data.Message, answerText, replyMarkup);
    }

    private decimal GetCurrentSum(string telegramState, string payload)
    {
        decimal currentSum;
        if (string.IsNullOrWhiteSpace(telegramState))
        {
            currentSum = 0;
        }
        else
        {
            currentSum = StringUtils.GetPaymentSumFromTelegramState(telegramState);
            currentSum += StringUtils.ParseSum(payload);
        }

        return currentSum switch
        {
            < 0 => 0,
            > MaxPaymentSum => MaxPaymentSum,
            _ => currentSum
        };
    }

    private void PrepareResponseData(decimal currentSum, ref string answerText, out InlineKeyboardMarkup replyMarkup)
    {
        replyMarkup = currentSum switch
        {
            <= MinPaymentSum => _replyMarkupLowBorder,
            >= MaxPaymentSum => _replyMarkupHighBorder,
            _ => _replyMarkup
        };
        
        var currentSumBorderedOrIncorrect = currentSum is <= MinPaymentSum or >= MaxPaymentSum;
        if (currentSumBorderedOrIncorrect)
        {
            answerText = $"Сумма должна быть от {MinPaymentSum} до {MaxPaymentSum}. " +
                         $"Текущая сумма: {currentSum} {CurrencyShort}";
        }
    }
}