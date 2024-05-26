using NafanyaVPN.Telegram.Constants;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace NafanyaVPN;

public class BotModule(
    IConfiguration configuration,
    IServiceScopeFactory scopeFactory, 
    ILogger<BotModule> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = scopeFactory.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        var telegramConfig = configuration.GetRequiredSection(TelegramConstants.SettingsSectionName);
        var host = telegramConfig[TelegramConstants.WebHookUrl]!;
        var route = telegramConfig[TelegramConstants.WebHookRoute]!;
        var secret = telegramConfig[TelegramConstants.WebHookSecret]!;
        
        logger.LogCritical("{BotName} запущен.", "Нафаня VPN");

        await botClient.SetWebhookAsync(
            url: $"{host}{route}",
            dropPendingUpdates: true,
            allowedUpdates: [
                UpdateType.Message,
                UpdateType.CallbackQuery,
                UpdateType.InlineQuery
            ],
            secretToken: secret,
            cancellationToken: stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
        
        logger.LogCritical("{BotName} остановлен.", "Нафаня VPN");
        await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
    }
}