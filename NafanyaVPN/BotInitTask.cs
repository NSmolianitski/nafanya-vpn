using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace NafanyaVPN;

public class BotInitTask(IServiceScopeFactory scopeFactory, ILogger<BotInitTask> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = scopeFactory.CreateScope();

        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
        var updateHandler = scope.ServiceProvider.GetRequiredService<IUpdateHandler>();
        
        logger.LogInformation("{BotName} запущен.", "Нафаня VPN");

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates =
            [
                UpdateType.Message,
                UpdateType.CallbackQuery,
                UpdateType.InlineQuery
            ]
        };
        
        await botClient.ReceiveAsync(
            updateHandler: updateHandler,
            receiverOptions: receiverOptions,
            cancellationToken: stoppingToken);
    }
}