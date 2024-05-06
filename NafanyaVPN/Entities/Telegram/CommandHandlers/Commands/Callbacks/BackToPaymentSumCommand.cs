﻿using NafanyaVPN.Entities.Telegram.Abstractions;
using NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Messages;
using NafanyaVPN.Entities.Telegram.CommandHandlers.DTOs;
using NafanyaVPN.Entities.Telegram.Constants;
using NafanyaVPN.Entities.Users;
using Telegram.Bot.Types.ReplyMarkups;

namespace NafanyaVPN.Entities.Telegram.CommandHandlers.Commands.Callbacks;

public class BackToPaymentSumCommand(IUserService userService, IReplyService replyService, ILogger<SendPaymentSumChooseCommand> logger)
    : ICommand<CallbackQueryDto>
{
    private readonly InlineKeyboardMarkup _replyMarkup = Markups.PaymentSum;
    private readonly ILogger<SendPaymentSumChooseCommand> _logger = logger;

    public async Task Execute(CallbackQueryDto data)
    {
        var user = await userService.GetAsync(data.User.Id);
        user.TelegramState = string.Empty;
        await userService.UpdateAsync(user);
        
        await replyService.EditMessageWithMarkupAsync(data.Message, "Выберите сумму:", _replyMarkup);
    }
}