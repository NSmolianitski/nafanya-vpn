using Telegram.Bot.Types;

namespace NafanyaVPN.Entities.Telegram.Abstractions;

public interface ITelegramUpdatesHandlerService
{
    Task HandleUpdateAsync(Update update, CancellationToken cancellationToken);
}