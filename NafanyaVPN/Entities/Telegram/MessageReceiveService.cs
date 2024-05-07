using NafanyaVPN.Entities.Registration;
using NafanyaVPN.Entities.Telegram.Abstractions;
using NafanyaVPN.Entities.Telegram.CommandHandlers.DTOs;
using NafanyaVPN.Utils;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace NafanyaVPN.Entities.Telegram;

public class MessageReceiveService(
    ICommandHandlerService<MessageDto> messageCommandHandlerService,
    ICommandHandlerService<CallbackQueryDto> callbackQueryCommandHandlerService,
    ITelegramStateService telegramStateService,
    IReplyService replyService,
    ILogger<MessageReceiveService> logger,
    IUserRegistrationService userRegistrationService)
    : IUpdateHandler
{
    public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        try
        {
            var handler = update switch
            {
                { Message: { } message } => OnMessageReceived(message, cancellationToken),
                { CallbackQuery: { } callbackQuery } => OnCallbackQueryReceived(callbackQuery, cancellationToken),
                _ => throw new ArgumentOutOfRangeException(nameof(update), update, null)
            };

            await handler;
        }
        catch (Exception e)
        {
            logger.LogError("{Message}", e);
        }
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError("{Message}", exception);
        return Task.CompletedTask;
    }

    private async Task OnMessageReceived(Message message, CancellationToken cancellationToken)
    {
        await replyService.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
        
        var telegramUserId = message.From!.Id;
        var user = await userRegistrationService.GetIfRegisteredAsync(telegramUserId) 
                   ?? await userRegistrationService.RegisterUser(telegramUserId, message.From.Username!);

        var hasValidTelegramState = telegramStateService.UserHasState(user) 
                                    && telegramStateService.CommandExists(user.TelegramState);
        
        var messageDto = new MessageDto(message, user);
        if (hasValidTelegramState)
            await telegramStateService.HandleStateAsync(messageDto);
        else
            await messageCommandHandlerService.HandleCommand(messageDto);
    }

    private async Task OnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        await replyService.SendChatActionAsync(callbackQuery.Message!.Chat.Id, ChatAction.Typing);
        
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