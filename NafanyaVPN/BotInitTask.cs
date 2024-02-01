using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace NafanyaVPN;

public class BotInitTask : BackgroundService
{
    private readonly ILogger<BotInitTask> _logger;

    private readonly IServiceScopeFactory _scopeFactory;

    public BotInitTask(IServiceScopeFactory scopeFactory, ILogger<BotInitTask> logger)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
        var updateHandler = scope.ServiceProvider.GetRequiredService<IUpdateHandler>();
        
        _logger.LogInformation("{BotName} запущен.", "Нафаня VPN");

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new[]
            {
                UpdateType.Message,
                UpdateType.CallbackQuery,
                UpdateType.InlineQuery
            }
        };
        
        await botClient.ReceiveAsync(
            updateHandler: updateHandler,
            receiverOptions: receiverOptions,
            cancellationToken: stoppingToken);
    }
}