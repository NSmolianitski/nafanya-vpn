using NafanyaVPN.Entities.Registration;
using NafanyaVPN.Entities.Telegram.Abstractions;
using NafanyaVPN.Entities.Telegram.CommandHandlers.DTOs;
using NafanyaVPN.Utils;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace NafanyaVPN.Entities.Telegram;

public class MessageReceiveService(
    ICommandHandlerService<Message> messageCommandHandlerService,
    ICommandHandlerService<CallbackQueryDto> callbackQueryCommandHandlerService,
    ITelegramStateService telegramStateService,
    ILogger<MessageReceiveService> logger,
    IUserRegistrationService userRegistrationService)
    : IUpdateHandler
{
    public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        var handler = update switch
        {
            { Message: { } message } => OnMessageReceived(message, cancellationToken),
            { CallbackQuery: { } callbackQuery } => OnCallbackQueryReceived(callbackQuery, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(update), update, null)
        };

        await handler;
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task OnMessageReceived(Message message, CancellationToken cancellationToken)
    {
        var telegramUserId = message.From!.Id;
        var isUserRegistered = await userRegistrationService.IsRegistered(telegramUserId);
        if (!isUserRegistered)
            await userRegistrationService.RegisterUser(telegramUserId, message.From.Username!);

        if (await telegramStateService.UserHasState(telegramUserId))
            await telegramStateService.HandleStateAsync(message);
        else
            await messageCommandHandlerService.HandleCommand(message);
    }

    private async Task OnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        string query;
        var data = string.Empty;
        var hasSplitSymbol = StringUtils.HasSplitSymbol(callbackQuery.Data!); 
        if (hasSplitSymbol)
        {
            var splitQuery = StringUtils.SplitBySymbol(callbackQuery.Data!);
            query = splitQuery[0];
            data = splitQuery[1];
        }
        else
        {
            query = callbackQuery.Data!;
        }
        var dto = new CallbackQueryDto(query, data, callbackQuery.Message!, callbackQuery.From);
        await callbackQueryCommandHandlerService.HandleCommand(dto);
    }
}