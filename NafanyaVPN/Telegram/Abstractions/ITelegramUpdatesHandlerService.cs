using Telegram.Bot.Types;

namespace NafanyaVPN.Telegram.Abstractions;

public interface ITelegramUpdatesHandlerService
{
    Task HandleUpdateAsync(Update update, CancellationToken cancellationToken);
}