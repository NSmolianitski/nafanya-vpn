using Microsoft.AspNetCore.Mvc;
using NafanyaVPN.Telegram.Abstractions;
using Telegram.Bot.Types;

namespace NafanyaVPN.Telegram;

[Route("/api/v1/bot")]
[ApiController]
public class BotController(ITelegramUpdatesHandlerService telegramUpdatesHandlerService) : ControllerBase
{
    [HttpGet]
    [Route("test")]
    public async Task<string> Test()
    {
        return "Ok";
    }
    
    [HttpPost]
    public async Task Post(Update update, CancellationToken cancellationToken)
    {
        await telegramUpdatesHandlerService.HandleUpdateAsync(update, cancellationToken);
    }
}