using NafanyaVPN.Services.Abstractions;
using NafanyaVPN.Services.CommandHandlers;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace NafanyaVPN.Services;

public class MessageReceiveService : IUpdateHandler
{
    private readonly ICommandHandlerService<Message> _messageCommandHandlerService;
    private readonly ICommandHandlerService<CallbackQueryDto> _callbackQueryCommandHandlerService;
    private readonly IUserRegistrationService _userRegistrationService;
    private readonly ITelegramStateService _telegramStateService;
    private readonly ILogger<MessageReceiveService> _logger;

    public MessageReceiveService(
        ICommandHandlerService<Message> messageCommandHandlerService, 
        ICommandHandlerService<CallbackQueryDto> callbackQueryCommandHandlerService, 
        ITelegramStateService telegramStateService, 
        ILogger<MessageReceiveService> logger, IUserRegistrationService userRegistrationService)
    {
        _messageCommandHandlerService = messageCommandHandlerService;
        _callbackQueryCommandHandlerService = callbackQueryCommandHandlerService;
        _userRegistrationService = userRegistrationService;
        _telegramStateService = telegramStateService;
        _logger = logger;
    }

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
        var isUserRegistered = await _userRegistrationService.IsRegistered(telegramUserId);
        if (!isUserRegistered)
            await _userRegistrationService.RegisterUser(telegramUserId, message.From.Username!);

        if (await _telegramStateService.UserHasState(telegramUserId))
            await _telegramStateService.HandleStateAsync(message);
        else
            await _messageCommandHandlerService.HandleCommand(message);
    }

    private async Task OnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        string query;
        string data = string.Empty;
        if (!callbackQuery.Data!.Contains('+'))
        {
            query = callbackQuery.Data;
        }
        else
        {
            var splitQuery = callbackQuery.Data!.Split('+');
            query = splitQuery[0];
            data = splitQuery[1];
        }
        var dto = new CallbackQueryDto(query, data, callbackQuery.Message!, callbackQuery.From);
        await _callbackQueryCommandHandlerService.HandleCommand(dto);
    }
}