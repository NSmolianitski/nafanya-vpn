using Microsoft.AspNetCore.Mvc;
using NafanyaVPN.Entities.Telegram.Abstractions;
using Telegram.Bot.Types;

namespace NafanyaVPN.Entities.Telegram;

[Route("/api/v1/bot")]
[ApiController]
public class BotController(ITelegramUpdatesHandlerService telegramUpdatesHandlerService) : ControllerBase
{
    [HttpPost]
    public async Task Post(Update update, CancellationToken cancellationToken)
    {
        await telegramUpdatesHandlerService.HandleUpdateAsync(update, cancellationToken);
    }
}